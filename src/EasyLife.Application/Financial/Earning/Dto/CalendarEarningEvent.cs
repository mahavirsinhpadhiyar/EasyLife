using Abp.Application.Services.Dto;
using System;

namespace EasyLife.Financial.Earning.Dto
{
    public class CalendarEarningEvent : EntityDto<Guid>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
