using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using EasyLife.Authorization;
using EasyLife.Financial.Earning;
using EasyLife.Financial.Earnings.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace EasyLife.Financial.Earnings
{
    /// <summary>
    /// Manages financial earning related operations
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Financial_Earnings)]
    public class EarningsAppService : AsyncCrudAppService<Financial.Earning.Earnings, CreateOrEditEarningsDto, Guid, PagedEarningsResultRequestDto, CreateOrEditEarningsDto, CreateOrEditEarningsDto>, IEarningAppService
    {
        private readonly IRepository<Financial.Earning.Earnings, Guid> _earningsRepository;
        private readonly IRepository<EarningCategory, Guid> _earningCategoryRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="earningsRepository"></param>
        /// <param name="earningCategoryRepository"></param>
        public EarningsAppService(IRepository<Financial.Earning.Earnings, Guid> earningsRepository, IRepository<EarningCategory, Guid> earningCategoryRepository) : base(earningsRepository)
        {
            _earningsRepository = earningsRepository;
            _earningCategoryRepository = earningCategoryRepository;
        }

        /// <summary>
        /// Will return the list of paging wise earnings with filtered result
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<CreateOrEditEarningsDto>> GetAllFiltered(PagedEarningsResultRequestDto input)
        {
            var earningsList = new List<Financial.Earning.Earnings>();
            if (input.FilterUserId == null || input.FilterUserId == 0)
                input.FilterUserId = AbpSession.UserId;
            var query = _earningsRepository.GetAll().Include(x => x.EarningCategory).Where(x => x.UserId == input.FilterUserId);

            query = ApplySorting(query, input);

            earningsList = query
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Payee.Contains(input.Keyword))
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.EarningCategory.Id == input.CategoryId)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(e => e.EarningCategory)
                .ToList();

            var earningsFilterList = earningsList.Select(r => new CreateOrEditEarningsDto()
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

            var result = new PagedResultDto<CreateOrEditEarningsDto>(query.Count(), earningsFilterList);
            return Task.FromResult(result);
        }

        /// <summary>
        /// Overridden ApplySorting
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<Financial.Earning.Earnings> ApplySorting(IQueryable<Financial.Earning.Earnings> query, PagedEarningsResultRequestDto input)
        {
            if (input.Sorting.IsNullOrEmpty() || input.Sorting == "0 asc")
            {
                input.Sorting = "Payee desc";
            }
            return base.ApplySorting(query, input);
        }

        /// <summary>
        /// Create earnings entry
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<CreateOrEditEarningsDto> CreateAsync(CreateOrEditEarningsDto input)
        {
            try
            {
                input.EarningDate = new DateTime(input.EarningDate.Year, input.EarningDate.Month, input.EarningDate.Day);
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
        public override async Task<CreateOrEditEarningsDto> UpdateAsync(CreateOrEditEarningsDto input)
        {
            try
            {
                input.EarningDate = new DateTime(input.EarningDate.Year, input.EarningDate.Month, input.EarningDate.Day);
                input.UserId = AbpSession.UserId.Value;
                var earnings = await _earningsRepository.GetAsync(input.Id);
                ObjectMapper.Map(input, earnings);
                await _earningsRepository.UpdateAsync(earnings);
                return MapToEntityDto(earnings);
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
        public async Task<CreateOrEditEarningsDto> GetEarningsForEdit(EntityDto<Guid> input)
        {
            var output = await _earningsRepository.FirstOrDefaultAsync(s => s.Id == input.Id);
            var ClassObj = ObjectMapper.Map<CreateOrEditEarningsDto>(output);
            return ClassObj;
        }

        /// <summary>
        /// Returns list of EarningCategories
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetEarningCategoriesList()
        {
            return _earningCategoryRepository.GetAll().OrderByDescending(e => e.CategoryName).Select(r =>
            new SelectListItem()
            {
                Text = r.CategoryName,
                Value = r.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// Returns earnings in calendar events form
        /// </summary>
        /// <returns></returns>
        public List<CalendarEarningEvents> GetCalendarEarnings()
        {
            var calendarEvents = _earningsRepository.GetAll().Include(x => x.EarningCategory).Where(x => x.UserId == AbpSession.UserId);
            return calendarEvents.Select(e => new CalendarEarningEvents()
            {
                Id = e.Id,
                Start = e.EarningDate,
                End = e.EarningDate,
                Description = e.Note,
                Name = e.Payee
            }).ToList();
        }
    }
}