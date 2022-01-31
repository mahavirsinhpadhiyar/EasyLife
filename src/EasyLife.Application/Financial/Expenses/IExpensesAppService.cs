using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Expenses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Expenses
{
    public interface IExpensesAppService : IAsyncCrudAppService<CreateOrEditExpensesDto, Guid, PagedExpensesResultRequestDto, CreateOrEditExpensesDto, CreateOrEditExpensesDto>
    {
        Task<CreateOrEditExpensesDto> GetExpensesForEdit(EntityDto<Guid> input);
        List<SelectListItem> GetExpenseCategoriesList();
        List<CalendarEvents> GetCalendarExpenses();
    }
}
