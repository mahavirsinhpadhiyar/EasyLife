using System.Threading.Tasks;
using Abp.Application.Services;
using EasyLife.Authorization.Accounts.Dto;

namespace EasyLife.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);
        Task<RegisterOutput> Register(RegisterInput input);
        Task<ForgotPassword> SendPasswordResetCode(ForgotPassword forgotPassword);
        Task<ResetPassword> ResetPasswordAsync(ResetPassword resetPassword);
    }
}
