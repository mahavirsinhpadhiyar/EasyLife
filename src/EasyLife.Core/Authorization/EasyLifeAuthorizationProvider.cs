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
            context.CreatePermission(PermissionNames.Pages_Financial_Earnings, L("Earnings"));
            context.CreatePermission(PermissionNames.Pages_Personal_EncryptedImportantInformation, L("EncryptedImportantInformation"));
            context.CreatePermission(PermissionNames.Pages_Financial_Investment_SIP, L("Pages_Financial_Investment_SIP"));
            context.CreatePermission(PermissionNames.Pages_Financial_Investment_ShareMarket, L("Pages_Financial_Investment_ShareMarket"));
            context.CreatePermission(PermissionNames.Pages_Financial_Investment_Crypto, L("Pages_Financial_Investment_Crypto"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EasyLifeConsts.LocalizationSourceName);
        }
    }
}
