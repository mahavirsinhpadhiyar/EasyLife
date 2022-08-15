using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Expenses.Dto
{
    public class PagedExpensesResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public long? FilterUserId { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
    }
}
