using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace EasyLife.Financial.Earnings.Dto
{
    /// <summary>
    /// Earning Dto class
    /// </summary>
    [AutoMapFrom(typeof(Financial.Earning.Earnings))]
    public class CreateOrEditEarningsDto : EntityDto<Guid>
    {
        public string Payee { get; set; }
        public string Note { get; set; }
        public DateTime EarningDate { get; set; }
        public bool ConsiderInTotal { get; set; }
        public double Money { get; set; }
        public Guid? EarningCategoryId { get; set; }
        public long UserId { get; set; }
        public string EarningCategoryName { get; set; }
        public string EarningDateDisplay
        {
            get
            {
                return EarningDate.ToString("dd/MM/yyyy");
            }
        }
    }
}
