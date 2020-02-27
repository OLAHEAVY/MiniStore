using Microsoft.Extensions.DependencyInjection;
using MiniStore.Data.Repository;
using MiniStore.Data.Repository.IRepository;
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
        }
    }
}
