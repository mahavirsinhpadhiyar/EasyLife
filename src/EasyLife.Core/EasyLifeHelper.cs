using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife
{
    public static class EasyLifeHelper
    {
        public static CultureInfo EnglishIndia = new CultureInfo("en-IN");

        public static String ToLocalMoneyFormat(this object value)
        {
            return string.Format(EnglishIndia, "₹ {0:#,0.00}", value);
        }
    }
}
