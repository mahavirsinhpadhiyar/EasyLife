using Abp.Application.Services.Dto;
using System;

namespace EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders
{
    public class PagedShareOrdersResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string ShareMasterId { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
    }
}
