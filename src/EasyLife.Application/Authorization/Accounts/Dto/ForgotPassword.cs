namespace EasyLife.Authorization.Accounts.Dto
{
    public class ForgotPassword
    {
        public string EmailAddress { get; set; }
        public bool SentSuccessfully { get; set; }
    }
}
