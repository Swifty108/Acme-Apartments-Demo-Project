using PeachGroveApartments.Infrastructure.DTOs;
using PeachGroveApartments.Infrastructure.Models;

namespace AcmeApartments.BLL.HelperClasses
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
            CreateMap<FloorPlansViewModelDTO, FloorPlansViewModel>();
            CreateMap<ApplicationViewModel, ApplicationViewModelDTO>();
            CreateMap<ApplicationViewModelDTO, ApplicationViewModel>();
        }
    }
}