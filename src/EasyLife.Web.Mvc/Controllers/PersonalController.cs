using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using EasyLife.Authorization;
using EasyLife.Controllers;
using EasyLife.Personal.EncryptedImportantThings;
using EasyLife.Users;
using EasyLife.Web.Models.Personal.EncryptedImportantThings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Controllers
{
    public class PersonalController : EasyLifeControllerBase
    {
        private readonly IEncryptedImportantInformationAppService _encryptedImportantInformationAppService;
        private readonly IUserAppService _userAppService;
        public PersonalController(IEncryptedImportantInformationAppService encryptedImportantInformationAppService, IUserAppService userAppService)
        {
            _encryptedImportantInformationAppService = encryptedImportantInformationAppService;
            _userAppService = userAppService;
        }
        #region EncryptedImportantInformation
        [AbpMvcAuthorize(PermissionNames.Pages_Personal_EncryptedImportantInformation)]
        public IActionResult EncryptedImportantInformationList()
        {
            if (User.IsInRole(EasyLife.Authorization.Roles.StaticRoleNames.Host.Admin))
            {
                var usersList = Task.Run(async () => await _userAppService.GetUsersList()).Result;
                return View("EncryptedImportantInformationList", new ImportantInformationListViewModel()
                {
                    ImportantInformationViewModel = new Personal.EncryptedImportantThings.Dto.CreateOrEditEncryptedImportantInformationDto(),
                    UsersList = usersList
                });
            }

            return View("EncryptedImportantInformationList", new ImportantInformationListViewModel()
            {
                ImportantInformationViewModel = new Personal.EncryptedImportantThings.Dto.CreateOrEditEncryptedImportantInformationDto()
            });
        }
        [AbpMvcAuthorize(PermissionNames.Pages_Personal_EncryptedImportantInformation)]
        public async Task<ActionResult> EncryptedImportantInformationEditModal(Guid Id)
        {
            var encryptedImportantInformationDetail = await _encryptedImportantInformationAppService.GetEncryptedImportantInformationForEdit(new EntityDto<Guid>(Id));
            var model = new EditImportantInformationListViewModel
            {
                ImportantInformationViewModel = encryptedImportantInformationDetail
            };
            return PartialView("_EncryptedImportantInformationEditModal", model);
        }
        #endregion EncryptedImportantInformation
    }
}
