using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLife.Web.Models.Account
{
    public class ForgotPasswordViewModel
    {
        public string EmailAddress { get; set; }
        public bool SentSuccessfully { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}
