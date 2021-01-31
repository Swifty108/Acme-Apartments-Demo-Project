using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.HelperClasses;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var app = await _unitOfWork.ApplicationRepository.GetByID(appId);

            app.Status = ApplicationStatus.APPROVED;

            _unitOfWork.ApplicationRepository.Update(app);
            await _unitOfWork.Save();

            var applicationUser = await _unitOfWork.AptUserRepository.GetByID(userId);

            applicationUser.SSN = ssn;

            applicationUser.AptNumber = aptNumber;
            applicationUser.AptPrice = aptPrice;

            await _userManager.RemoveFromRoleAsync(applicationUser, "Resident");
            await _userManager.AddToRoleAsync(applicationUser, "Applicant");
            await _unitOfWork.Save();

            _unitOfWork.AptUserRepository.Update(applicationUser);
            await _unitOfWork.Save();
        }

        public async Task UnApproveApplication(string userId, string aptNumber, int appId)
        {
            var app = await _unitOfWork.ApplicationRepository.GetByID(appId);

            app.Status = ApplicationStatus.UNAPPROVED;

            _unitOfWork.ApplicationRepository.Update(app);
            await _unitOfWork.Save();

            var applicationUser = await _unitOfWork.AptUserRepository.GetByID(userId);

            applicationUser.SSN = null;
            applicationUser.AptNumber = null;
            applicationUser.AptPrice = null;

            await _userManager.RemoveFromRoleAsync(applicationUser, "Resident");
            await _userManager.AddToRoleAsync(applicationUser, "Applicant");
            await _unitOfWork.Save();

            _unitOfWork.AptUserRepository.Update(applicationUser);
            await _unitOfWork.Save();
        }

        public async Task EditApplication(ApplicationDTO applicationDTO)
        {
            var applicationEntity = _mapper.Map<Application>(applicationDTO);
            _unitOfWork.ApplicationRepository.Update(applicationEntity);
            await _unitOfWork.Save();
        }

        public async Task<Application> CancelApplication(int ApplicationId)
        {
            var application = await _appService.GetApplication(ApplicationId);
            application.Status = ApplicationStatus.CANCELED;

            _unitOfWork.ApplicationRepository.Update(application);
            await _unitOfWork.Save();

            return application;
        }

        public async Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId)
        {
            var maintenanceRecord = await _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceId);
            return maintenanceRecord;
        }

        public async Task<List<AptUser>> GetMaintenanceRequestsUsers()
        {
            var users = await (from userRecord in _unitOfWork.AptUserRepository.Get()
                               join mRecord in _unitOfWork.MaintenanceRequestRepository.Get() on userRecord.Id equals mRecord.AptUserId
                               select userRecord).Distinct().ToListAsync();

            return users;
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceUserRequests(string aptUserId)
        {
            var requests = await (from userRecord in _unitOfWork.AptUserRepository.Get()
                                  join mRecord in _unitOfWork.MaintenanceRequestRepository.Get() on userRecord.Id equals mRecord.AptUserId
                                  select mRecord).Where(s => s.AptUserId == aptUserId).ToListAsync();

            return requests;
        }

        public async Task<MaintenanceRequest> EditMaintenanceRequest(MaintenanceRequestEditDTO maintenanceRequestEditDTO)
        {
            var maintenanceRecord = await _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceRequestEditDTO.Id);

            maintenanceRecord.ProblemDescription = maintenanceRequestEditDTO.ProblemDescription;
            maintenanceRecord.isAllowedToEnter = maintenanceRequestEditDTO.isAllowedToEnter;

            _unitOfWork.MaintenanceRequestRepository.Update(maintenanceRecord);
            await _unitOfWork.Save();

            return maintenanceRecord;
        }

        public async Task ApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var maintenanceRecord = await _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceId);

            maintenanceRecord.AptUserId = userId;
            maintenanceRecord.Status = MaintenanceRequestStatus.APPROVED;

            _unitOfWork.MaintenanceRequestRepository.Update(maintenanceRecord);
            await _unitOfWork.Save();
        }

        public async Task UnApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var maintenanceRecord = await _unitOfWork.MaintenanceRequestRepository.GetByID(maintenanceId);
            maintenanceRecord.AptUserId = userId;
            maintenanceRecord.Status = MaintenanceRequestStatus.UNAPPROVED;

            _unitOfWork.MaintenanceRequestRepository.Update(maintenanceRecord);
            await _unitOfWork.Save();
        }
    }
}