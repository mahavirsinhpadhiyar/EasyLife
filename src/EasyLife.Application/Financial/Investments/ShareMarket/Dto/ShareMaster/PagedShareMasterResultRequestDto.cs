using Abp.Application.Services.Dto;
using System;

namespace EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto
{
    public class PagedShareMasterResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? FilterUserId { get; set; }
    }
}
