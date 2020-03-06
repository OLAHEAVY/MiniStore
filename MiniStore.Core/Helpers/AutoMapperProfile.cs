using AutoMapper;
using MiniStore.Core.ViewModels;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, ApplicationUser>();

        }
        
    }
}
