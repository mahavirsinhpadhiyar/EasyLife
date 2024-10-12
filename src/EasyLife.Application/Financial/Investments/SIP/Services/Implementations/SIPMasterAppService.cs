using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using EasyLife.Authorization;
using EasyLife.Financial.Investment;
using EasyLife.Financial.Investments.SIP.Dto.SIPMaster;
using EasyLife.Financial.Investments.SIP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.SIP.Services.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Financial_Investment_SIP)]
    public class SIPMasterAppService : AsyncCrudAppService<EL_Financial_Investment_SIP_Master, CreateOrEditSIPMasterDto, Guid, PagedSIPMasterResultRequestDto, CreateOrEditSIPMasterDto, CreateOrEditSIPMasterDto>, ISIPMasterAppService
    {
        private readonly IRepository<EL_Financial_Investment_SIP_Master, Guid> _investmentSIPMaster;
        private readonly IRepository<EL_Financial_Investment_SIP_Entry, Guid> _investmentSIPEntry;

        public SIPMasterAppService(IRepository<EL_Financial_Investment_SIP_Master, Guid> investmentSIPMaster,
            IRepository<EL_Financial_Investment_SIP_Entry, Guid> investmentSIPEntry
            ) : base(investmentSIPMaster)
        {
            _investmentSIPMaster = investmentSIPMaster;
            _investmentSIPEntry = investmentSIPEntry;
        }

        /// <summary>
        /// Will return the list of paging wise investment master entries with filtered result of SIP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<CreateOrEditSIPMasterDto>> GetAllFiltered(PagedSIPMasterResultRequestDto input)
        {
            var sipMasterList = new List<EL_Financial_Investment_SIP_Master>();
            if (input.FilterUserId == null || input.FilterUserId == 0)
                input.FilterUserId = AbpSession.UserId;
            var query = _investmentSIPMaster.GetAll().Where(x => x.UserId == input.FilterUserId);

            query = ApplySorting(query, input);

            sipMasterList = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.SIP_Name.Contains(input.Keyword))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(e => e.SIP_Name)
                .ToList();

            var pageCount = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.SIP_Name.Contains(input.Keyword))
                .OrderBy(e => e.SIP_Name)
                .Count();

            var sipMasterFilterList = sipMasterList.Select(r => new CreateOrEditSIPMasterDto()
            {
                SIP_Name = r.SIP_Name,
                Id = r.Id
            }).OrderBy(r => r.SIP_Name).ToList();

            ////foreach (var shareMaster in sipMasterFilterList)
            ////{
            ////    var shareMasterAverageDetails = CalculateAverageSharePrice(shareMaster.Id);
            ////    shareMaster.TotalAveragePriceDisplay = shareMasterAverageDetails.TotalAveragePriceDisplay;
            ////    shareMaster.TotalShareDisplay = shareMasterAverageDetails.TotalShareDisplay;
            ////    shareMaster.TotalEarnedOrLossDisplay = shareMasterAverageDetails.TotalEarnedOrLossDisplay;
            ////    shareMaster.TotalInvestedDisplay = shareMasterAverageDetails.TotalInvestedDisplay;
            ////    shareMaster.TotalEarnedOrLoss = shareMasterAverageDetails.TotalEarnedOrLoss;
            ////}

            var result = new PagedResultDto<CreateOrEditSIPMasterDto>(pageCount, sipMasterFilterList);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Create sip master entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditSIPMasterDto> CreateAsync(CreateOrEditSIPMasterDto input)
        {
            try
            {
                //DateTime dt = DateTime.Now;
                //input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
                input.UserId = AbpSession.UserId.Value;
                var sipMasterDetail = ObjectMapper.Map<EL_Financial_Investment_SIP_Master>(input);
                await _investmentSIPMaster.InsertAsync(sipMasterDetail);
                return MapToEntityDto(sipMasterDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update sip master
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditSIPMasterDto> UpdateAsync(CreateOrEditSIPMasterDto input)
        {
            try
            {
                //For duplicate entry
                if (input.Id == Guid.Empty)
                {
                    input.UserId = AbpSession.UserId.Value;
                    var sipMasterDetail = ObjectMapper.Map<EL_Financial_Investment_SIP_Master>(input);
                    sipMasterDetail.Id = await _investmentSIPMaster.InsertAndGetIdAsync(sipMasterDetail);
                    return MapToEntityDto(sipMasterDetail);
                }
                else
                {
                    //input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
                    input.UserId = AbpSession.UserId.Value;
                    var shareMasterDetail = await _investmentSIPMaster.GetAsync(input.Id);
                    ObjectMapper.Map(input, shareMasterDetail);
                    await _investmentSIPMaster.UpdateAsync(shareMasterDetail);
                    return MapToEntityDto(shareMasterDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            }

        /// <summary>
        /// Edit sip master entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <returns></returns>
        public async Task<CreateOrEditSIPMasterDto> GetSIPMasterForEdit(EntityDto<Guid> input)
        {
            var output = await _investmentSIPMaster.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditSIPMasterDto>(output);
            return ClassObj;
        }

        /// <summary>
        /// Returns the final average price of buy/bonus shares etc...
        /// Update this function whenever same function in shareorder service updates.
        /// </summary>
        /// <param name="filterUserId"></param>
        /// <returns></returns>
        public async Task<string> CalculateTotalInvested(long? FilterUserId)
        {
            double totalInvested = 0;
            try
            {
                if (FilterUserId == null || FilterUserId == 0)
                    FilterUserId = AbpSession.UserId;

                var shareOrders = await _investmentSIPMaster.GetAll().Include(s => s.EL_Financial_Investment_SIP_Entries).Where(s => s.UserId == FilterUserId && s.EL_Financial_Investment_SIP_Entries.Count() > 0).ToListAsync();

                foreach (var shareOrder in shareOrders)
                {
                    totalInvested += shareOrder.EL_Financial_Investment_SIP_Entries.Sum(o => o.SIP_Amount);
                }

                return totalInvested.ToLocalMoneyFormat();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
    }
}
