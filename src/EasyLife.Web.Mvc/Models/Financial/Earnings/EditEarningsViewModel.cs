using EasyLife.Financial.Earning.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EasyLife.Web.Models.Financial.Earnings
{
    public class EditEarningsViewModel
    {
        public CreateOrEditEarningDto Earnings { get; set; }
        public List<SelectListItem> EarningCategoryList { get; set; }
    }
}
