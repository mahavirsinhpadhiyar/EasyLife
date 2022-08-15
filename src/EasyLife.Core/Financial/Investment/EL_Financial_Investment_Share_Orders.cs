using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static EasyLife.EasyLifeEnums;

namespace EasyLife.Financial.Investment
{
    [Table("EL_Financial_Investment_Share_Orders")]
    public class EL_Financial_Investment_Share_Orders: FullAuditedEntity<Guid>
    {
        public DateTime Share_Order_Date { get; set; }
        public Double Share_Average_Price { get; set; }
        public double Share_Amount { get; set; }
        public Share_Order_Type Share_Order_Type { get; set; }
        public Share_Qty_Exchange_Type Share_Qty_Exchange_Type { get; set; }
        public Share_Price_Type Share_Price_Type { get; set; }
        public int Share_Quantity { get; set; }
        public string Share_Order_Id { get; set; }
        public Share_Transaction_Type Share_Transaction_Type { get; set; }
        public string Share_App_Order_Id { get; set; }
        #region ForeignKeys
        [ForeignKey("EL_Financial_Investment_Share_Master")]
        public Guid EL_Financial_Investment_Share_Master_Id { get; set; }
        public EL_Financial_Investment_Share_Master EL_Financial_Investment_Share_Master { get; set; }
        #endregion ForeignKeys
    }
}
