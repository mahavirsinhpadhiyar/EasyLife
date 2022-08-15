using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyLife.Financial.Earning
{
    [Table("EarningCategory")]
    public class EarningCategory : FullAuditedEntity<Guid>
    {
        public string CategoryName { get; set; }
        [ForeignKey("FKEarningCategory")]
        public Guid? ParentId { get; set; }
        public EarningCategory FKEarningCategory { get; set; }
        public bool IsActive { get; set; }
        public bool IsForMeActive { get; set; }
    }
}
