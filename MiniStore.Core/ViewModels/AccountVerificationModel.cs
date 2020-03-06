using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class AccountVerificationModel
    {

        public string Code { get; set; }
        public long UserId { get; set; }
    }
}
