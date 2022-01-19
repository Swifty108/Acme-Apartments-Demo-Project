using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Models;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;

namespace AcmeApartments.Web.HelperClasses
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            //Home controller

            //CreateMap<FloorPlansViewModel, FloorPlansViewModelDTO>();
            CreateMap<FloorPlansViewModelDTO, FloorPlansViewModel>().ReverseMap();

            //CreateMap<ApplyViewModelDTO, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModelDTO>().ReverseMap();

            //CreateMap<ApplyViewModel, ApplyBindingModel>();
            CreateMap<ApplyBindingModel, ApplyViewModel>().ReverseMap();

            CreateMap<AppUserContactBindingModel, AppUserContactViewModel>().ReverseMap();
            //CreateMap<AppUserContactViewModel, AppUserContactBindingModel>();

            //Home Controller Logic Class

            CreateMap<FloorPlan, FloorPlanDTO>().ReverseMap();
            //CreateMap<FloorPlanDTO, FloorPlan>();

            //Manager controller

            //CreateMap<ApplicationViewModel, Application>();
            CreateMap<Application, ApplicationViewModel>().ReverseMap();

            CreateMap<ApplicationDTO, ApplicationBindingModel>().ReverseMap();
            //CreateMap<ApplicationBindingModel, ApplicationDTO>();

            //CreateMap<ApplicationViewModel, ApplicationBindingModel>();
            CreateMap<ApplicationBindingModel, ApplicationViewModel>().ReverseMap();

            //CreateMap<MaintenanceRequestViewModel, MaintenanceRequest>();
            CreateMap<MaintenanceRequest, MaintenanceRequestViewModel>().ReverseMap();

            CreateMap<MaintenanceRequestEditDTO, MaintenanceRequestEditBindingModel>().ReverseMap();
            //CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditDTO>();

            //CreateMap<MaintenanceRequestEditViewModel, MaintenanceRequestEditBindingModel>();
            CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditViewModel>().ReverseMap();

            //Manager Controller Logic

            //CreateMap<ApplicationDTO, Application>();
            CreateMap<Application, ApplicationDTO>().ReverseMap();

            //Applicant controller

            CreateMap<ApplicantContactBindingModel, ApplicantContactViewModel>().ReverseMap();
            //CreateMap<ApplicantContactViewModel, ApplicantContactBindingModel>();

            //Resident controller

            CreateMap<ReviewBindingModel, ReviewViewModelDTO>().ReverseMap();
            //CreateMap<ReviewViewModelDTO, ReviewBindingModel>();

            CreateMap<ReviewBindingModel, ReviewViewModel>().ReverseMap();
            //CreateMap<ReviewViewModel, ReviewBindingModel>();

            CreateMap<ResidentContactBindingModel, ResidentContactViewModel>().ReverseMap();
            //CreateMap<ResidentContactViewModel, ResidentContactBindingModel>();

            CreateMap<NewMaintenanceRequestDTO, NewMaintenanceRequestBindingModel>().ReverseMap();
            //CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestDTO>();

            CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestViewModel>().ReverseMap();
            //CreateMap<NewMaintenanceRequestViewModel, NewMaintenanceRequestBindingModel>();

            CreateMap<PaymentsViewModelDTO, PaymentsViewModel>().ReverseMap();
            //CreateMap<PaymentsViewModel, PaymentsViewModelDTO>();
        }
    }
}