using System;
using System.Collections.Generic;
using EasyLife.Financial.Expenses.Dto.Reports;

namespace EasyLife.Financial.Expenses.Dto.Reports
{
    public class CBarChartExpenses
    {
        public CBarChartExpenses()
        {
            title = new title();
            tooltip = new tooltip() { formatter = "{a} <br/>{b} : {c} ({d}%)", axisPointer = new axisPointer() { type = "shadow" } };
            legend = new legend() { };
            series = new List<CBartChartSeries>();
        }
        public title title { get; set; }
        public tooltip tooltip { get; set; }
        public legend legend { get; set; }
        public List<CBartChartSeries> series { get; set; }
        public grid grid { get; set; }
        public xAxis xAxis { get; set; }
        public yAxis yAxis { get; set; }
    }
    public class yAxis
    {
        public string type { get; set; }
    }
    public class xAxis
    {
        public string type { get; set; }
        public List<string> data { get; set; }
    }
    public class grid
    {
        public string left { get; set; }
        public string right { get; set; }
        public string bottom { get; set; }
        public bool containLabel { get; set; }
    }
    public class axisPointer
    {
        public string type { get; set; }
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
        public axisPointer axisPointer { get; set; }
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
    public class CBartChartSeries
    {
        public CBartChartSeries()
        {
            data = new List<String>();
            emphasis = new emphasis();
        }
        public string name { get; set; }
        public string type { get; set; }
        public string radius { get; set; }
        public List<String> data { get; set; }
        public emphasis emphasis { get; set; }
        public string stack { get; set; }
    }
    public class emphasis
    {
        public emphasis()
        {
            focus = "series";
        }
        public string focus { get; set; }
    }
    public class itemStyle
    {
        public int shadowBlur { get; set; }
        public int shadowOffsetX { get; set; }
        public string shadowColor { get; set; }
    }
}
