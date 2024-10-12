using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLife.Financial.Expenses
{
    [Table("ExpenseCategory")]
    public class ExpenseCategory : FullAuditedEntity<Guid>
    {
        public string CategoryName { get; set; }
        [ForeignKey("FKExpenseCategory")]
        public Guid? ParentId { get; set; }
        public ExpenseCategory FKExpenseCategory { get; set; }
        public bool IsActive { get; set; }
        public bool IsForMeActive { get; set; }
        public ICollection<Expenses> EL_Financial_Expenses_List { get; set; }
    }
}
