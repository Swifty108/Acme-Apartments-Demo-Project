using AutoMapper;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.HelperClasses
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<ApplicationViewModel, Application>();
            CreateMap<Application, ApplicationViewModel>();
        }
    }
}
