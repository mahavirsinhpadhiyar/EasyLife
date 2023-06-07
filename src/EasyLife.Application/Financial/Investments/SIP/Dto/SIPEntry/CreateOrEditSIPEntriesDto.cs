using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using EasyLife.Financial.Investment;
using System;
using static EasyLife.EasyLifeEnums;

namespace EasyLife.Financial.Investments.SIP.Dto.SIPEntry
{
    [AutoMapFrom(typeof(EL_Financial_Investment_SIP_Entry))]
    public class CreateOrEditSIPEntriesDto : EntityDto<Guid>
    {
        public DateTime SIP_Order_Date { get; set; }
        public Double SIP_Average_Price { get; set; }
        public double SIP_Amount { get; set; }
        public double SIP_Units { get; set; }
        public string SIP_Order_Id { get; set; }
        public SIPType SIPType { get; set; }
        public Guid EL_Financial_Investment_SIP_Master_Id { get; set; }

        public string SIP_Entry_Date_Display { get { return SIP_Order_Date.ToString("dd MMMM yyyy HH:mm"); } }
        public string SIPTypeDisplay { get { return SIPType.ToString(); } }
    }
}
