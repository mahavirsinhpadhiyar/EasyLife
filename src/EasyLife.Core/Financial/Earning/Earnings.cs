using Abp.Domain.Entities.Auditing;
using EasyLife.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLife.Financial.Earning
{
    [Table("Earnings")]
    public class Earnings : FullAuditedEntity<Guid>
    {
        //Short description of the payee or note.
        public string Payee { get; set; }
        public string Note { get; set; }
        public DateTime EarningDate { get; set; }
        public bool ConsiderInTotal { get; set; }
        public double Money { get; set; }
        [ForeignKey("EarningCategory")]
        public Guid? EarningCategoryId { get; set; }
        public EarningCategory EarningCategory { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        //Expense entry considerintotal having false will not count anywhere in whole application.
        public bool DoNotConsiderInTotal { get; set; }
    }
}
