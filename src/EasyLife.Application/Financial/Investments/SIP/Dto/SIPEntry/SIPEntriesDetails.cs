using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.SIP.Dto.SIPEntry
{
    public class SIPEntriesDetails
    {
        public string TotalAveragePrice { get; set; }
        public double TotalInvested { get; set; }
        public double TotalEarnedOrLoss { get; set; }
        public double TotalEarnedOrLossPercentage { get; set; }
        public int TotalShare { get; set; }

        //for display
        public string TotalAveragePriceDisplay { get; set; }
        public string TotalInvestedDisplay { get; set; }
        public string TotalEarnedOrLossDisplay { get; set; }
        public string TotalEarnedOrLossPercentageDisplay { get; set; }
        public string TotalShareDisplay { get; set; }
    }
}
