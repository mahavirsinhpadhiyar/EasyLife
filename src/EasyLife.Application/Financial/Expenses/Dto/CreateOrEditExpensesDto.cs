using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace EasyLife.Financial.Expenses.Dto
{
    [AutoMapFrom(typeof(Expenses))]
    public class CreateOrEditExpensesDto : EntityDto<Guid>
    {
        public string Payee { get; set; }
        public string Note { get; set; }
        public DateTime ExpenseDate { get; set; }
        public bool DoNotConsiderInTotal { get; set; }
        public double Money { get; set; }
        public Guid? ExpenseCategoryId { get; set; }
        public long UserId { get; set; }
        public string ExpenseCategoryName { get; set; }
        public string ExpenseDateDisplay
        {
            get
            {
                return ExpenseDate.ToString_dd_MM_yyyy_HH_MM();
            }
        }
    }
}
