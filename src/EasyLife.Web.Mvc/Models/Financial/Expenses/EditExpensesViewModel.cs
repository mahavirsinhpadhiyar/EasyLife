using EasyLife.Financial.Expenses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EasyLife.Web.Models.Financial.Expenses
{
    public class EditExpensesViewModel
    {
        public CreateOrEditExpensesDto Expenses { get; set; }
        public List<SelectListItem> ExpenseCategoryList { get; set; }
    }
}
