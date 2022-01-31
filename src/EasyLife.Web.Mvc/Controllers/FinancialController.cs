using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using EasyLife.Authorization;
using EasyLife.Controllers;
using EasyLife.Financial.Expenses;
using EasyLife.Web.Models.Financial.Expenses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EasyLife.Web.Controllers
{
    public class FinancialController : EasyLifeControllerBase
    {
        private readonly IExpensesAppService _expensesAppService;
        public FinancialController(IExpensesAppService expensesAppService)
        {
            _expensesAppService = expensesAppService;
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Expenses)]
        #region Expenses
        public IActionResult Expenses()
        {
            var expenseCategoryList = _expensesAppService.GetExpenseCategoriesList();
            return View("Expenses",new ExpensesListViewModel() { 
                Expenses = new Financial.Expenses.Dto.CreateOrEditExpensesDto(),
                ExpenseCategoryList = expenseCategoryList
            });
        }

        public async Task<ActionResult> ExpensesEditModal(Guid expenseId)
        {
            var expenseDetail = await _expensesAppService.GetExpensesForEdit(new EntityDto<Guid>(expenseId));
            var expenseCategoryList = _expensesAppService.GetExpenseCategoriesList();
            var model = new EditExpensesViewModel
            {
                Expenses = expenseDetail,
                ExpenseCategoryList = expenseCategoryList
            };
            return PartialView("_ExpensesEditModal", model);
        }

        #endregion Expenses
    }
}
