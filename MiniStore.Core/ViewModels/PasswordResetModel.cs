using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class PasswordResetModel
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
    }
}
