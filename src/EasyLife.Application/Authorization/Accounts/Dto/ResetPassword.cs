namespace EasyLife.Authorization.Accounts.Dto
{
    public class ResetPassword
    {
        public long UserId { get; set; }
        public string ResetCode { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public bool PasswordChangedSuccessfully { get; set; }
        public string ErrorMessage { get; set; }
    }
}
