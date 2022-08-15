using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using EasyLife.Authorization.Roles;
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

        protected RoleEnums.Roles LoggedInUserRole()
        {
            return User.IsInRole(EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin) ? RoleEnums.Roles.Admin : User.IsInRole(EasyLife.Authorization.Roles.StaticRoleNames.Host.SuperAdmin) ? RoleEnums.Roles.SuperAdmin : RoleEnums.Roles.User;
        }
    }
}
