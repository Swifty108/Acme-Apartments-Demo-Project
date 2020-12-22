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

            //keep
            CreateMap<FloorPlansViewModel, FloorPlansViewModelDTO>();
            CreateMap<FloorPlansViewModelDTO, FloorPlansViewModel>();

            //keep
            CreateMap<ApplyViewModelDTO, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModelDTO>();

            //keep
            CreateMap<ApplyViewModel, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModel>();
            //keep
            CreateMap<AppUserContactBindingModel, AppUserContactViewModel>();
            CreateMap<AppUserContactViewModel, AppUserContactBindingModel>();

            //Home Controller Logic Class

            //keep
            CreateMap<FloorPlan, FloorPlanDTO>();
            CreateMap<FloorPlanDTO, FloorPlan>();

            ////////
            //CreateMap<ApplicationViewModel, ApplyViewModelDTO>();
            //CreateMap<ApplyViewModelDTO, ApplicationViewModel>();

            //CreateMap<ApplicationViewModel, ApplicationDTO>();
            //CreateMap<ApplicationDTO, ApplicationViewModel>();

            //CreateMap<ApplicationViewModel, ApplicationViewModelDTO>();
            //CreateMap<ApplicationViewModelDTO, ApplicationViewModel>();

            //CreateMap<ReviewViewModel, ReviewViewModelDTO>();
            //CreateMap<ReviewViewModelDTO, ReviewViewModel>();

            ////////////////////

            //Manager controller

            //todo-p: erase the keep comments

            //keep
            CreateMap<ApplicationViewModel, Application>();
            CreateMap<Application, ApplicationViewModel>();

            //keep
            CreateMap<ApplicationDTO, ApplicationBindingModel>();
            CreateMap<ApplicationBindingModel, ApplicationDTO>();

            //keep
            CreateMap<ApplicationViewModel, ApplicationBindingModel>();
            CreateMap<ApplicationBindingModel, ApplicationViewModel>();

            //keep
            CreateMap<MaintenanceRequestEditDTO, MaintenanceRequestEditBindingModel>();
            CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditDTO>();

            //keep
            CreateMap<MaintenanceRequestEditViewModel, MaintenanceRequestEditBindingModel>();
            CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditViewModel>();

            //Manager Controller Logic

            //keep
            CreateMap<ApplicationDTO, Application>();
            CreateMap<Application, ApplicationDTO>();

            //Applicant controller

            //keep
            CreateMap<ApplicantContactBindingModel, ApplicantContactViewModel>();
            CreateMap<ApplicantContactViewModel, ApplicantContactBindingModel>();

            //Resident controller

            //keep all
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

            CreateMap<PaymentsViewModelDTO, PaymentsViewModel>();
            CreateMap<PaymentsViewModel, PaymentsViewModelDTO>();
        }
    }
}