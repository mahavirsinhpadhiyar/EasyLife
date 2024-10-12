using EasyLife.Financial.Investment;
using EasyLife.Financial.Investments.SIP.Dto.SIPEntry;
using EasyLife.Financial.Investments.SIP.Dto.SIPMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EasyLife.EasyLifeEnums;

namespace EasyLife.Financial
{
    public static class FinancialFuncs
    {
        public static double CalculateSIPAveragePrice(double totalInvestment, double totalUnits)
        {
            if (totalUnits == 0)
            {
                throw new ArgumentException("Total units cannot be zero.");
            }

            double averagePrice = totalInvestment / totalUnits;
            return averagePrice;
        }

        public static void CalculateTotalCostAndTotalUnits(CreateOrEditSIPMasterDto SIPMaster, EL_Financial_Investment_SIP_Entry SIPEntry)
        {
            if (SIPEntry.SIPType == SIPType.Instalment || SIPEntry.SIPType == SIPType.OneTime)
            {
                double unitsBought = SIPEntry.SIP_Amount / SIPEntry.SIP_Average_Price;
                SIPMaster.TotalInvestedAmount += SIPEntry.SIP_Amount;
                SIPMaster.TotalPurchasedUnits += unitsBought;
            }
            else if (SIPEntry.SIPType == SIPType.Redeem)
            {
                double unitsRedeemed = SIPEntry.SIP_Units;
                // Assuming FIFO for cost basis calculation
                double costBasisPerUnit = SIPMaster.TotalInvestedAmount / SIPMaster.TotalPurchasedUnits;
                SIPMaster.TotalInvestedAmount -= unitsRedeemed * costBasisPerUnit;
                SIPMaster.TotalPurchasedUnits -= unitsRedeemed;
            }
        }

        public static double CalculatePercentageOfProfit(double initialNav, double currentNav)
        {
            if (initialNav == 0)
            {
                throw new ArgumentException("Initial NAV cannot be zero.");
            }

            double profit = currentNav - initialNav;
            double percentageOfProfit = (profit / initialNav) * 100;

            return percentageOfProfit;
        }

        public static bool TryConvertStringToDouble(string input, out double result)
        {
            return double.TryParse(input, out result);
        }
    }
}
