using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.BLL.HelperClasses;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public MaintenanceService(
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId)
        {
            var maintenanceRecord = await _unitOfWork.MaintenanceRequestRepository.Get(filter: maintenanceRecord => maintenanceRecord.Id == maintenanceId).FirstOrDefaultAsync();
            return maintenanceRecord;
        }

        public async Task SubmitMaintenanceRequest(NewMaintenanceRequestDTO newMaintenanceRequestDTO)
        {
            var user = await _userService.GetUser();
            var maintenanceRequest = new MaintenanceRequest
            {
                User = user,
                DateRequested = DateTime.Now,
                isAllowedToEnter = newMaintenanceRequestDTO.isAllowedToEnter,
                ProblemDescription = newMaintenanceRequestDTO.ProblemDescription,
                Status = MaintenanceRequestStatus.PENDINGAPPROVAL
            };

            await _unitOfWork.MaintenanceRequestRepository.Insert(maintenanceRequest);
            await _unitOfWork.Save();
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceRequests()
        {
            var userId = _userService.GetUserId();
            var requests = await _unitOfWork.MaintenanceRequestRepository.Get(filter: u => u.AptUserId == userId).ToListAsync();

            return requests;
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
                                  join mRecord in _unitOfWork.MaintenanceRequestRepository.Get(includeProperties: "User") on userRecord.Id equals mRecord.AptUserId
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
