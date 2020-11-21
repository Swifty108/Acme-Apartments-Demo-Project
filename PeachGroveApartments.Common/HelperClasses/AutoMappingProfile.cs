using AutoMapper;
using PeachGroveApartments.BLL.Entities;
using PeachGroveApartments.Common.ViewModels;

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