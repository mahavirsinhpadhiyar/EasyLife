using Abp.Application.Services;
using EasyLife.MultiTenancy.Dto;

namespace EasyLife.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

