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
            //Home controller

            CreateMap<FloorPlansViewModel, FloorPlansViewModelDTO>();
            CreateMap<FloorPlansViewModelDTO, FloorPlansViewModel>();

            CreateMap<ApplyViewModelDTO, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModelDTO>();

            CreateMap<ApplyViewModel, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModel>();

            CreateMap<AppUserContactBindingModel, AppUserContactViewModel>();
            CreateMap<AppUserContactViewModel, AppUserContactBindingModel>();

            //Home Controller Logic Class

            CreateMap<FloorPlan, FloorPlanDTO>();
            CreateMap<FloorPlanDTO, FloorPlan>();

            //Manager controller

            CreateMap<ApplicationViewModel, Application>();
            CreateMap<Application, ApplicationViewModel>();

            CreateMap<ApplicationDTO, ApplicationBindingModel>();
            CreateMap<ApplicationBindingModel, ApplicationDTO>();

            CreateMap<ApplicationViewModel, ApplicationBindingModel>();
            CreateMap<ApplicationBindingModel, ApplicationViewModel>();

            CreateMap<MaintenanceRequestEditDTO, MaintenanceRequestEditBindingModel>();
            CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditDTO>();

            CreateMap<MaintenanceRequestEditViewModel, MaintenanceRequestEditBindingModel>();
            CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditViewModel>();

            //Manager Controller Logic

            CreateMap<ApplicationDTO, Application>();
            CreateMap<Application, ApplicationDTO>();

            //Applicant controller

            CreateMap<ApplicantContactBindingModel, ApplicantContactViewModel>();
            CreateMap<ApplicantContactViewModel, ApplicantContactBindingModel>();

            //Resident controller

            CreateMap<ReviewBindingModel, ReviewViewModelDTO>();
            CreateMap<ReviewViewModelDTO, ReviewBindingModel>();

            CreateMap<ReviewBindingModel, ReviewViewModel>();
            CreateMap<ReviewViewModel, ReviewBindingModel>();

            CreateMap<ResidentContactBindingModel, ResidentContactViewModel>();
            CreateMap<ResidentContactViewModel, ResidentContactBindingModel>();

            CreateMap<NewMaintenanceRequestDTO, NewMaintenanceRequestBindingModel>();
            CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestDTO>();

            CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestViewModel>();
            CreateMap<NewMaintenanceRequestViewModel, NewMaintenanceRequestBindingModel>();

            CreateMap<MaintenanceRequest, MaintenanceRequestViewModel>();
            CreateMap<MaintenanceRequestViewModel, MaintenanceRequest>();

            CreateMap<PaymentsViewModelDTO, PaymentsViewModel>();
            CreateMap<PaymentsViewModel, PaymentsViewModelDTO>();
        }
    }
}