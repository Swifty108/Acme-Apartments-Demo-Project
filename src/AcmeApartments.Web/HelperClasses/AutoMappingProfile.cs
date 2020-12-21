using AcmeApartments.BLL.DTOs;
using AcmeApartments.Common.DTOs;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Models;
using AcmeApartments.Web.BindingModels;
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

            CreateMap<ApplicationDTO, Application>();
            CreateMap<Application, ApplicationDTO>();

            CreateMap<NewMaintenanceRequestDTO, NewMaintenanceRequestBindingModel>();
            CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestDTO>();

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

            CreateMap<ApplyViewModelDTO, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModelDTO>();

            CreateMap<ApplyViewModel, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModel>();

            CreateMap<ApplicantContactBindingModel, ApplicantContactViewModel>();
            CreateMap<ApplicantContactViewModel, ApplicantContactBindingModel>();

            CreateMap<ResidentContactBindingModel, ResidentContactViewModel>();
            CreateMap<ResidentContactViewModel, ResidentContactBindingModel>();

            CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestViewModel>();
            CreateMap<NewMaintenanceRequestViewModel, NewMaintenanceRequestBindingModel>();

            CreateMap<ReviewBindingModel, ReviewViewModelDTO>();
            CreateMap<ReviewViewModelDTO, ReviewBindingModel>();

            CreateMap<ReviewBindingModel, ReviewViewModel>();
            CreateMap<ReviewViewModel, ReviewBindingModel>();

            CreateMap<AppUserContactBindingModel, AppUserContactViewModel>();
            CreateMap<AppUserContactViewModel, AppUserContactBindingModel>();
        }
    }
}