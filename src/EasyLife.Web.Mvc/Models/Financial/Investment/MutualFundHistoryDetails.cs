using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Financial.Investment
{
    /// <summary>
    /// Contains Mutual fund history details
    /// </summary>
    public class MutualFundHistoryDetails
    {
        public Meta Meta { get; set; }
        public List<Data> Data { get; set; }
        public string Status { get; set; }
    }

    public class Meta
    {
        public string Fund_House { get; set; }
        public string Scheme_Type { get; set; }
        public string Scheme_Category { get; set; }
        public double Scheme_Code { get; set; }
        public string Scheme_Name { get; set; }
    }
    public class Data
    {
        public string Date { get; set; }
        /// <summary>
        /// Net Asset Value
        /// </summary>
        public double NAV { get; set; }
    }
}
