using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using EasyLife.Authorization;
using EasyLife.Authorization.Roles;
using EasyLife.Controllers;
using EasyLife.Financial.Earning;
using EasyLife.Financial.Expenses;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using EasyLife.Financial.Investments.ShareMarket.Services.Interfaces;
using EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto;
using EasyLife.Financial.Investments.SIP.Dto.SIPEntry;
using EasyLife.Financial.Investments.SIP.Dto.SIPMaster;
using EasyLife.Financial.Investments.SIP.Services.Interfaces;
using EasyLife.Users;
using EasyLife.Web.Models.Financial.Earnings;
using EasyLife.Web.Models.Financial.Expenses;
using EasyLife.Web.Models.Financial.Investment;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EasyLife.Web.Controllers
{

    public class FinancialController : EasyLifeControllerBase
    {
        private readonly IExpensesAppService _expensesAppService;
        private readonly IEarningAppService _earningsAppService;
        private readonly ISIPMasterAppService _sipMasterAppService;
        private readonly ISIPEntriesAppService _sipEntriesAppService;
        private readonly IUserAppService _userAppService;
        private readonly IShareMasterAppService _shareMasterAppService;
        private readonly IShareOrdersAppService _shareOrdersAppService;
        public FinancialController(IExpensesAppService expensesAppService, IEarningAppService earningsAppService, IUserAppService userAppService, IShareMasterAppService shareMasterAppService, IShareOrdersAppService shareOrdersAppService, ISIPMasterAppService sipMasterAppService, ISIPEntriesAppService sipEntriesAppService)
        {
            _expensesAppService = expensesAppService;
            _earningsAppService = earningsAppService;
            _userAppService = userAppService;
            _shareMasterAppService = shareMasterAppService;
            _shareOrdersAppService = shareOrdersAppService;
            _sipMasterAppService = sipMasterAppService;
            _sipEntriesAppService = sipEntriesAppService;
        }

        #region Expenses
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Expenses)]
        public IActionResult Expenses()
        {
            var expenseCategoryList = Task.Run(async () => await _expensesAppService.GetExpenseCategoriesListAsync()).Result;
            if (User.IsInRole(EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin))
            {
                var usersList = Task.Run(async () => await _userAppService.GetUsersList()).Result;
                return View("Expenses", new ExpensesListViewModel()
                {
                    Expenses = new Financial.Expenses.Dto.CreateOrEditExpensesDto(),
                    ExpenseCategoryList = expenseCategoryList,
                    UsersList = usersList
                });
            }

            var currentMonthlyExpenses = Task.Run(async () => await _expensesAppService.DashboardTotalExpensesSum(null, null)).Result;

            return View("Expenses", new ExpensesListViewModel()
            {
                Expenses = new Financial.Expenses.Dto.CreateOrEditExpensesDto(),
                ExpenseCategoryList = expenseCategoryList,
                CurrentMonthlyExpenses = currentMonthlyExpenses,
                LoggedInUserIsAdmin = LoggedInUserRole()
            });
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Expenses)]
        public async Task<ActionResult> ExpensesEditModal(Guid expenseId)
        {
            var expenseDetail = await _expensesAppService.GetExpensesForEdit(new EntityDto<Guid>(expenseId));
            var expenseCategoryList = await _expensesAppService.GetExpenseCategoriesListAsync();
            var model = new EditExpensesViewModel
            {
                Expenses = expenseDetail,
                ExpenseCategoryList = expenseCategoryList,
                LoggedInUserIsAdmin = LoggedInUserRole()
            };
            return PartialView("_ExpensesEditModal", model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Expenses)]
        public async Task<ActionResult> ExpensesDuplicateModal(Guid expenseId)
        {
            var expenseDetail = await _expensesAppService.GetExpensesForEdit(new EntityDto<Guid>(expenseId));
            var expenseCategoryList = await _expensesAppService.GetExpenseCategoriesListAsync();
            var model = new EditExpensesViewModel
            {
                Expenses = expenseDetail,
                ExpenseCategoryList = expenseCategoryList
            };
            model.Expenses.Id = Guid.Empty;
            model.Expenses.ExpenseDate = DateTime.Now;
            return PartialView("_ExpensesEditModal", model);
        }

        #endregion Expenses

        #region Earnings
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Earnings)]
        public IActionResult Earnings()
        {
            var earningCategoryList = Task.Run(async () => await _earningsAppService.GetEarningCategoriesList()).Result;
            if (User.IsInRole(EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin))
            {
                var usersList = Task.Run(async () => await _userAppService.GetUsersList()).Result;
                return View("Earnings", new EarningsListViewModel()
                {
                    Earnings = new Financial.Earning.Dto.CreateOrEditEarningDto(),
                    EarningCategoryList = earningCategoryList,
                    UsersList = usersList
                });
            }

            var currentMonthlyEarnings = Task.Run(async () => await _earningsAppService.DashboardTotalEarnings(null, null)).Result;

            return View("Earnings", new EarningsListViewModel()
            {
                Earnings = new Financial.Earning.Dto.CreateOrEditEarningDto(),
                EarningCategoryList = earningCategoryList,
                CurrentMonthlyEarnings = currentMonthlyEarnings
            });
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Earnings)]
        public async Task<ActionResult> EarningsEditModal(Guid earningId)
        {
            var earningDetail = await _earningsAppService.GetEarningsForEdit(new EntityDto<Guid>(earningId));
            var earningCategoryList = await _earningsAppService.GetEarningCategoriesList();
            var model = new EditEarningsViewModel
            {
                Earnings = earningDetail,
                EarningCategoryList = earningCategoryList
            };
            return PartialView("_EarningsEditModal", model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Expenses)]
        public async Task<ActionResult> EarningsDuplicateModal(Guid earningId)
        {
            var expenseDetail = await _earningsAppService.GetEarningsForEdit(new EntityDto<Guid>(earningId));
            var earningCategoryList = await _earningsAppService.GetEarningCategoriesList();
            var model = new EditEarningsViewModel
            {
                Earnings = expenseDetail,
                EarningCategoryList = earningCategoryList
            };
            model.Earnings.Id = Guid.Empty;
            model.Earnings.EarningDate = DateTime.Now;
            return PartialView("_EarningsEditModal", model);
        }

        #endregion Earnings

        #region Investment

        #region ShareMarket

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_ShareMarket)]
        public IActionResult ShareMaster()
        {

            var totalInvestments = Task.Run(async () => await _shareMasterAppService.CalculateTotalInvested(null)).Result;

            if (User.IsInRole(EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin))
            {
                var usersList = Task.Run(async () => await _userAppService.GetUsersList()).Result;
                return View("ShareMaster", new ShareMasterListViewModel()
                {
                    ShareMasterDto = new CreateOrEditShareMasterDto(),
                    UsersList = usersList,
                    TotalInvestments = totalInvestments
                });
            }

            return View("ShareMaster", new ShareMasterListViewModel()
            {
                ShareMasterDto = new CreateOrEditShareMasterDto(),
                TotalInvestments = totalInvestments
            });
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_ShareMarket)]
        public async Task<ActionResult> ShareMasterEditModal(Guid shareMasterId)
        {
            var shareMasterDetail = await _shareMasterAppService.GetShareMasterForEdit(new EntityDto<Guid>(shareMasterId));
            var expenseCategoryList = await _expensesAppService.GetExpenseCategoriesListAsync();
            var model = new EditShareMasterViewModel
            {
                ShareMasterDto = shareMasterDetail
            };
            return PartialView("_ShareMasterEditModal", model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_ShareMarket)]
        public async Task<IActionResult> ShareOrders(Guid shareMasterId)
        {
            var shareMasterDetail = await _shareMasterAppService.GetShareMasterForEdit(new EntityDto<Guid>(shareMasterId));
            return View("ShareOrders", new ShareOrdersListViewModel()
            {
                ShareOrdersDto = new CreateOrEditShareOrdersDto() { EL_Financial_Investment_Share_Master_Id = shareMasterId },
                ShareMasterId = shareMasterId,
                ShareMasterName = shareMasterDetail.Share_Name
            });
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_ShareMarket)]
        public async Task<ActionResult> ShareOrdersEditModal(Guid shareOrderId)
        {
            var shareOrdersDetail = await _shareOrdersAppService.GetShareOrderForEdit(new EntityDto<Guid>(shareOrderId));
            var model = new EditShareOrdersViewModel
            {
                ShareOrdersDto = shareOrdersDetail
            };
            return PartialView("_ShareOrdersEditModal", model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_ShareMarket)]
        public async Task<ActionResult> ShareOrderDuplicateModal(Guid shareOrderId)
        {
            var shareOrderDetail = await _shareOrdersAppService.GetShareOrderForEdit(new EntityDto<Guid>(shareOrderId));
            var model = new EditShareOrdersViewModel
            {
                ShareOrdersDto = shareOrderDetail
            };
            model.ShareOrdersDto.Id = Guid.Empty;
            model.ShareOrdersDto.Share_Order_Date = DateTime.Now;
            return PartialView("_ShareOrdersEditModal", model);
        }

        #endregion ShareMarket

        #region SIP

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_SIP)]
        public IActionResult SIPMaster()
        {

            var totalInvestments = Task.Run(async () => await _sipMasterAppService.CalculateTotalInvested(null)).Result;

            if (User.IsInRole(EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin))
            {
                var usersList = Task.Run(async () => await _userAppService.GetUsersList()).Result;
                return View("SIPMaster", new SIPMasterListViewModel()
                {
                    SIPMasterDto = new CreateOrEditSIPMasterDto(),
                    UsersList = usersList,
                    TotalInvestments = totalInvestments
                });
            }

            return View("SIPMaster", new SIPMasterListViewModel()
            {
                SIPMasterDto = new CreateOrEditSIPMasterDto(),
                TotalInvestments = totalInvestments
            });
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_SIP)]
        public async Task<ActionResult> SIPMasterEditModal(Guid sipMasterId)
        {
            var sipMasterDetail = await _sipMasterAppService.GetSIPMasterForEdit(new EntityDto<Guid>(sipMasterId));
            var expenseCategoryList = await _expensesAppService.GetExpenseCategoriesListAsync();
            var model = new EditSIPMasterViewModel
            {
                SIPMasterDto = sipMasterDetail
            };
            return PartialView("_SIPMasterEditModal", model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_SIP)]
        public async Task<IActionResult> SIPEntries(Guid sipMasterId)
        {
            var sipMasterDetail = await _sipMasterAppService.GetSIPMasterForEdit(new EntityDto<Guid>(sipMasterId));
            return View("SIPEntries", new SIPEntriesListViewModel()
            {
                SIPEntriesDto = new CreateOrEditSIPEntriesDto() { EL_Financial_Investment_SIP_Master_Id = sipMasterId },
                SIPMasterId = sipMasterId,
                SIPMasterName = sipMasterDetail.SIP_Name
            });
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_SIP)]
        public async Task<ActionResult> SIPEntriesEditModal(Guid sipEntryId)
        {
            var sipEntriesDetail = await _sipEntriesAppService.GetSIPEntryForEdit(new EntityDto<Guid>(sipEntryId));
            var model = new EditSIPEntriesViewModel
            {
                SIPEntriesDto = sipEntriesDetail
            };
            return PartialView("_SIPEntriesEditModal", model);
        }

        [AbpMvcAuthorize(PermissionNames.Pages_Financial_Investment_SIP)]
        public async Task<ActionResult> SIPEntriesDuplicateModal(Guid sipEntryId)
        {
            var sipEntryDetail = await _sipEntriesAppService.GetSIPEntryForEdit(new EntityDto<Guid>(sipEntryId));
            var model = new EditSIPEntriesViewModel
            {
                SIPEntriesDto = sipEntryDetail
            };
            model.SIPEntriesDto.Id = Guid.Empty;
            model.SIPEntriesDto.SIP_Order_Date = DateTime.Now;
            return PartialView("_SIPEntriesEditModal", model);
        }

        #region SIP

        #endregion

        #endregion

        #endregion Investment
    }
}
