using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace EasyLife.Authorization
{
    public class EasyLifeAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Account_Change_Password, L("Account_Change_Password"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Financial_Expenses, L("Expenses"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EasyLifeConsts.LocalizationSourceName);
        }
    }
}
