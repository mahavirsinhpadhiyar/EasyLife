using EasyLife.Financial.Investments.SIP.Dto.SIPEntry;
using System;

namespace EasyLife.Web.Models.Financial.Investment
{
    public class SIPEntriesListViewModel
    {
        public CreateOrEditSIPEntriesDto SIPEntriesDto { get; set; }
        public Guid SIPMasterId { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string SIPMasterName { get; set; }
    }
}
