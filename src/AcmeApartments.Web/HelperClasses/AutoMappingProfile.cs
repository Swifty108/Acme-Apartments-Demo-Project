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
            CreateMap<ApplicationsViewModel, ApplyViewModelDTO>();
            CreateMap<ApplyViewModelDTO, ApplicationsViewModel>();
            CreateMap<ApplicationsViewModel, ApplicationDTO>();
            CreateMap<ApplicationDTO, ApplicationsViewModel>();
            CreateMap<ApplicationsViewModel, Application>();
            CreateMap<Application, ApplicationsViewModel>();
            CreateMap<MaintenanceRequestDTO, MaintenanceRequestViewModel>();
            CreateMap<MaintenanceRequestViewModel, MaintenanceRequestDTO>();
            CreateMap<FloorPlansViewModel, FloorPlansViewModelDTO>();
            CreateMap<FloorPlansViewModelDTO, FloorPlansViewModel>();
            CreateMap<ApplicationsViewModel, ApplicationViewModelDTO>();
            CreateMap<ApplicationViewModelDTO, ApplicationsViewModel>();
            CreateMap<PaymentsViewModelDTO, PaymentsViewModel>();
            CreateMap<PaymentsViewModel, PaymentsViewModelDTO>();
            CreateMap<ReviewViewModel, ReviewViewModelDTO>();
            CreateMap<ReviewViewModelDTO, ReviewViewModel>();
            CreateMap<FloorPlanDTO, FloorPlan>();
            CreateMap<FloorPlan, FloorPlanDTO>();
            CreateMap<ApplyViewModelDTO, ApplyViewModel>();
            CreateMap<ApplyViewModel, ApplyViewModelDTO>();
        }
    }
}