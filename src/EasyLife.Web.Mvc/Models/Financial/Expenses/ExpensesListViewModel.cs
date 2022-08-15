using EasyLife.Financial.Expenses.Dto;
using EasyLife.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EasyLife.Web.Models.Financial.Expenses
{
    public class ExpensesListViewModel: CommonVM
    {
        public CreateOrEditExpensesDto Expenses { get; set; }
        public List<SelectListItem> ExpenseCategoryList { get; set; }
        public List<SelectListItem> UsersList { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string CurrentMonthlyExpenses { get; set; }
    }
}
