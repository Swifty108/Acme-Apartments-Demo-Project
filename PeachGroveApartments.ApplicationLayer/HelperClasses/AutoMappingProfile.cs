using AutoMapper;
using PeachGroveApartments.ApplicationLayer.ViewModels;
using PeachGroveApartments.Infrastructure.DTOs;
using PeachGroveApartments.Infrastructure.Models;

namespace PeachGroveApartments.ApplicationLayer.HelperClasses
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<ApplicationViewModel, Application>();
            CreateMap<Application, ApplicationViewModel>();
            CreateMap<MaintenanceRequest, MaintenanceRequestViewModel>();
            CreateMap<MaintenanceRequestViewModel, MaintenanceRequest>();
            CreateMap<FloorPlansViewModel, FloorPlansViewModelDTO>();
            CreateMap<ApplicationViewModel, ApplicationViewModelDTO>();
        }
    }
}