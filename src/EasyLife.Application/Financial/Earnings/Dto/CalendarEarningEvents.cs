using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace EasyLife.Financial.Earnings.Dto
{
    /// <summary>
    /// Will use to show earnings in calendar
    /// </summary>
    public class CalendarEarningEvents : EntityDto<Guid>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
