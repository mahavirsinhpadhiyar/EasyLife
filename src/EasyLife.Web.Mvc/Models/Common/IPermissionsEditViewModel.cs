using System.Collections.Generic;
using EasyLife.Roles.Dto;

namespace EasyLife.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}