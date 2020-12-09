using AcmeApartments.BLL.DTOs;
using AcmeApartments.Common.DTOs;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Models;
using AcmeApartments.Web.ViewModels;
using AutoMapper;

namespace AcmeApartments.BLL.HelperClasses
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<ApplicationViewModel, ApplicationDTO>();
            CreateMap<ApplicationDTO, ApplicationViewModel>();
            CreateMap<MaintenanceRequestDTO, MaintenanceRequestViewModel>();
            CreateMap<MaintenanceRequestViewModel, MaintenanceRequestDTO>();
            CreateMap<FloorPlansViewModel, FloorPlansViewModelDTO>();
            CreateMap<FloorPlansViewModelDTO, FloorPlansViewModel>();
            CreateMap<ApplicationViewModel, ApplicationViewModelDTO>();
            CreateMap<ApplicationViewModelDTO, ApplicationViewModel>();
            CreateMap<PaymentsViewModelDTO, PaymentsViewModel>();
            CreateMap<PaymentsViewModel, PaymentsViewModelDTO>();
            CreateMap<ReviewViewModel, ReviewViewModelDTO>();
            CreateMap<ReviewViewModelDTO, ReviewViewModel>();
            CreateMap<FloorPlanDTO, FloorPlan>();
            CreateMap<FloorPlan, FloorPlanDTO>();
        }
    }
}