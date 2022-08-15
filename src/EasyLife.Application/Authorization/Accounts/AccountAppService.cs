using Abp.Configuration;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.Extensions.Configuration;
using EasyLife.Authorization.Accounts.Dto;
using EasyLife.Authorization.Users;
using EasyLife.Configuration;
using EasyLife.Url;
using System;
using System.Threading.Tasks;

namespace EasyLife.Authorization.Accounts
{
    public class AccountAppService : EasyLifeAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmtpEmailSender _smtpEmailSender;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IConfigurationRoot _appConfiguration;
        public AccountAppService(
            UserRegistrationManager userRegistrationManager,
            IEmailSender emailSender,
            ISmtpEmailSender smtpEmailSender,
            IAppConfigurationAccessor appConfiguration)
        {
            _userRegistrationManager = userRegistrationManager;
            _emailSender = emailSender;
            _smtpEmailSender = smtpEmailSender;
            _appConfiguration = appConfiguration.Configuration;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true, // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
                isEmailConfirmationRequiredForLogin
            );

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        public async Task<ForgotPassword> SendPasswordResetCode(ForgotPassword forgotPassword)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(forgotPassword.EmailAddress);

                if (user == null)
                {
                    throw new UserFriendlyException("User not found!");
                }

                user.SetNewPasswordResetCode();
                var url = _appConfiguration["App:MvcRootAddress"] + $"account/resetpassword?Id={user.Id}&Code={user.PasswordResetCode}";

                await _emailSender.SendAsync(
                    //from: "padhiyarmahavirsinh@gmail.com",
                    to: forgotPassword.EmailAddress,
                    subject: "EasyLife - Forgot Password!",
                    body: $"Click here to reset your password: <b>{url}</b>",
                    isBodyHtml: true
                );
                forgotPassword.SentSuccessfully = true;
            }
            catch (Exception ex)
            {
                forgotPassword.SentSuccessfully = false;
            }
            return forgotPassword;
        }

        public async Task<ResetPassword> ResetPasswordAsync(ResetPassword resetPassword)
        {
            try
            {
                var user = await UserManager.GetUserByIdAsync(resetPassword.UserId);
                if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != resetPassword.ResetCode)
                {
                    throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
                }

                CheckErrors(await UserManager.ChangePasswordAsync(user, resetPassword.NewPassword));

                user.PasswordResetCode = null;
                user.IsEmailConfirmed = true;
                user.LockoutEndDateUtc = null;
                user.ShouldChangePasswordOnNextLogin = false;

                CheckErrors(await UserManager.UpdateAsync(user));
                resetPassword.PasswordChangedSuccessfully = true;

            }
            catch (Exception ex)
            {
                resetPassword.PasswordChangedSuccessfully = false;
                resetPassword.ErrorMessage = ex.Message;
                return resetPassword;
            }

            return resetPassword;
        }
    }
}
