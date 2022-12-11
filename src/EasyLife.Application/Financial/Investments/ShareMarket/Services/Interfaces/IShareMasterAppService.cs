using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareMaster;
using EasyLife.Financial.Investments.ShareMarket.ShareMaster.Dto;
using System;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.ShareMarket.Services.Interfaces
{
    /// <summary>
    /// Share Master interface
    /// </summary>
    public interface IShareMasterAppService : IAsyncCrudAppService<CreateOrEditShareMasterDto, Guid, PagedShareMasterResultRequestDto, CreateOrEditShareMasterDto, CreateOrEditShareMasterDto>
    {
        Task<CreateOrEditShareMasterDto> GetShareMasterForEdit(EntityDto<Guid> input);
        BarChartShareMasterWithOrders GetShareMasterWithOrdersBarDetails(long? FilterUserId);
        Task<string> CalculateTotalInvested(long? FilterUserId);
    }
}
