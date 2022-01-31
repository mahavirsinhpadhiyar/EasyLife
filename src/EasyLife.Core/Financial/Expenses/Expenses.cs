using Abp.Domain.Entities.Auditing;
using EasyLife.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLife.Financial.Expenses
{
    [Table("Expenses")]
    public class Expenses: FullAuditedEntity<Guid>
    {
        //Short description of the payee or note.
        public string Payee { get; set; }
        public string Note { get; set; }
        public DateTime ExpenseDate { get; set; }
        public bool ConsiderInTotal { get; set; }
        public double Money { get; set; }
        [ForeignKey("ExpenseCategory")]
        public Guid? ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
