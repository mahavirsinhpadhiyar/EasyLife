using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Expenses.Dto
{
    public class CBarChartExpenses
    {
        public CBarChartExpenses()
        {
            title = new title();
            tooltip = new tooltip() { formatter = "{a} <br/>{b} : {c} ({d}%)" };
            legend = new legend() { };
            series = new List<series>();
        }
        public title title { get; set; }
        public tooltip tooltip { get; set; }
        public legend legend { get; set; }
        public List<series> series { get; set; }
    }
    public class title
    {
        public string text { get; set; }
        public string subtext { get; set; }
        public string left { get; set; }
    }
}
