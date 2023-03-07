using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using EasyLife.Authorization;
using EasyLife.Authorization.Users;
using EasyLife.Financial.Expenses.Dto;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareMaster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using data = EasyLife.Financial.Expenses.Dto.data;
using emphasis = EasyLife.Financial.Expenses.Dto.emphasis;
using itemStyle = EasyLife.Financial.Expenses.Dto.itemStyle;
using series = EasyLife.Financial.Expenses.Dto.series;

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
        private readonly UserManager _userManager;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expensesRepository"></param>
        /// <param name="expenseCategoryRepository"></param>
        /// /// <param name="userManager"></param>
        public ExpensesAppService(IRepository<Expenses, Guid> expensesRepository, IRepository<ExpenseCategory, Guid> expenseCategoryRepository, UserManager userManager) : base(expensesRepository)
        {
            _expensesRepository = expensesRepository;
            _expenseCategoryRepository = expenseCategoryRepository;
            _userManager = userManager;
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
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Payee.ToLower().Contains(input.Keyword.ToLower()))
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.ExpenseCategory.Id == input.CategoryId)
                .WhereIf(input.FilterStartDate.HasValue, x => x.ExpenseDate >= input.FilterStartDate.Value)
                .WhereIf(input.FilterEndDate.HasValue, x => x.ExpenseDate <= input.FilterEndDate.Value)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(e => e.ExpenseDate)
                .ToList();

            var pageCount = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Payee.Contains(input.Keyword))
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.ExpenseCategory.Id == input.CategoryId)
                .WhereIf(input.FilterStartDate.HasValue, x => x.ExpenseDate >= input.FilterStartDate.Value)
                .WhereIf(input.FilterEndDate.HasValue, x => x.ExpenseDate <= input.FilterEndDate.Value)
                .OrderByDescending(e => e.ExpenseDate)
                .Count();

            var expensesFilterList = expensesList.Select(r => new CreateOrEditExpensesDto()
            {
                DoNotConsiderInTotal = r.DoNotConsiderInTotal,
                Payee = r.Payee,
                ExpenseCategoryId = r.ExpenseCategoryId,
                ExpenseDate = r.ExpenseDate,
                ExpenseCategoryName = r.ExpenseCategory.CategoryName,
                Id = r.Id,
                Money = r.Money,
                Note = r.Note,
                UserId = r.UserId
            }).ToList();

            var result = new PagedResultDto<CreateOrEditExpensesDto>(pageCount, expensesFilterList);
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
                input.Sorting = "ExpenseDate desc";
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
                //DateTime dt = DateTime.Now;
                //input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
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
                //For duplicate entry
                if (input.Id == Guid.Empty)
                {
                    input.UserId = AbpSession.UserId.Value;
                    var expenses = ObjectMapper.Map<Expenses>(input);
                    expenses.Id = await _expensesRepository.InsertAndGetIdAsync(expenses);
                    return MapToEntityDto(expenses);
                }
                else
                {
                    //input.ExpenseDate = new DateTime(input.ExpenseDate.Year, input.ExpenseDate.Month, input.ExpenseDate.Day);
                    input.UserId = AbpSession.UserId.Value;
                    var expenses = await _expensesRepository.GetAsync(input.Id);
                    ObjectMapper.Map(input, expenses);
                    await _expensesRepository.UpdateAsync(expenses);
                    return MapToEntityDto(expenses);
                }
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
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetExpenseCategoriesListAsync()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (await _userManager.IsInRoleAsync(user, EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin))
            {
                return _expenseCategoryRepository.GetAll().OrderBy(e => e.CategoryName).Select(r =>
                new SelectListItem()
                {
                    Text = r.CategoryName,
                    Value = r.Id.ToString()
                }).ToList();
            }
            else
            {
                return _expenseCategoryRepository.GetAll().Where(e => e.IsActive == true && e.IsForMeActive == false).OrderBy(e => e.CategoryName).Select(r =>
                new SelectListItem()
                {
                    Text = r.CategoryName,
                    Value = r.Id.ToString()
                }).ToList();
            }
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
        /// <summary>
        /// Returns total of all expenses
        /// </summary>
        /// <returns></returns>
        public async Task<string> DashboardTotalExpensesSum(DateTime? monthStartDate, DateTime? monthEndDate)
        {
            monthStartDate = monthStartDate.HasValue ? monthStartDate.Value : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            monthEndDate = monthEndDate.HasValue ? monthEndDate.Value : monthStartDate.Value.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            return _expensesRepository.GetAll().Where(e => e.ExpenseDate >= monthStartDate && e.ExpenseDate <= monthEndDate && e.UserId == AbpSession.UserId && !e.DoNotConsiderInTotal).SumAsync(e => e.Money).Result.ToLocalMoneyFormat();
        }
        /// <summary>
        /// Deletes the expense
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            var expenseDetails = await _expensesRepository.GetAsync(input.Id);
            await _expensesRepository.HardDeleteAsync(expenseDetails);
        }

        /// <summary>
        /// Multple expense entry in one call
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CreateListAsync(List<CreateOrEditExpensesDto> input)
        {
            try
            {
                foreach (var expense in input)
                {
                    expense.UserId = AbpSession.UserId.Value;
                    var expenses = ObjectMapper.Map<Expenses>(expense);
                    await _expensesRepository.InsertAsync(expenses);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BarChartExpenses GetExpensesBarDetails(long? FilterUserId, DateTime? FilterStartDate, DateTime? FilterEndDate)
        {
            if (FilterUserId == null || FilterUserId == 0)
                FilterUserId = AbpSession.UserId;

            var expensesList = new List<Expenses>();

            if (FilterStartDate == null && FilterEndDate == null) {
                FilterStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                FilterEndDate = new DateTime(DateTime.Now.Year, 12, 31);
            }

            if (FilterStartDate != null && FilterEndDate != null)
            {
                expensesList = _expensesRepository.GetAll()
                    .Include(e => e.ExpenseCategory)
                    .Where(i => i.ExpenseDate >= FilterStartDate && i.ExpenseDate <= FilterEndDate && i.UserId == FilterUserId && !i.DoNotConsiderInTotal)
                    .Select(n => new Expenses()
                    {
                        CreationTime = n.CreationTime,
                        CreatorUserId = n.CreatorUserId,
                        DeleterUserId = n.DeleterUserId,
                        DeletionTime = n.DeletionTime,
                        DoNotConsiderInTotal = n.DoNotConsiderInTotal,
                        ExpenseCategory = n.ExpenseCategory,
                        ExpenseDate = n.ExpenseDate,
                        ExpenseCategoryId = n.ExpenseCategoryId,
                        Id = n.Id,
                        IsDeleted = n.IsDeleted,
                        LastModificationTime = n.LastModificationTime,
                        LastModifierUserId = n.LastModifierUserId,
                        Money = n.Money,
                        Note = n.Note,
                        Payee = n.Payee,
                        UserId = n.UserId
                    })
                    .OrderBy(s => s.ExpenseCategory)
                    .ToList();
            }
            else
            {
                expensesList = _expensesRepository.GetAll()
                    .Include(e => e.ExpenseCategory)
                    .Where(i => i.UserId == FilterUserId)
                    .OrderBy(s => s.ExpenseCategory)
                    .ToList();
            }

            var resultData = expensesList
                        .OrderBy(x => x.ExpenseCategory.CategoryName)
                        .GroupBy(e => e.ExpenseCategory.CategoryName)
                        .Select(x => new data()
                        {
                            name = x.Key,
                            value = x.Sum(items => items.Money).ToString()
                        }).ToList();

            //var result = _expensesRepository.GetAll()
            //        .Include(e => e.ExpenseCategory)
            //        .GroupBy(ex => ex.ExpenseCategory.CategoryName, (key, value) => new data() { name = value, value = exp.Sum() });

            BarChartExpenses barChartExpenses = new BarChartExpenses();

            //Hide as share names overrite the main heading
            //barChartShareMasterWithOrders.title.text = "Shares Count";        
            //barChartShareMasterWithOrders.title.left = "center";

            barChartExpenses.tooltip.trigger = "item";
            barChartExpenses.legend.orient = "horizontal";
            barChartExpenses.legend.left = "center";
            barChartExpenses.series.Add(
                new series()
                {
                    name = "Expenses Total Counts",
                    type = "pie",
                    radius = "50%",
                    data = resultData,
                    emphasis = new emphasis() { itemStyle = new itemStyle() { shadowBlur = 10, shadowOffsetX = 0, shadowColor = "rgba(0, 0, 0, 0.5)" } }
                }
                );

            return barChartExpenses;
        }
    }
}