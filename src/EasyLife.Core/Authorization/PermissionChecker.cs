using Abp.Authorization;
using EasyLife.Authorization.Roles;
using EasyLife.Authorization.Users;

namespace EasyLife.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
