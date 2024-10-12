using Abp.Application.Services.Dto;

namespace EasyLife.Financial.Investments.SIP.Dto.SIPMaster
{
    public class PagedSIPMasterResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? FilterUserId { get; set; }
    }
}
