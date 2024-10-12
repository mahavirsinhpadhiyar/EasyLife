using Abp.Application.Services.Dto;
using System;

namespace EasyLife.Financial.Investments.SIP.Dto.SIPEntry
{
    public class PagedSIPEntriesResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string SIPMasterId { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
    }
}
