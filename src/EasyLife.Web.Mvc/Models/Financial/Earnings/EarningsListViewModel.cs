using EasyLife.Financial.Earning.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EasyLife.Web.Models.Financial.Earnings
{
    public class EarningsListViewModel
    {
        public CreateOrEditEarningDto Earnings { get; set; }
        public List<SelectListItem> EarningCategoryList { get; set; }
        public List<SelectListItem> UsersList { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string CurrentMonthlyEarnings { get; set; }
    }
}
