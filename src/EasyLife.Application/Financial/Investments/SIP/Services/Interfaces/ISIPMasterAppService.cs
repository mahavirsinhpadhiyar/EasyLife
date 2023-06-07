using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto;
using EasyLife.Financial.Investments.SIP.Dto;
using EasyLife.Financial.Investments.SIP.Dto.SIPMaster;
using System;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.SIP.Services.Interfaces
{
    /// <summary>
    /// SIP Master interface
    /// </summary>
    public interface ISIPMasterAppService : IAsyncCrudAppService<CreateOrEditSIPMasterDto, Guid, PagedSIPMasterResultRequestDto, CreateOrEditSIPMasterDto, CreateOrEditSIPMasterDto>
    {
        Task<CreateOrEditSIPMasterDto> GetSIPMasterForEdit(EntityDto<Guid> input);
        Task<string> CalculateTotalInvested(long? FilterUserId);
    }
}
