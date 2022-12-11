using Abp.Domain.Entities.Auditing;
using EasyLife.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLife.Financial.Expenses
{
    [Table("Expenses")]
    public class Expenses
        : FullAuditedEntity<Guid>
    {
        //name of the person or place or shop to whom you are giving/spending money.
        public string Payee { get; set; }
        public string Note { get; set; }
        public DateTime ExpenseDate { get; set; }
        //Expense entry considerintotal having false will not count anywhere in whole application.
        public bool DoNotConsiderInTotal { get; set; }
        public double Money { get; set; }
        [ForeignKey("ExpenseCategory")]
        public Guid? ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
