using Abp.Domain.Entities.Auditing;
using EasyLife.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EasyLife.EasyLifeEnums;

namespace EasyLife.Financial.Investment
{
    [Table("EL_Financial_Investment_SIP_Entry")]
    public class EL_Financial_Investment_SIP_Entry : FullAuditedEntity<Guid>
    {
        public DateTime SIP_Order_Date { get; set; }
        public Double SIP_Average_Price { get; set; }
        public double SIP_Amount { get; set; }
        public double SIP_Units { get; set; }
        public string SIP_Order_Id { get; set; }
        public SIPType SIPType { get; set; }
        #region ForeignKeys
        [ForeignKey("EL_Financial_Investment_SIP_Master")]
        public Guid EL_Financial_Investment_SIP_Master_Id { get; set; }
        public EL_Financial_Investment_SIP_Master EL_Financial_Investment_SIP_Master { get; set; }
        #endregion ForeignKeys
    }
}
