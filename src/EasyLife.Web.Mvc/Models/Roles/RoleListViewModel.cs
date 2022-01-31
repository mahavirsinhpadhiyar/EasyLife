using System.Collections.Generic;
using EasyLife.Roles.Dto;

namespace EasyLife.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
