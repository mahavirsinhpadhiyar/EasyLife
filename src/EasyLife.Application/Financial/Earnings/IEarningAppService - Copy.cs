using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Earnings.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyLife.Financial.Earnings
{
    public interface IEarningAppService : IAsyncCrudAppService<CreateOrEditEarningsDto, Guid, PagedEarningsResultRequestDto, CreateOrEditEarningsDto, CreateOrEditEarningsDto>
    {
        Task<CreateOrEditEarningsDto> GetEarningsForEdit(EntityDto<Guid> input);
        List<SelectListItem> GetEarningCategoriesList();
        List<CalendarEarningEvents> GetCalendarEarnings();
    }
}
