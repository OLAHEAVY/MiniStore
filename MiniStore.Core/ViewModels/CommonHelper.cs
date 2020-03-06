using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniStore.Core.Interface;
using MiniStore.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class CommonHelper : ICommonHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEnvironmentalVariables _environmentalVariables;

        public CommonHelper(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IServiceScopeFactory serviceScopeFactory, IEnvironmentalVariables environmentalVariables)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _serviceScopeFactory = serviceScopeFactory;
            _environmentalVariables = environmentalVariables;
        }

        public long? CurrentUserId()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            long.TryParse(currentUser, out var userId);
            return userId;
        }

        public string CurrentUsername()
        {
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Acr);
            return currentUser;
        }

        public ResponseMessage OutputMessage(bool isSuccessful, string message) => new ResponseMessage { IsSuccessful = isSuccessful, Message = message };

    }
}
