using Abp.Application.Services.Dto;
using System;

namespace EasyLife.Financial.Earnings.Dto
{
    public class PagedEarningsResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public long? FilterUserId { get; set; }
    }
}
