using System.Collections.Generic;

namespace EasyLife.Financial.Earning.Dto
{
    public class BarChartEarnings
    {
        public BarChartEarnings()
        {
            title = new title();
            tooltip = new tooltip() { formatter = "{a} <br/>{b} : {c} ({d}%)" };
            legend = new legend() {  };
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

    public class tooltip
    {
        public string trigger { get; set; }
        public string formatter { get; set; }
    }

    public class legend
    {
        public string type { get; set; }
        public string orient { get; set; }
        public string right { get; set; }
        public string left { get; set; }
        public string top { get; set; }
        public string bottom { get; set; }
    }
    public class series
    {
        public series()
        {
            data = new List<data>();
            emphasis = new emphasis();
        }
        public string name { get; set; }
        public string type { get; set; }
        public string radius { get; set; }
        public List<data> data { get; set; }
        public emphasis emphasis { get; set; }
    }
    public class data
    {
        public string value { get; set; }
        public string name { get; set; }
    }
    public class emphasis
    {
        public emphasis()
        {
            itemStyle = new itemStyle();
        }
        public itemStyle itemStyle { get; set; }
    }
    public class itemStyle
    {
        public int shadowBlur { get; set; }
        public int shadowOffsetX { get; set; }
        public string shadowColor { get; set; }
    }
}