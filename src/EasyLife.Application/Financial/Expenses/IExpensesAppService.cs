using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Expenses.Dto;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareMaster;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyLife.Financial.Expenses
{
    /// <summary>
    /// Interface of ExpensesAppService
    /// </summary>
    public interface IExpensesAppService : IAsyncCrudAppService<CreateOrEditExpensesDto, Guid, PagedExpensesResultRequestDto, CreateOrEditExpensesDto, CreateOrEditExpensesDto>
    {
        /// <summary>
        /// Return expense details
        /// </summary>
        /// <param name="input">EntityDto</param>
        /// <returns></returns>
        Task<CreateOrEditExpensesDto> GetExpensesForEdit(EntityDto<Guid> input);
        /// <summary>
        /// Return expense category list
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetExpenseCategoriesListAsync();
        /// <summary>
        /// Return expense calendar details
        /// </summary>
        /// <returns></returns>
        List<CalendarEvents> GetCalendarExpenses();
        /// <summary>
        /// Return Total Expenses Sum
        /// </summary>
        /// <param name="monthStartDate">Month Start Date</param>
        /// <param name="monthEndDate">Month End Date</param>
        /// <returns></returns>
        Task<string> DashboardTotalExpensesSum(DateTime? monthStartDate, DateTime? monthEndDate);
        /// <summary>
        /// Create multiple entries of expenses
        /// </summary>
        /// <param name="input">Expense Dto list</param>
        /// <returns></returns>
        Task<bool> CreateListAsync(List<CreateOrEditExpensesDto> input);
        /// <summary>
        /// Return chart report details of expenses
        /// </summary>
        /// <param name="FilterUserId">Selected User Id</param>
        /// <param name="FilterStartDate">Start Date</param>
        /// <param name="FilterEndDate">End Date</param>
        /// <returns></returns>
        BarChartExpenses GetExpensesBarDetails(long? FilterUserId, DateTime? FilterStartDate, DateTime? FilterEndDate);
    }
}
