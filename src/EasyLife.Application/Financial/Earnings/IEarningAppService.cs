using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Earnings.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyLife.Financial.Earnings
{
    /// <summary>
    /// Interface of EarningAppService
    /// </summary>
    public interface IEarningAppService : IAsyncCrudAppService<CreateOrEditEarningsDto, Guid, PagedEarningsResultRequestDto, CreateOrEditEarningsDto, CreateOrEditEarningsDto>
    {
        /// <summary>
        /// Edit earnings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreateOrEditEarningsDto> GetEarningsForEdit(EntityDto<Guid> input);
        /// <summary>
        /// Returns list of EarningCategories
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetEarningCategoriesList();
        /// <summary>
        /// Returns earnings in calendar events form
        /// </summary>
        /// <returns></returns>
        List<CalendarEarningEvents> GetCalendarEarnings();
    }
}
