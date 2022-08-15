using System.ComponentModel.DataAnnotations;

namespace EasyLife.Web.Models.Account
{
    public class ResetPasswordViewModel
    {
        public long UserId { get; set; }
        public string ResetCode { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
        public bool PasswordChangedSuccessfully { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}
