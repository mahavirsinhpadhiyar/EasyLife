using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Expenses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyLife.Financial.Expenses
{
    public interface IExpensesAppService : IAsyncCrudAppService<CreateOrEditExpensesDto, Guid, PagedExpensesResultRequestDto, CreateOrEditExpensesDto, CreateOrEditExpensesDto>
    {
        Task<CreateOrEditExpensesDto> GetExpensesForEdit(EntityDto<Guid> input);
        Task<List<SelectListItem>> GetExpenseCategoriesListAsync();
        List<CalendarEvents> GetCalendarExpenses();
        Task<string> DashboardTotalExpensesSum(DateTime? monthStartDate, DateTime? monthEndDate);
    }
}
