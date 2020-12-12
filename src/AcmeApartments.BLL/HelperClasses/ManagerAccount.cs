using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.HelperClasses;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class ManagerAccount : IManagerAccount
    {
        private readonly UserManager<AptUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationService _appService;

        public ManagerAccount(
            UserManager<AptUser> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserService userService,
            IApplicationService appService
            )
        {
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _appService = appService;
        }

        public async Task ApproveApplication(
            string userId,
            int appId,
            string ssn,
            string aptNumber,
            string aptPrice
            )
        {
            var app = _unitOfWork.ApplicationRepository.GetByID(appId);

            app.Status = ApplicationStatus.APPROVED;

            _unitOfWork.ApplicationRepository.Update(app);
            _unitOfWork.Save();

            var applicationUser = _unitOfWork.AptUserRepository.GetByID(userId);

            applicationUser.SSN = ssn;

            applicationUser.AptNumber = aptNumber;
            applicationUser.AptPrice = aptPrice;

            _unitOfWork.AptUserRepository.Update(applicationUser);
            _unitOfWork.Save();

            await _userManager.RemoveFromRoleAsync(applicationUser, "Applicant");
            await _userManager.AddToRoleAsync(applicationUser, "Resident");
        }

        public async Task UnApproveApplication(string userId, string aptNumber, int appId)
        {//todo-p: put this line in applicatin service
            var app = _unitOfWork.ApplicationRepository.GetByID(appId);

            app.Status = ApplicationStatus.UNAPPROVED;

            _unitOfWork.ApplicationRepository.Update(app);

            var applicationUser = await _userService.GetUser();

            applicationUser.SSN = null;
            applicationUser.AptNumber = null;
            applicationUser.AptPrice = null;

            await _userManager.RemoveFromRoleAsync(applicationUser, "Resident");
            await _userManager.AddToRoleAsync(applicationUser, "Applicant");

            _unitOfWork.AptUserRepository.Update(applicationUser);
        }

        public void EditApplication(ApplicationDTO application)
        {
            var applicationEntity = _mapper.Map<Application>(application);
            _unitOfWork.ApplicationRepository.Update(applicationEntity);
        }

        public Application CancelApplication(int ApplicationId)
        {
            var application = _appService.GetApplication(ApplicationId);
            application.Status = ApplicationStatus.CANCELED;

            _unitOfWork.ApplicationRepository.Update(application);
            _unitOfWork.Save();

            return application;
        }

        public MaintenanceRequest GetMaintenanceRequest(int maintenanceId)
        {
            var maintenanceRecord = _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceId);
            return maintenanceRecord;
        }

        public List<AptUser> GetMaintenanceRequestsUsers()
        {
            var users = (from userRecord in _unitOfWork.AptUserRepository.Get()
                         join mRecord in _unitOfWork.MaintenanceRequestRepository.Get() on userRecord.Id equals mRecord.AptUserId
                         select userRecord).Distinct().ToList();

            return users;
        }

        public List<MaintenanceRequest> GetMaintenanceUserRequests()
        {
            var requests = (from userRecord in _unitOfWork.AptUserRepository.Get()
                            join mRecord in _unitOfWork.MaintenanceRequestRepository.Get() on userRecord.Id equals mRecord.AptUserId
                            select mRecord).ToList();

            return requests;
        }

        public MaintenanceRequest EditMaintenanceRequest(MaintenanceRequestDTO maintenanceViewModelDTO)
        {
            var maintenanceRecord = _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceViewModelDTO.Id);

            maintenanceRecord.ProblemDescription = maintenanceViewModelDTO.ProblemDescription;
            maintenanceRecord.isAllowedToEnter = maintenanceViewModelDTO.isAllowedToEnter;

            _unitOfWork.MaintenanceRequestRepository.Update(maintenanceRecord);
            _unitOfWork.Save();

            return maintenanceRecord;
        }

        public void ApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var maintenanceRecord = _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceId);

            maintenanceRecord.AptUserId = userId;
            maintenanceRecord.Status = MaintenanceRequestStatus.APPROVED;

            _unitOfWork.MaintenanceRequestRepository.Update(maintenanceRecord);
            _unitOfWork.Save();
        }

        public void UnApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var maintenanceRecord = _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceId);
            maintenanceRecord.AptUserId = userId;
            maintenanceRecord.Status = MaintenanceRequestStatus.UNAPPROVED;

            _unitOfWork.MaintenanceRequestRepository.Update(maintenanceRecord);
            _unitOfWork.Save();
        }
    }
}