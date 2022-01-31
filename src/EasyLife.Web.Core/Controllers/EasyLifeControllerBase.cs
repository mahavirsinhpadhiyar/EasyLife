using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace EasyLife.Controllers
{
    public abstract class EasyLifeControllerBase: AbpController
    {
        protected EasyLifeControllerBase()
        {
            LocalizationSourceName = EasyLifeConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
