using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using EasyLife.Authorization;
using EasyLife.Financial.Investment;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using EasyLife.Financial.Investments.ShareMarket.Services.Interfaces;
using EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.ShareMarket.Services.Implementation
{
    [AbpAuthorize(PermissionNames.Pages_Financial_Investment_ShareMarket)]
    public class ShareOrdersAppService : AsyncCrudAppService<EL_Financial_Investment_Share_Orders, CreateOrEditShareOrdersDto, Guid, PagedShareOrdersResultRequestDto, CreateOrEditShareOrdersDto, CreateOrEditShareOrdersDto>, IShareOrdersAppService
    {
        private readonly IRepository<EL_Financial_Investment_Share_Orders, Guid> _investmentShareOrders;
        public ShareOrdersAppService(IRepository<EL_Financial_Investment_Share_Orders, Guid> investmentShareOrders) : base(investmentShareOrders)
        {
            _investmentShareOrders = investmentShareOrders;
        }

        /// <summary>
        /// Will return the list of paging wise investment orders entries with filtered result of respected share master
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>defaulok
        public Task<PagedResultDto<CreateOrEditShareOrdersDto>> GetAllFiltered(PagedShareOrdersResultRequestDto input)
        {
            var shareMasterList = new List<EL_Financial_Investment_Share_Orders>();
            var query = _investmentShareOrders.GetAll().Where(x => x.EL_Financial_Investment_Share_Master_Id == Guid.Parse(input.ShareMasterId));

            query = ApplySorting(query, input);

            shareMasterList = query
                .WhereIf(input.FilterStartDate.HasValue && input.FilterEndDate.HasValue, x => x.Share_Order_Date >= input.FilterStartDate.Value && x.Share_Order_Date <= input.FilterEndDate.Value)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(e => e.Share_Order_Date)
                .ToList();

            var pageCount = query
                .WhereIf(input.FilterStartDate.HasValue && input.FilterEndDate.HasValue, x => x.Share_Order_Date >= input.FilterStartDate.Value && x.Share_Order_Date <= input.FilterEndDate.Value)
                .OrderByDescending(e => e.Share_Order_Date)
                .Count();

            var shareOrdersFilterList = shareMasterList.Select(r => new CreateOrEditShareOrdersDto()
            {
                Share_Order_Date = r.Share_Order_Date,
                Share_Amount = r.Share_Amount,
                Share_Average_Price = r.Share_Average_Price,
                Share_Order_Id = r.Share_Order_Id,
                Share_Order_Type = r.Share_Order_Type,
                Share_Price_Type = r.Share_Price_Type,
                Share_Qty_Exchange_Type = r.Share_Qty_Exchange_Type,
                Share_Quantity = r.Share_Quantity,
                Share_Transaction_Type = r.Share_Transaction_Type,
                EL_Financial_Investment_Share_Master_Id = r.EL_Financial_Investment_Share_Master_Id,
                Id = r.Id,
                Share_App_Order_Id = r.Share_App_Order_Id
            }).ToList();

            var result = new PagedResultDto<CreateOrEditShareOrdersDto>(pageCount, shareOrdersFilterList);
            return Task.FromResult(result);
        }
        public override async Task<CreateOrEditShareOrdersDto> CreateAsync(CreateOrEditShareOrdersDto input)
        {
            try
            {
                var shareOrdersDetail = ObjectMapper.Map<EL_Financial_Investment_Share_Orders>(input);
                await _investmentShareOrders.InsertAsync(shareOrdersDetail);
                return MapToEntityDto(shareOrdersDetail);
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

        public async Task<CreateOrEditShareOrdersDto> GetShareOrderForEdit(EntityDto<Guid> input)
        {
            var output = await _investmentShareOrders.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditShareOrdersDto>(output);
            return ClassObj;
        }

        public override async Task<CreateOrEditShareOrdersDto> UpdateAsync(CreateOrEditShareOrdersDto input)
        {
            try
            {
                //For duplicate entry
                if (input.Id == Guid.Empty)
                {

                    var shareOrdersDetail = ObjectMapper.Map<EL_Financial_Investment_Share_Orders>(input);
                    shareOrdersDetail.Id = await _investmentShareOrders.InsertAndGetIdAsync(shareOrdersDetail);
                    await _investmentShareOrders.InsertAsync(shareOrdersDetail);
                    return MapToEntityDto(shareOrdersDetail);
                }
                else
                {
                    var shareOrdersDetail = await _investmentShareOrders.GetAsync(input.Id);
                    ObjectMapper.Map(input, shareOrdersDetail);
                    await _investmentShareOrders.UpdateAsync(shareOrdersDetail);
                    return MapToEntityDto(shareOrdersDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Task<CreateOrEditShareOrdersDto> IAsyncCrudAppService<CreateOrEditShareOrdersDto, Guid, PagedShareOrdersResultRequestDto, CreateOrEditShareOrdersDto, CreateOrEditShareOrdersDto, EntityDto<Guid>, EntityDto<Guid>>.GetAsync(EntityDto<Guid> input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit share orders entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <returns></returns>
        public async Task<CreateOrEditShareOrdersDto> GetShareMasterForEdit(EntityDto<Guid> input)
        {
            var output = await _investmentShareOrders.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditShareOrdersDto>(output);
            return ClassObj;
        }

        /// <summary>
        /// Overridden ApplySorting
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<EL_Financial_Investment_Share_Orders> ApplySorting(IQueryable<EL_Financial_Investment_Share_Orders> query, PagedShareOrdersResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty() || input.Sorting == "0 asc" || input.Sorting.Contains("Share_Order_Date"))
            {
                input.Sorting = input.Sorting.Contains("desc") ? "Share_Order_Date desc" : "Share_Order_Date asc";
            }
            return base.ApplySorting(query, input);
        }

        /// <summary>
        /// Deletes the share orders entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            var shareOrdersDetails = await _investmentShareOrders.GetAsync(input.Id);
            await _investmentShareOrders.HardDeleteAsync(shareOrdersDetails);
        }
        public OrderCalculationDetails GetAverageSharePrice(Guid shareMasterId)
        {
            return CalculateAverageSharePrice(shareMasterId);
        }
        /// <summary>
        /// Returns the final average price of buy/bonus shares etc...
        /// Update the same function in sharemaster service if any changes made to this function.
        /// </summary>
        /// <param name="shareMasterId"></param>
        /// <returns></returns>
        private OrderCalculationDetails CalculateAverageSharePrice(Guid shareMasterId)
        {
            OrderCalculationDetails orderCalculationDetails = new OrderCalculationDetails();
            try
            {
                var investmentShareMasterOrders = _investmentShareOrders.GetAllList(o => o.EL_Financial_Investment_Share_Master_Id == shareMasterId);

                double totalOrdersAmount = 0;
                double totalShareOrdersAmount = 0;
                double totalShareSellAmount = 0;
                int totalOrdersPurchasedShared = 0;
                foreach (var item in investmentShareMasterOrders)
                {
                    if (item.Share_Transaction_Type != EasyLifeEnums.Share_Transaction_Type.Sell)
                    {
                        totalOrdersAmount += item.Share_Amount;
                        totalShareOrdersAmount += item.Share_Amount;
                        totalOrdersPurchasedShared += item.Share_Quantity;
                    }
                    else if (item.Share_Transaction_Type == EasyLifeEnums.Share_Transaction_Type.Sell)
                    {
                        totalOrdersAmount -= item.Share_Amount;
                        totalShareSellAmount += item.Share_Amount;
                        totalOrdersPurchasedShared -= item.Share_Quantity;
                    }
                }

                orderCalculationDetails.TotalAveragePriceDisplay = totalOrdersPurchasedShared != 0 ? (totalOrdersAmount / totalOrdersPurchasedShared).ToString("0.00").ToLocalMoneyFormat() : "N/A";

                orderCalculationDetails.TotalShareDisplay = totalOrdersPurchasedShared.ToString();
                orderCalculationDetails.TotalInvestedDisplay = totalShareOrdersAmount.ToLocalMoneyFormat();
                orderCalculationDetails.TotalEarnedOrLossDisplay = (totalShareSellAmount - totalShareOrdersAmount).ToString("0.00").ToLocalMoneyFormat() + " (" + (((totalShareSellAmount - totalShareOrdersAmount) / totalShareOrdersAmount * 100).ToString("0.00") + "%)");
                orderCalculationDetails.TotalEarnedOrLossPercentage = (totalShareSellAmount - totalShareOrdersAmount) / totalShareOrdersAmount * 100;
                orderCalculationDetails.TotalEarnedOrLoss = (totalShareSellAmount - totalShareOrdersAmount);
                return orderCalculationDetails;
            }
            catch (Exception ex)
            {
                return orderCalculationDetails;
            }
        }
    }
}
