using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife
{
    public class EasyLifeEnums
    {
        #region Financial
        public enum Share_Order_Type
        {
            [Description("Delivery")]
            Delivery,
            [Description("Intraday")]
            Intraday
        }

        public enum Share_Qty_Exchange_Type
        {
            [Description("National Stock Exchange")]
            NSE,
            [Description("Bombay Stock Exchange")]
            BSE
        }

        public enum Share_Price_Type
        {
            [Description("Market")]
            Market,
            [Description("Limit")]
            Limit
        }
        public enum Share_Transaction_Type
        {
            [Description("Buy")]
            Buy,
            [Description("Sell")]
            Sell,
            [Description("Bonus")]
            Bonus
        }
        public enum EBarReportType
        {
            Monthly,
            Yearly
        }

        public enum SIPType
        {
            [Description("INSTALMENT")]
            Instalment,
            [Description("ONE-TIME")]
            OneTime,
            [Description("REDEEM")]
            Redeem
        }
        #endregion Financial
    }
}
