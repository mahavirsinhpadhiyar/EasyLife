using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using EasyLife.Financial.Investment;
using System;

namespace EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto
{
    [AutoMapFrom(typeof(EL_Financial_Investment_Share_Master))]
    public class CreateOrEditShareMasterDto : EntityDto<Guid>
    {
        public string Share_Name { get; set; }
        public string TotalAveragePrice { get; set; }
        public double TotalInvested { get; set; }
        public double TotalEarnedOrLoss { get; set; }
        public int TotalShare { get; set; }
        public long UserId { get; set; }

        //for display
        public string TotalAveragePriceDisplay { get; set; }
        public string TotalInvestedDisplay { get; set; }
        public string TotalEarnedOrLossDisplay { get; set; }
        public string TotalEarnedOrLossPercentageDisplay { get; set; }
        public string TotalShareDisplay { get; set; }
    }
}
