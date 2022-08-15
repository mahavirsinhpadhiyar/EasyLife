using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EasyLife.Financial.Investments.ShareMarket.Dto.ShareOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLife.Financial.Investments.ShareMarket.Services.Interfaces
{
    public interface IShareOrdersAppService : IAsyncCrudAppService<CreateOrEditShareOrdersDto, Guid, PagedShareOrdersResultRequestDto, CreateOrEditShareOrdersDto, CreateOrEditShareOrdersDto>
    {
        Task<CreateOrEditShareOrdersDto> GetShareOrderForEdit(EntityDto<Guid> input);
        OrderCalculationDetails GetAverageSharePrice(Guid shareMasterId);
    }
}
