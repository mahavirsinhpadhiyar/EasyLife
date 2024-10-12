using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife
{
    public static class Extensions
    {
        public static string ToString_dd_MM_yyyy_HH_MM(this DateTime dt)
        {
            return dt.ToString("dd MMMM yyyy HH:mm");
        }
    }
}
