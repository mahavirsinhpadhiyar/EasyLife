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
    [Table("EL_Financial_Investment_SIP_Master")]
    public class EL_Financial_Investment_SIP_Master : FullAuditedEntity<Guid>
    {
        public string SIP_Name { get; set; }
        public string SIP_Folio_No { get; set; }
        //This will be the date on which installment of SIP get deducated.
        public DateTime AutoInstallmentDate { get; set; }
        public string AutopayID { get; set; }
        public string SIPID { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        public ICollection<EL_Financial_Investment_SIP_Entry> EL_Financial_Investment_SIP_Entries { get; set; }
    }
}
