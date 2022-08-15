using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using EasyLife.Financial.Investment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EasyLife.EasyLifeEnums;

namespace EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders
{
    [AutoMapFrom(typeof(EL_Financial_Investment_Share_Orders))]
    public class CreateOrEditShareOrdersDto: EntityDto<Guid>
    {
        public DateTime Share_Order_Date { get; set; }
        public string Share_Order_Date_Display { get { return Share_Order_Date.ToString("dd/MM/yyyy HH:mm"); } }
        public Double Share_Average_Price { get; set; }
        public double Share_Amount { get; set; }
        public Share_Order_Type Share_Order_Type { get; set; }
        public string Share_Order_Type_Display
        {
            get { return Share_Order_Type.ToString(); }
        }
        public Share_Qty_Exchange_Type Share_Qty_Exchange_Type { get; set; }
        public string Share_Qty_Exchange_Type_Display
        {
            get { return Share_Qty_Exchange_Type.ToString(); }
        }
        public Share_Price_Type Share_Price_Type { get; set; }
        public string Share_Price_Type_Display
        {
            get { return Share_Price_Type.ToString(); }
        }
        public Share_Transaction_Type Share_Transaction_Type { get; set; }
        public string Share_Transaction_Type_Display
        {
            get { return Share_Transaction_Type.ToString(); }
        }
        public int Share_Quantity { get; set; }
        public string Share_Order_Id { get; set; }
        public string Share_App_Order_Id { get; set; }
        public Guid EL_Financial_Investment_Share_Master_Id { get; set; }
    }
}
