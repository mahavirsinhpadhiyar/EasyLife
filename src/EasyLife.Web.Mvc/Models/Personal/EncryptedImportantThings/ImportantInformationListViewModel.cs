using EasyLife.Personal.EncryptedImportantThings.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Personal.EncryptedImportantThings
{
    public class ImportantInformationListViewModel
    {
        public CreateOrEditEncryptedImportantInformationDto ImportantInformationViewModel { get; set; }
        public List<SelectListItem> UsersList { get; set; }
    }
}
