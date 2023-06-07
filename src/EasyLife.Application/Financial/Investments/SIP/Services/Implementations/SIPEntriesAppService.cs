using Abp.Application.Services;
using Abp.Authorization;
using EasyLife.Authorization;
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
using EasyLife.Financial.Investments.SIP.Dto.SIPEntry;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using Abp.Extensions;
using AutoMapper;

namespace EasyLife.Financial.Investments.SIP.Services.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Financial_Investment_SIP)]
    public class SIPEntriesAppService : AsyncCrudAppService<EL_Financial_Investment_SIP_Entry, CreateOrEditSIPEntriesDto, Guid, PagedSIPEntriesResultRequestDto, CreateOrEditSIPEntriesDto, CreateOrEditSIPEntriesDto>, ISIPEntriesAppService
    {
        private readonly IRepository<EL_Financial_Investment_SIP_Master, Guid> _investmentSIPMaster;
        private readonly IRepository<EL_Financial_Investment_SIP_Entry, Guid> _investmentSIPEntry;

        public SIPEntriesAppService(IRepository<EL_Financial_Investment_SIP_Master, Guid> investmentSIPMaster,
            IRepository<EL_Financial_Investment_SIP_Entry, Guid> investmentSIPEntry
            ) : base(investmentSIPEntry)
        {
            _investmentSIPMaster = investmentSIPMaster;
            _investmentSIPEntry = investmentSIPEntry;
        }

        /// <summary>
        /// Will return the list of paging wise sip entries with filtered result of respect to sip master
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>defaulok
        public Task<PagedResultDto<CreateOrEditSIPEntriesDto>> GetAllFiltered(PagedSIPEntriesResultRequestDto input)
        {
            var sipEntryList = new List<EL_Financial_Investment_SIP_Entry>();
            var query = _investmentSIPEntry.GetAll().Where(x => x.EL_Financial_Investment_SIP_Master_Id == Guid.Parse(input.SIPMasterId));

            query = ApplySorting(query, input);

            sipEntryList = query
                .WhereIf(input.FilterStartDate.HasValue && input.FilterEndDate.HasValue, x => x.SIP_Order_Date >= input.FilterStartDate.Value && x.SIP_Order_Date <= input.FilterEndDate.Value)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(e => e.SIP_Order_Date)
                .ToList();

            var pageCount = query
                .WhereIf(input.FilterStartDate.HasValue && input.FilterEndDate.HasValue, x => x.SIP_Order_Date >= input.FilterStartDate.Value && x.SIP_Order_Date <= input.FilterEndDate.Value)
                .OrderByDescending(e => e.SIP_Order_Date)
                .Count();

            var sipEntriesFilterList = sipEntryList.Select(r => new CreateOrEditSIPEntriesDto()
            {
                SIP_Order_Date = r.SIP_Order_Date,
                SIP_Amount = r.SIP_Amount,
                SIP_Average_Price = r.SIP_Average_Price,
                SIP_Order_Id = r.SIP_Order_Id,
                SIP_Units = r.SIP_Units,
                EL_Financial_Investment_SIP_Master_Id = r.EL_Financial_Investment_SIP_Master_Id,
                Id = r.Id,
                SIPType = r.SIPType
            }).ToList();

            var result = new PagedResultDto<CreateOrEditSIPEntriesDto>(pageCount, sipEntriesFilterList);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Overridden ApplySorting
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<EL_Financial_Investment_SIP_Entry> ApplySorting(IQueryable<EL_Financial_Investment_SIP_Entry> query, PagedSIPEntriesResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty() || input.Sorting == "0 asc" || input.Sorting.Contains("SIP_Order_Date"))
            {
                input.Sorting = input.Sorting.Contains("desc") ? "SIP_Order_Date desc" : "SIP_Order_Date asc";
            }
            return base.ApplySorting(query, input);
        }

        public override async Task<CreateOrEditSIPEntriesDto> CreateAsync(CreateOrEditSIPEntriesDto input)
        {
            try
            {
                var sipEntriesDetail = ObjectMapper.Map<EL_Financial_Investment_SIP_Entry>(input);
                await _investmentSIPEntry.InsertAsync(sipEntriesDetail);
                return MapToEntityDto(sipEntriesDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<PagedResultDto<CreateOrEditShareOrdersDto>> GetAllAsync(PagedShareOrdersResultRequestDto input)
        {
            throw new NotImplementedException();
        }

        public override async Task<CreateOrEditSIPEntriesDto> UpdateAsync(CreateOrEditSIPEntriesDto input)
        {
            try
            {
                //For duplicate entry
                if (input.Id == Guid.Empty)
                {

                    var sipEntriesDetail = ObjectMapper.Map<EL_Financial_Investment_SIP_Entry>(input);
                    sipEntriesDetail.Id = await _investmentSIPEntry.InsertAndGetIdAsync(sipEntriesDetail);
                    await _investmentSIPEntry.InsertAsync(sipEntriesDetail);
                    return MapToEntityDto(sipEntriesDetail);
                }
                else
                {
                    var sipEntriesDetail = await _investmentSIPEntry.GetAsync(input.Id);
                    ObjectMapper.Map(input, sipEntriesDetail);
                    await _investmentSIPEntry.UpdateAsync(sipEntriesDetail);
                    return MapToEntityDto(sipEntriesDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit sip orders entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreateOrEditSIPEntriesDto> GetSIPEntryForEdit(EntityDto<Guid> input)
        {
            var output = await _investmentSIPEntry.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditSIPEntriesDto>(output);
            return ClassObj;
        }

        /// <summary>
        /// Deletes the sip entries
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            var shareOrdersDetails = await _investmentSIPEntry.GetAsync(input.Id);
            await _investmentSIPEntry.HardDeleteAsync(shareOrdersDetails);
        }
    }
}
