using AutoMapper;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
using PeachGroveApartments.Core.Models;

namespace PeachGroveApartments.BLL.HelperClasses
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<ApplicationViewModel, Application>();
            CreateMap<Application, ApplicationViewModel>();
            CreateMap<MaintenanceRequest, MaintenanceRequestViewModel>();
            CreateMap<MaintenanceRequestViewModel, MaintenanceRequest>();
        }
    }
}