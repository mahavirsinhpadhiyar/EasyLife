using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Earning.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyLife.Financial.Earning
{
    /// <summary>
    /// Interface of EarningAppService
    /// </summary>
    public interface IEarningAppService : IAsyncCrudAppService<CreateOrEditEarningDto, Guid, PagedEarningResultRequestDto, CreateOrEditEarningDto, CreateOrEditEarningDto>
    {
        /// <summary>
        /// Edit earnings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreateOrEditEarningDto> GetEarningsForEdit(EntityDto<Guid> input);
        /// <summary>
        /// Returns list of EarningCategories
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetEarningCategoriesList();
        /// <summary>
        /// Returns earnings in calendar events form
        /// </summary>
        /// <returns></returns>
        List<CalendarEarningEvent> GetCalendarEarnings();
        /// <summary>
        /// Returns total earnings in count
        /// </summary>
        /// <returns></returns>
        Task<string> DashboardTotalEarnings(DateTime? monthStartDate, DateTime? monthEndDate);
    }
}
