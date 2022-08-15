using Abp.Domain.Entities.Auditing;
using EasyLife.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investment
{
    [Table("EL_Financial_Investment_Share_Master")]
    public class EL_Financial_Investment_Share_Master : FullAuditedEntity<Guid>
    {
        public string Share_Name { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        public ICollection<EL_Financial_Investment_Share_Orders> EL_Financial_Investment_Share_Orders { get; set; }
    }
}
