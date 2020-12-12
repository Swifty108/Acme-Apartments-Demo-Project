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
        private readonly UserManager<AptUser> _userManager;
        private readonly IUserService _userService;
        private readonly IApplicationService _appService;
        private readonly IUnitOfWork _unitOfWork;

        public ResidentAccount(
            UserManager<AptUser> userManager,
            IUserService userService,
            IApplicationService appService,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userService = userService;
            _appService = appService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentsViewModelDTO> GetBills(AptUser user)
        {
            var waterBill = await _unitOfWork.WaterBillRepository.Get().FirstAsync();
            var electricBill = await _unitOfWork.ElectricBillRepository.Get().FirstAsync();
            var newWaterBill = new WaterBill();
            var newElectricBill = new ElectricBill();

            if (waterBill == null)
            {
                newWaterBill = new WaterBill
                {
                    AptUser = user,
                    Amount = 42.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                await _unitOfWork.WaterBillRepository.Insert(newWaterBill);
                await _unitOfWork.Save();
            }

            if (electricBill == null)
            {
                newElectricBill = new ElectricBill
                {
                    AptUser = user,
                    Amount = 96.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                await _unitOfWork.ElectricBillRepository.Insert(newElectricBill);
                await _unitOfWork.Save();
            }

            return new PaymentsViewModelDTO
            {
                AptUser = user,
                WaterBill = waterBill ?? newWaterBill,
                ElectricBill = electricBill ?? newElectricBill
            };
        }

        public async Task<List<Application>> GetApplications()
        {
            var userId = _userService.GetUserId();
            var apps = await _appService.GetApplications(userId);
            return apps;
        }

        public async Task AddReview(ReviewViewModelDTO review)
        {
            var user = await _userService.GetUser();
            var newReview = new Review
            {
                AptUser = user,
                DateReviewed = DateTime.Now,
                ReviewText = review.ReviewText
            };
            await _unitOfWork.ReviewRepository.Insert(newReview);
            await _unitOfWork.Save();
        }

        public async Task SubmitMaintenanceRequest(MaintenanceRequestDTO maintenanceRequestDTO)
        {
            var user = await _userService.GetUser();
            var maintenanceRequest = new MaintenanceRequest
            {
                AptUser = user,
                DateRequested = DateTime.Now,
                isAllowedToEnter = maintenanceRequestDTO.isAllowedToEnter,
                ProblemDescription = maintenanceRequestDTO.ProblemDescription,
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