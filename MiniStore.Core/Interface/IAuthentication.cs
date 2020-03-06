using MiniStore.Core.ViewModels;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Core.Interface
{
    public interface IAuthentication
    {
        Task<UserAccountModel> RegisterCustomer(RegisterModel model);

        Task<UserResponseModel> CheckValidUser(LoginModel model);

        Task<string> GenerateAccessToken(ApplicationUser user);
    }
}
