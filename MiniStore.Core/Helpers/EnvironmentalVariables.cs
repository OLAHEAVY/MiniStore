using Microsoft.Extensions.Configuration;
using MiniStore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.Helpers
{
    public class EnvironmentalVariables : IEnvironmentalVariables
    {
        private readonly IConfiguration _configuration;

        public EnvironmentalVariables(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string ClientUrl => $"{_configuration.GetValue<string>("clientUrl:url")}";


        public string ConnectionString => _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

        public string SendGridKey =>  _configuration.GetValue<string>("appSettings:sendGridKey");
    }
}
