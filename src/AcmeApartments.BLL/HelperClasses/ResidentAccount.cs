using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.HelperClasses;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Data;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class ResidentAccount : IResidentAccount
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AptUser> _userManager;
        private readonly IUserService _userService;
        private readonly IApplicationService _appService;
        private readonly IUnitOfWork _unitOfWork;

        public ResidentAccount(
            ApplicationDbContext dbContext,
            UserManager<AptUser> userManager,
            IResidentRepository residentRepository,
            IUserService userService,
            IApplicationService appService,
            IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userService = userService;
            _appService = appService;
            _unitOfWork = unitOfWork;
        }

        public PaymentsViewModelDTO GetBills(AptUser user)
        {
            var waterBill = _dbContext.WaterBills.FirstOrDefault();
            var electricBill = _dbContext.ElectricBills.FirstOrDefault();
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

                _dbContext.AddAsync(newWaterBill);
                _dbContext.SaveChangesAsync();
            }

            if (electricBill == null)
            {
                newElectricBill = new ElectricBill
                {
                    AptUser = user,
                    Amount = 96.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                _dbContext.AddAsync(newElectricBill);
                _dbContext.SaveChangesAsync();
            }

            return new PaymentsViewModelDTO
            {
                AptUser = user,
                WaterBill = waterBill ?? newWaterBill,
                ElectricBill = electricBill ?? newElectricBill
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
                AptUser = user,
                DateReviewed = DateTime.Now,
                ReviewText = review.ReviewText
            };
            _unitOfWork.ReviewRepository.Insert(newReview);
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

            _unitOfWork.MaintenanceRequestRepository.Insert(maintenanceRequest);
        }

        public List<MaintenanceRequest> GetMaintenanceRequests()
        {
            var userId = _userService.GetUserId();
            var requests = _unitOfWork.MaintenanceRequestRepository.Get(filter: u => u.AptUserId == userId).ToList();

            return requests;
        }
    }
}