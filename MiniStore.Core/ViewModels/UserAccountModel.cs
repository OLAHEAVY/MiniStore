using Microsoft.AspNetCore.Identity;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class UserAccountModel
    {
        public IdentityResult IdentityResult { get; set; }
        public ApplicationUser User { get; set; }

    }
}
