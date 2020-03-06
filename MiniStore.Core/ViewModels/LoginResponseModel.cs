using MiniStore.Core.Dto;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class LoginResponseModel
    {
        public string Token { get; set; }

        public LoginDto AppUser { get; set; }

    }
}
