using Microsoft.Extensions.DependencyInjection;
using MiniStore.Core.Helpers;
using MiniStore.Core.Interface;
using MiniStore.Core.Security;
using MiniStore.Core.Services;
using MiniStore.Core.ViewModels;
using MiniStore.Data.Repository;
using MiniStore.Data.Repository.IRepository;
using MiniStore.Utility.Logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core
{
    public static class ServiceConnector
    {

        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILog, LogService>();
            services.AddScoped<IEnvironmentalVariables, EnvironmentalVariables>();
            services.AddScoped<IAuthentication, AuthenticationService>();
            services.AddSingleton<EncryptionService>();
            services.AddSingleton<AesEncryption>();
            services.AddScoped<ICommonHelper, CommonHelper>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ILocalGovernmentService, LocalGovernmentService>();
      
        }
    }
}
