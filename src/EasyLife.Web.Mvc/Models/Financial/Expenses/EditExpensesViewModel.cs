using EasyLife.Financial.Expenses.Dto;
using EasyLife.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EasyLife.Web.Models.Financial.Expenses
{
    public class EditExpensesViewModel : CommonVM
    {
        public CreateOrEditExpensesDto Expenses { get; set; }
        public List<SelectListItem> ExpenseCategoryList { get; set; }
    }
}
