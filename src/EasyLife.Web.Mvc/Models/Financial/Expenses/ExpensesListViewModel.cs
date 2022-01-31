using EasyLife.Financial.Expenses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Financial.Expenses
{
    public class ExpensesListViewModel
    {
        public CreateOrEditExpensesDto Expenses { get; set; }
        public List<SelectListItem> ExpenseCategoryList { get; set; }
    }
}
