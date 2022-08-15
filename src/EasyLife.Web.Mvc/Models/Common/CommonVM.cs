using EasyLife.Authorization.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Common
{
    public class CommonVM
    {
        public RoleEnums.Roles LoggedInUserIsAdmin { get; set; }
    }
}
