using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Investments.SIP.Dto.SIPEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.SIP.Services.Interfaces
{
    public interface ISIPEntriesAppService : IAsyncCrudAppService<CreateOrEditSIPEntriesDto, Guid, PagedSIPEntriesResultRequestDto, CreateOrEditSIPEntriesDto, CreateOrEditSIPEntriesDto>
    {
        Task<CreateOrEditSIPEntriesDto> GetSIPEntryForEdit(EntityDto<Guid> input);
        ////OrderCalculationDetails GetAverageSharePrice(Guid sipMasterId);
    }
}
