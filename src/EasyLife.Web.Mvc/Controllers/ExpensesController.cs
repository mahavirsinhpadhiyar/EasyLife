using Abp.AspNetCore.Mvc.Authorization;
using EasyLife.Authorization;
using EasyLife.Controllers;
using EasyLife.Financial.Expenses;
using Microsoft.AspNetCore.Mvc;

namespace EasyLife.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Financial_Expenses)]
    public class ExpensesController : EasyLifeControllerBase
    {
        private readonly IExpensesAppService _expensesAppService;
        public ExpensesController(IExpensesAppService expensesAppService)
        {
            _expensesAppService = expensesAppService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
