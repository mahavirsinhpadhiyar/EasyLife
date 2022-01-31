using System.Threading.Tasks;
using Abp.Application.Services;
using EasyLife.Sessions.Dto;

namespace EasyLife.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
