using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using EasyLife.Financial.Investment;
using System;


namespace EasyLife.Financial.Investments.SIP.Dto.SIPMaster
{
    [AutoMapFrom(typeof(EL_Financial_Investment_SIP_Master))]
    public class CreateOrEditSIPMasterDto : EntityDto<Guid>
    {
        #region FieldsMappedWithDB
        public string SIP_Name { get; set; }
        public string SIP_Folio_No { get; set; }
        public long UserId { get; set; }
        public DateTime AutoInstallmentDate { get; set; }
        public string AutopayID { get; set; }
        public string SIPID { get; set; }
        #endregion FieldsMappedWithDB

        //To display the current average price of the taken mutual fund
        public string CurrentAveragePrice { get; set; }
        //To display the average price of the user's taken stock wise.
        public string AverageNAV { get; set; }
        public string TotalEarnedOrLossDisplay { get; set; }
        public string TotalEarnedOrLossPercentageDisplay { get; set; }
        public string TotalPurchasedUnits { get; set; }
        public string TotalInvestedAmount { get; set; }
    }
}
