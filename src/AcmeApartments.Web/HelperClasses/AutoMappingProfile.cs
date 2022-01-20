using AcmeApartments.BLL.DTOs;
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

            CreateMap<FloorPlansViewModelDTO, FloorPlansViewModel>().ReverseMap();

            CreateMap<ApplyBindingModel, ApplyModelDTO>().ReverseMap();

            CreateMap<ApplyBindingModel, ApplyViewModel>().ReverseMap();

            CreateMap<AppUserContactBindingModel, AppUserContactViewModel>().ReverseMap();

            //Home Controller Logic Class

            CreateMap<FloorPlan, FloorPlanDTO>().ReverseMap();

            //Manager controller

            CreateMap<Application, ApplicationViewModel>().ReverseMap();

            CreateMap<ApplicationDTO, ApplicationBindingModel>().ReverseMap();

            CreateMap<ApplicationBindingModel, ApplicationViewModel>().ReverseMap();

            CreateMap<MaintenanceRequest, MaintenanceRequestViewModel>().ReverseMap();

            CreateMap<MaintenanceRequestEditDTO, MaintenanceRequestEditBindingModel>().ReverseMap();

            CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditViewModel>().ReverseMap();

            //Manager Controller Logic

            CreateMap<Application, ApplicationDTO>().ReverseMap();

            //Applicant controller

            CreateMap<ApplicantContactBindingModel, ApplicantContactViewModel>().ReverseMap();

            //Resident controller

            CreateMap<ReviewBindingModel, ReviewViewModelDTO>().ReverseMap();

            CreateMap<ReviewBindingModel, ReviewViewModel>().ReverseMap();

            CreateMap<ResidentContactBindingModel, ResidentContactViewModel>().ReverseMap();

            CreateMap<NewMaintenanceRequestDTO, NewMaintenanceRequestBindingModel>().ReverseMap();

            CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestViewModel>().ReverseMap();

            CreateMap<PaymentsViewModelDTO, PaymentsViewModel>().ReverseMap();
        }
    }
}