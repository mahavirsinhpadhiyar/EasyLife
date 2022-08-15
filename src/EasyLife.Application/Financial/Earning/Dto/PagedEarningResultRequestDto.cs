using Abp.Application.Services.Dto;
using System;

namespace EasyLife.Financial.Earning.Dto
{
    public class PagedEarningResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public long? FilterUserId { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
    }
}
