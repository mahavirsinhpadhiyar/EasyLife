using System.Collections.Generic;
using EasyLife.Roles.Dto;

namespace EasyLife.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
