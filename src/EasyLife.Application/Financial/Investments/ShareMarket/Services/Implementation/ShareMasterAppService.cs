using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using EasyLife.Authorization;
using EasyLife.EntityFrameworkCore;
using EasyLife.Financial.Investment;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareMaster;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using EasyLife.Financial.Investments.ShareMarket.Services.Interfaces;
using EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.ShareMarket.Services.Implementation
{
    [AbpAuthorize(PermissionNames.Pages_Financial_Investment_ShareMarket)]
    public class ShareMasterAppService : AsyncCrudAppService<EL_Financial_Investment_Share_Master, CreateOrEditShareMasterDto, Guid, PagedShareMasterResultRequestDto, CreateOrEditShareMasterDto, CreateOrEditShareMasterDto>, IShareMasterAppService
    {
        private readonly IRepository<EL_Financial_Investment_Share_Master, Guid> _investmentShareMaster;
        private readonly IRepository<EL_Financial_Investment_Share_Orders, Guid> _investmentShareOrders;
        //private readonly EasyLifeDbContext _dbContextProvider;
        public ShareMasterAppService(IRepository<EL_Financial_Investment_Share_Master, Guid> investmentShareMaster,
            IRepository<EL_Financial_Investment_Share_Orders, Guid> investmentShareOrders
            //,IDbContextProvider<EasyLifeDbContext> dbContextProvider
            ) : base(investmentShareMaster)
        {
            _investmentShareMaster = investmentShareMaster;
            _investmentShareOrders = investmentShareOrders;
            //_dbContextProvider = dbContextProvider.GetDbContext();
        }

        /// <summary>
        /// Will return the list of paging wise investment master entries with filtered result
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<CreateOrEditShareMasterDto>> GetAllFiltered(PagedShareMasterResultRequestDto input)
        {
            var shareMasterList = new List<EL_Financial_Investment_Share_Master>();
            if (input.FilterUserId == null || input.FilterUserId == 0)
                input.FilterUserId = AbpSession.UserId;
            var query = _investmentShareMaster.GetAll().Where(x => x.UserId == input.FilterUserId);

            query = ApplySorting(query, input);

            shareMasterList = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Share_Name.Contains(input.Keyword))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(e => e.Share_Name)
                .ToList();

            var pageCount = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Share_Name.Contains(input.Keyword))
                .OrderBy(e => e.Share_Name)
                .Count();

            var shareMasterFilterList = shareMasterList.Select(r => new CreateOrEditShareMasterDto()
            {
                Share_Name = r.Share_Name,
                Id = r.Id
            }).OrderBy(r => r.Share_Name).ToList();

            foreach (var shareMaster in shareMasterFilterList)
            {
                var shareMasterAverageDetails = CalculateAverageSharePrice(shareMaster.Id);
                shareMaster.TotalAveragePriceDisplay = shareMasterAverageDetails.TotalAveragePriceDisplay;
                shareMaster.TotalShareDisplay = shareMasterAverageDetails.TotalShareDisplay;
                shareMaster.TotalEarnedOrLossDisplay = shareMasterAverageDetails.TotalEarnedOrLossDisplay;
                shareMaster.TotalInvestedDisplay = shareMasterAverageDetails.TotalInvestedDisplay;
                shareMaster.TotalEarnedOrLoss = shareMasterAverageDetails.TotalEarnedOrLoss;
            }

            var result = new PagedResultDto<CreateOrEditShareMasterDto>(pageCount, shareMasterFilterList);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Create share master entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditShareMasterDto> CreateAsync(CreateOrEditShareMasterDto input)
        {
            try
            {
                //DateTime dt = DateTime.Now;
                //input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
                input.UserId = AbpSession.UserId.Value;
                var shareMasterDetail = ObjectMapper.Map<EL_Financial_Investment_Share_Master>(input);
                await _investmentShareMaster.InsertAsync(shareMasterDetail);
                return MapToEntityDto(shareMasterDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update share master
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditShareMasterDto> UpdateAsync(CreateOrEditShareMasterDto input)
        {
            try
            {
                //For duplicate entry
                if (input.Id == Guid.Empty)
                {
                    input.UserId = AbpSession.UserId.Value;
                    var shareMasterDetail = ObjectMapper.Map<EL_Financial_Investment_Share_Master>(input);
                    shareMasterDetail.Id = await _investmentShareMaster.InsertAndGetIdAsync(shareMasterDetail);
                    return MapToEntityDto(shareMasterDetail);
                }
                else
                {
                    //input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
                    input.UserId = AbpSession.UserId.Value;
                    var shareMasterDetail = await _investmentShareMaster.GetAsync(input.Id);
                    ObjectMapper.Map(input, shareMasterDetail);
                    await _investmentShareMaster.UpdateAsync(shareMasterDetail);
                    return MapToEntityDto(shareMasterDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit share master entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <returns></returns>
        public async Task<CreateOrEditShareMasterDto> GetShareMasterForEdit(EntityDto<Guid> input)
        {
            var output = await _investmentShareMaster.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditShareMasterDto>(output);
            return ClassObj;
        }

        /// <summary>
        /// Overridden ApplySorting
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<EL_Financial_Investment_Share_Master> ApplySorting(IQueryable<EL_Financial_Investment_Share_Master> query, PagedShareMasterResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty() || input.Sorting == "0 asc" || input.Sorting.Contains("Share_Name"))
            {
                input.Sorting = input.Sorting.Contains("desc") ? "Share_Name desc" : "Share_Name asc";
            }
            return base.ApplySorting(query, input);
        }

        /// <summary>
        /// Deletes the share master entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            var shareMasterDetails = await _investmentShareMaster.GetAsync(input.Id);
            await _investmentShareMaster.HardDeleteAsync(shareMasterDetails);
        }

        public BarChartShareMasterWithOrders GetShareMasterWithOrdersBarDetails(long? FilterUserId)
        {
            if (FilterUserId == null || FilterUserId == 0)
                FilterUserId = AbpSession.UserId;
            var investmentShareMaster = _investmentShareMaster.GetAll().Include(i => i.EL_Financial_Investment_Share_Orders).Where(i => i.UserId == FilterUserId).OrderBy(s => s.Share_Name);

            var data = new List<data>();

            foreach (var item in investmentShareMaster)
            {
                var totalBuy = item.EL_Financial_Investment_Share_Orders.Where(o => o.Share_Transaction_Type != EasyLifeEnums.Share_Transaction_Type.Sell).Sum(o => o.Share_Quantity);
                var totalSell = item.EL_Financial_Investment_Share_Orders.Where(o => o.Share_Transaction_Type == EasyLifeEnums.Share_Transaction_Type.Sell).Sum(o => o.Share_Quantity);
                if (totalBuy != totalSell)
                {
                    data.Add(new Dto.ShareMaster.data() { name = item.Share_Name, value = (totalBuy - totalSell).ToString() });
                }
            }

            BarChartShareMasterWithOrders barChartShareMasterWithOrders = new BarChartShareMasterWithOrders();
            
            //Hide as share names overrite the main heading
            //barChartShareMasterWithOrders.title.text = "Shares Count";        
            //barChartShareMasterWithOrders.title.left = "center";

            barChartShareMasterWithOrders.tooltip.trigger = "item";
            barChartShareMasterWithOrders.legend.orient = "horizontal";
            barChartShareMasterWithOrders.legend.left = "center";
            barChartShareMasterWithOrders.series.Add(
                new series()
                {
                    name = "Share Market Total counts",
                    type = "pie",
                    radius = "50%",
                    data = data,
                    emphasis = new emphasis() { itemStyle = new itemStyle() { shadowBlur = 10, shadowOffsetX = 0, shadowColor = "rgba(0, 0, 0, 0.5)" } }
                }
                );

            return barChartShareMasterWithOrders;
        }

        /// <summary>
        /// Returns the final average price of buy/bonus shares etc...
        /// Update this function whenever same function in shareorder service updates.
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

        /// <summary>
        /// Returns the final average price of buy/bonus shares etc...
        /// Update this function whenever same function in shareorder service updates.
        /// </summary>
        /// <param name="shareMasterId"></param>
        /// <returns></returns>
        public async Task<string> CalculateTotalInvested(long? FilterUserId)
        {
            double totalInvested = 0;
            try
            {
                if (FilterUserId == null || FilterUserId == 0)
                    FilterUserId = AbpSession.UserId;

                var shareOrders = await _investmentShareMaster.GetAll().Include(s => s.EL_Financial_Investment_Share_Orders).Where(s => s.UserId == FilterUserId && s.EL_Financial_Investment_Share_Orders.Where(o => o.Share_Transaction_Type == EasyLifeEnums.Share_Transaction_Type.Buy).Count() > 0).ToListAsync();

                foreach (var shareOrder in shareOrders)
                {
                    totalInvested += shareOrder.EL_Financial_Investment_Share_Orders.Where(o => o.Share_Transaction_Type == EasyLifeEnums.Share_Transaction_Type.Buy).Sum(o => o.Share_Amount);
                }

                return totalInvested.ToLocalMoneyFormat();

                //var tempShareMaster = from shareMaster in _dbContextProvider.EL_Financial_Investment_Share_Master
                //join shareOrders in _dbContextProvider.EL_Financial_Investment_Share_Orders
                //on shareMaster.Id equals shareOrders.EL_Financial_Investment_Share_Master_Id
                //where shareOrders.Share_Transaction_Type == EasyLifeEnums.Share_Transaction_Type.Buy
                //select new
                //{
                //    shareAmount = shareOrders.Share_Amount
                //};

                //return tempShareMaster.Sum(o => o.shareAmount);

                //return _investmentShareMaster.GetAll()
                //    .Where(s => s.EL_Financial_Investment_Share_Orders.All(o => o.Share_Transaction_Type == EasyLifeEnums.Share_Transaction_Type.Buy))
                //    .Include(s => s.EL_Financial_Investment_Share_Orders)
                //    .Sum(s => s.EL_Financial_Investment_Share_Orders.Sum(o => o.Share_Amount));

                //return _investmentShareMaster.GetAllIncluding(s => s.EL_Financial_Investment_Share_Orders).Where(s => s.UserId == FilterUserId).Sum(r => r.EL_Financial_Investment_Share_Orders.Sum(o => o.Share_Amount));
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
    }
}