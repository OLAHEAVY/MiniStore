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

        Task<UserAccountModel> ConfirmAccount(string token);

        Task<bool> SendVerificationLink(ApplicationUser user);

        Task<bool> SendResetPasswordLink(ApplicationUser user);

        Task<ResetPasswordModel> ResetPasword(string token);

        Task<ResponseMessage> PerformPasswordChange(PasswordResetModel model);
    }
}
