using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace EasyLife.Financial.Expenses.Dto
{
    public class CalendarEvents : EntityDto<Guid>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
