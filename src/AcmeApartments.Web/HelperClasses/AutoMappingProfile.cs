using AcmeApartments.Providers.DTOs;
using AcmeApartments.Data.Provider.Entities;
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

            CreateMap<FloorPlansViewModelDto, FloorPlansViewModel>().ReverseMap();

            CreateMap<ApplyBindingModel, ApplyModelDto>().ReverseMap();

            CreateMap<ApplyBindingModel, ApplyViewModel>().ReverseMap();

            CreateMap<AppUserContactBindingModel, AppUserContactViewModel>().ReverseMap();

            //Home Controller Logic Class

            CreateMap<FloorPlan, FloorPlanDto>().ReverseMap();

            //Manager controller

            CreateMap<Application, ApplicationViewModel>().ReverseMap();

            CreateMap<ApplicationDto, ApplicationBindingModel>().ReverseMap();

            CreateMap<ApplicationBindingModel, ApplicationViewModel>().ReverseMap();

            CreateMap<MaintenanceRequest, MaintenanceRequestViewModel>().ReverseMap();

            CreateMap<MaintenanceRequestEditDto, MaintenanceRequestEditBindingModel>().ReverseMap();

            CreateMap<MaintenanceRequestEditBindingModel, MaintenanceRequestEditViewModel>().ReverseMap();

            //Manager Controller Logic

            CreateMap<Application, ApplicationDto>().ReverseMap();

            //Applicant controller

            CreateMap<ApplicantContactBindingModel, ApplicantContactViewModel>().ReverseMap();

            //Resident controller

            CreateMap<ReviewBindingModel, ReviewViewModelDto>().ReverseMap();

            CreateMap<ReviewBindingModel, ReviewViewModel>().ReverseMap();

            CreateMap<ResidentContactBindingModel, ResidentContactViewModel>().ReverseMap();

            CreateMap<NewMaintenanceRequestDto, NewMaintenanceRequestBindingModel>().ReverseMap();

            CreateMap<NewMaintenanceRequestBindingModel, NewMaintenanceRequestViewModel>().ReverseMap();

            CreateMap<PaymentsViewModelDto, PaymentsViewModel>().ReverseMap();
        }
    }
}