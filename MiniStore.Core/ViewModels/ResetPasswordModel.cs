using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }

        public ApplicationUser User { get; set; }
    }
}
