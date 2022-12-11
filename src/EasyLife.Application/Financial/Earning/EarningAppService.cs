using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using EasyLife.Authorization;
using EasyLife.Authorization.Users;
using EasyLife.Financial.Earning;
using EasyLife.Financial.Earning.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace EasyLife.Financial.Earning
{
    /// <summary>
    /// Manages financial earning related operations
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Financial_Earnings)]
    public class EarningAppService : AsyncCrudAppService<Financial.Earning.Earnings, CreateOrEditEarningDto, Guid, PagedEarningResultRequestDto, CreateOrEditEarningDto, CreateOrEditEarningDto>, IEarningAppService
    {
        private readonly IRepository<Financial.Earning.Earnings, Guid> _earningsRepository;
        private readonly IRepository<EarningCategory, Guid> _earningCategoryRepository;
        private readonly UserManager _userManager;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="earningsRepository"></param>
        /// <param name="earningCategoryRepository"></param>
        /// <param name="userManager"></param>
        public EarningAppService(IRepository<Financial.Earning.Earnings, Guid> earningsRepository, IRepository<EarningCategory, Guid> earningCategoryRepository, UserManager userManager) : base(earningsRepository)
        {
            _earningsRepository = earningsRepository;
            _earningCategoryRepository = earningCategoryRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Will return the list of paging wise earnings with filtered result
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<CreateOrEditEarningDto>> GetAllFiltered(PagedEarningResultRequestDto input)
        {
            var earningsList = new List<Financial.Earning.Earnings>();
            if (input.FilterUserId == null || input.FilterUserId == 0)
                input.FilterUserId = AbpSession.UserId;
            var query = _earningsRepository.GetAll().Include(x => x.EarningCategory).Where(x => x.UserId == input.FilterUserId);

            query = ApplySorting(query, input);
                
            earningsList = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Payee.ToLower().Contains(input.Keyword.ToLower()))
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.EarningCategory.Id == input.CategoryId)
                .WhereIf(input.FilterStartDate.HasValue, x => x.EarningDate >= input.FilterStartDate.Value)
                .WhereIf(input.FilterEndDate.HasValue, x => x.EarningDate <= input.FilterEndDate.Value)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                //.OrderByDescending(e => e.EarningDate)
                .ToList();

            var pageCount = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Payee.Contains(input.Keyword))
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.EarningCategory.Id == input.CategoryId)
                .WhereIf(input.FilterStartDate.HasValue, x => x.EarningDate >= input.FilterStartDate.Value)
                .WhereIf(input.FilterEndDate.HasValue, x => x.EarningDate <= input.FilterEndDate.Value)
                //.OrderByDescending(e => e.EarningDate)
                .Count();

            var earningsFilterList = earningsList.Select(r => new CreateOrEditEarningDto()
            {
                ConsiderInTotal = r.ConsiderInTotal,
                Payee = r.Payee,
                EarningCategoryId = r.EarningCategoryId,
                EarningDate = r.EarningDate,
                EarningCategoryName = r.EarningCategory.CategoryName,
                Id = r.Id,
                Money = r.Money,
                Note = r.Note,
                UserId = r.UserId
            }).ToList();

            var result = new PagedResultDto<CreateOrEditEarningDto>(pageCount, earningsFilterList);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Overridden ApplySorting
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<Financial.Earning.Earnings> ApplySorting(IQueryable<Financial.Earning.Earnings> query, PagedEarningResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty() || input.Sorting == "0 asc" || input.Sorting.Contains("earningDateDisplay"))
            {
                input.Sorting = input.Sorting.Contains("desc") ? "EarningDate desc" : "EarningDate asc";
            }
            else if (input.Sorting.Contains("payee"))
            {
                input.Sorting = input.Sorting.Contains("desc") ? "Payee desc" : "Payee asc";
            }
            return base.ApplySorting(query, input);
        }

        /// <summary>
        /// Create earnings entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditEarningDto> CreateAsync(CreateOrEditEarningDto input)
        {
            try
            {
                //input.EarningDate = new DateTime(input.EarningDate.Year, input.EarningDate.Month, input.EarningDate.Day);
                input.UserId = AbpSession.UserId.Value;
                var earnings = ObjectMapper.Map<Financial.Earning.Earnings>(input);
                await _earningsRepository.InsertAsync(earnings);
                return MapToEntityDto(earnings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update earnings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditEarningDto> UpdateAsync(CreateOrEditEarningDto input)
        {
            try
            {
                if (input.Id == Guid.Empty)
                {
                    //Duplicate earning
                    input.UserId = AbpSession.UserId.Value;
                    var earnings = ObjectMapper.Map<Financial.Earning.Earnings>(input);
                    await _earningsRepository.InsertAsync(earnings);
                    return MapToEntityDto(earnings);
                }
                else
                {
                    //Update existing earning
                    //input.EarningDate = new DateTime(input.EarningDate.Year, input.EarningDate.Month, input.EarningDate.Day);
                    input.UserId = AbpSession.UserId.Value;
                    var earnings = await _earningsRepository.GetAsync(input.Id);
                    ObjectMapper.Map(input, earnings);
                    await _earningsRepository.UpdateAsync(earnings);
                    return MapToEntityDto(earnings);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit earnings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreateOrEditEarningDto> GetEarningsForEdit(EntityDto<Guid> input)
        {
            var output = await _earningsRepository.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditEarningDto>(output);
            return ClassObj;
        }

        /// <summary>
        /// Returns list of EarningCategories
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetEarningCategoriesList()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (await _userManager.IsInRoleAsync(user, EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin))
            {
                return _earningCategoryRepository.GetAll().OrderBy(e => e.CategoryName).Select(r =>
                new SelectListItem()
                {
                    Text = r.CategoryName,
                    Value = r.Id.ToString()
                }).ToList();
            }
            else
            {
                return _earningCategoryRepository.GetAll().Where(e => e.IsActive == true && e.IsForMeActive == false).OrderBy(e => e.CategoryName).Select(r =>
                new SelectListItem()
                {
                    Text = r.CategoryName,
                    Value = r.Id.ToString()
                }).ToList();
            }
        }

        /// <summary>
        /// Returns earnings in calendar events form
        /// </summary>
        /// <returns></returns>
        public List<CalendarEarningEvent> GetCalendarEarnings()
        {
            var calendarEvents = _earningsRepository.GetAll().Include(x => x.EarningCategory).Where(x => x.UserId == AbpSession.UserId);
            return calendarEvents.Select(e => new CalendarEarningEvent()
            {
                Id = e.Id,
                Start = e.EarningDate,
                End = e.EarningDate,
                Description = e.Note,
                Name = e.Payee
            }).ToList();
        }
        /// <summary>
        /// Returns total of all earnings
        /// </summary>
        /// <returns></returns>
        public async Task<string> DashboardTotalEarnings(DateTime? monthStartDate, DateTime? monthEndDate)
        {
            monthStartDate = monthStartDate.HasValue ? monthStartDate.Value : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            monthEndDate = monthEndDate.HasValue ? monthEndDate.Value : monthStartDate.Value.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            return _earningsRepository.GetAll().Where(e => e.EarningDate >= monthStartDate && e.EarningDate <= monthEndDate && e.UserId == AbpSession.UserId).SumAsync(e => e.Money).Result.ToLocalMoneyFormat();
        }
        /// <summary>
        /// Deletes the earnings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            var earningDetails = await _earningsRepository.GetAsync(input.Id);
            await _earningsRepository.HardDeleteAsync(earningDetails);
        }
    }
}
