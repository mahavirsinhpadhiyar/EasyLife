using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using EasyLife.Authorization;
using EasyLife.Financial.Expenses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace EasyLife.Financial.Expenses
{
    /// <summary>
    /// Manages financial expenses related operations
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Financial_Expenses)]
    public class ExpensesAppService : AsyncCrudAppService<Expenses, CreateOrEditExpensesDto, Guid, PagedExpensesResultRequestDto, CreateOrEditExpensesDto, CreateOrEditExpensesDto>, IExpensesAppService
    {
        private readonly IRepository<Expenses, Guid> _expensesRepository;
        private readonly IRepository<ExpenseCategory, Guid> _expenseCategoryRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expensesRepository"></param>
        /// <param name="expenseCategoryRepository"></param>
        public ExpensesAppService(IRepository<Expenses, Guid> expensesRepository, IRepository<ExpenseCategory, Guid> expenseCategoryRepository) : base(expensesRepository)
        {
            _expensesRepository = expensesRepository;
            _expenseCategoryRepository = expenseCategoryRepository;
        }

        /// <summary>
        /// Will return the list of paging wise expenses with filtered result
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<CreateOrEditExpensesDto>> GetAllFiltered(PagedExpensesResultRequestDto input)
        {
            var expensesList = new List<Expenses>();
            if (input.FilterUserId == null || input.FilterUserId == 0)
                input.FilterUserId = AbpSession.UserId;
            var query = _expensesRepository.GetAll().Include(x => x.ExpenseCategory).Where(x => x.UserId == input.FilterUserId);

            query = ApplySorting(query, input);

            expensesList = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Payee.Contains(input.Keyword))
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.ExpenseCategory.Id == input.CategoryId)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(e => e.ExpenseDate)
                .ToList();

            var expensesFilterList = expensesList.Select(r => new CreateOrEditExpensesDto()
            {
                ConsiderInTotal = r.ConsiderInTotal,
                Payee = r.Payee,
                ExpenseCategoryId = r.ExpenseCategoryId,
                ExpenseDate = r.ExpenseDate,
                ExpenseCategoryName = r.ExpenseCategory.CategoryName,
                Id = r.Id,
                Money = r.Money,
                Note = r.Note,
                UserId = r.UserId
            }).ToList();

            var result = new PagedResultDto<CreateOrEditExpensesDto>(query.Count(), expensesFilterList);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Overridden ApplySorting
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<Expenses> ApplySorting(IQueryable<Expenses> query, PagedExpensesResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty() || input.Sorting == "0 asc")
            {
                input.Sorting = "Payee desc";
            }
            return base.ApplySorting(query, input);
        }

        /// <summary>
        /// Create expenses entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditExpensesDto> CreateAsync(CreateOrEditExpensesDto input)
        {
            try
            {
                input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
                input.UserId = AbpSession.UserId.Value;
                var expenses = ObjectMapper.Map<Expenses>(input);
                await _expensesRepository.InsertAsync(expenses);
                return MapToEntityDto(expenses);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update expenses
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditExpensesDto> UpdateAsync(CreateOrEditExpensesDto input)
        {
            try
            {
                input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
                input.UserId = AbpSession.UserId.Value;
                var expenses = await _expensesRepository.GetAsync(input.Id);
                ObjectMapper.Map(input, expenses);
                await _expensesRepository.UpdateAsync(expenses);
                return MapToEntityDto(expenses);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit expenses
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreateOrEditExpensesDto> GetExpensesForEdit(EntityDto<Guid> input)
        {
            var output = await _expensesRepository.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditExpensesDto>(output);
            return ClassObj;
        }

        /// <summary>
        /// Returns list of ExpenseCategories
        /// </summary>
        /// <param name="RinkId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetExpenseCategoriesList()
        {
            return _expenseCategoryRepository.GetAll().OrderByDescending(e => e.CategoryName).Select(r =>
            new SelectListItem()
            {
                Text = r.CategoryName,
                Value = r.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// Returns expenses in calendar events form
        /// </summary>
        /// <returns></returns>
        public List<CalendarEvents> GetCalendarExpenses()
        {
            var calendarEvents = _expensesRepository.GetAll().Include(x => x.ExpenseCategory).Where(x => x.UserId == AbpSession.UserId);
            return calendarEvents.Select(e => new CalendarEvents()
            {
                Id = e.Id,
                Start = e.ExpenseDate,
                End = e.ExpenseDate,
                Description = e.Note,
                Name = e.Payee
            }).ToList();
        }
    }
}