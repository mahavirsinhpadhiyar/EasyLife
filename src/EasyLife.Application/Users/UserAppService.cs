using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using EasyLife.Authorization;
using EasyLife.Authorization.Roles;
using EasyLife.Authorization.Users;
using EasyLife.Roles.Dto;
using EasyLife.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Net.Mail;
using Microsoft.Extensions.Configuration;
using EasyLife.Configuration;

namespace EasyLife.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfigurationRoot _appConfiguration;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IEmailSender emailSender,
            IAppConfigurationAccessor appConfiguration)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _emailSender = emailSender;
            _appConfiguration = appConfiguration.Configuration;
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            try
            {
                CheckCreatePermission();

                var user = ObjectMapper.Map<User>(input);

                user.TenantId = AbpSession.TenantId;
                user.IsEmailConfirmed = false;
                user.SetNewPasswordResetCode();
                user.Password = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString());
                user.IsActive = true;
                user.UserName = user.EmailAddress;

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

                CheckErrors(await _userManager.CreateAsync(user, input.Password));

                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
                }

                CurrentUnitOfWork.SaveChanges();

                user.SetNewPasswordResetCode();
                var url = _appConfiguration["App:MvcRootAddress"] + $"account/resetpassword?Id={user.Id}&Code={user.PasswordResetCode}";
                //await _emailSender.SendAsync(
                //        to: input.EmailAddress,
                //        subject: "EasyLife Registration!",
                //        body: $"Dear <b>{input.Name},</b> <br/>Congratulations on joining our EasyLife! Make the most of your life easy here by just Signing In <b>{input.UserName}</b> as a Username for login after reset password by using the link given below.<br/><br/><b><a href='{url}'>{url}<a/></b> <br/> Live Easy with EasyLife. :) <br/> Regards, <br/> Mahavirsinh Padhiyar <br/> <b>CEO, EasyLife</b> <br/> <b>E.</b> contact@easy-life.site<br/> <b>W.</b> https://www.easy-life.site",
                //        isBodyHtml: true
                //    );

                return MapToEntityDto(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }

            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }

            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<SelectListItem>> GetUsersList()
        {
            return await Repository.GetAll().Select(u => new SelectListItem()
            {
                Text = u.FullName,
                Value = u.Id.ToString()
            }).ToListAsync();
        }
    }
}

