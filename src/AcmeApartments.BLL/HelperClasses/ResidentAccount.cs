using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.HelperClasses;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class ResidentAccount : IResidentAccount
    {
        private readonly IUserService _userService;
        private readonly IApplicationService _appService;
        private readonly IUnitOfWork _unitOfWork;

        public ResidentAccount(
            UserManager<AptUser> userManager,
            IUserService userService,
            IApplicationService appService,
            IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _appService = appService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentsViewModelDTO> GetBills(AptUser user)
        {
            var waterBill = await _unitOfWork.WaterBillRepository.Get().ToListAsync();
            var electricBill = await _unitOfWork.ElectricBillRepository.Get().ToListAsync();
            var newWaterBill = new WaterBill();
            var newElectricBill = new ElectricBill();

            if (waterBill.Count == 0)
            {
                newWaterBill = new WaterBill
                {
                    User = user,
                    Amount = 42.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                await _unitOfWork.WaterBillRepository.Insert(newWaterBill);
                await _unitOfWork.Save();
            }

            if (waterBill.Count == 0)
            {
                newElectricBill = new ElectricBill
                {
                    User = user,
                    Amount = 96.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                await _unitOfWork.ElectricBillRepository.Insert(newElectricBill);
                await _unitOfWork.Save();
            }

            return new PaymentsViewModelDTO
            {
                User = user,
                WaterBill = waterBill.Count == 0 ? newWaterBill : waterBill[0],
                ElectricBill = waterBill.Count == 0 ? newElectricBill : electricBill[0]
            };
        }

        public List<Application> GetApplications()
        {
            var userId = _userService.GetUserId();
            var apps = _appService.GetApplications(userId);
            return apps;
        }

        public async Task AddReview(ReviewViewModelDTO review)
        {
            var user = await _userService.GetUser();
            var newReview = new Review
            {
                User = user,
                DateReviewed = DateTime.Now,
                ReviewText = review.ReviewText
            };
            await _unitOfWork.ReviewRepository.Insert(newReview);
            await _unitOfWork.Save();
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
    }
}