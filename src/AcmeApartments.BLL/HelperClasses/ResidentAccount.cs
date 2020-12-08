using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.HelperClasses;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Data;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IResidentRepository _residentRepository;
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;

        public ResidentAccount(
            ApplicationDbContext dbContext,
            UserManager<AptUser> userManager,
            IResidentRepository residentRepository,
            IUserService userService,
            IApplicationService applicationService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _residentRepository = residentRepository;
            _userService = userService;
            _applicationService = applicationService;
        }

        public async Task<PaymentsViewModelDTO> GetBills(AptUser user)
        {
            var waterBill = await _dbContext.WaterBills.FirstOrDefaultAsync();
            var electricBill = await _dbContext.ElectricBills.FirstOrDefaultAsync();
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

                await _dbContext.AddAsync(newWaterBill);
                await _dbContext.SaveChangesAsync();
            }

            if (electricBill == null)
            {
                newElectricBill = new ElectricBill
                {
                    AptUser = user,
                    Amount = 96.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                await _dbContext.AddAsync(newElectricBill);
                await _dbContext.SaveChangesAsync();
            }

            var app = await _dbContext.Applications.Where(u => u.AptUserId == user.Id).FirstOrDefaultAsync();

            return new PaymentsViewModelDTO
            {
                AptUser = user,
                WaterBill = waterBill ?? newWaterBill,
                ElectricBill = electricBill ?? newElectricBill
            };
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
            await _dbContext.AddAsync(newReview);
            await _dbContext.SaveChangesAsync();
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

            await _residentRepository.AddMaintenanceRequest(maintenanceRequest);
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceRequests()
        {
            return await _residentRepository.GetMaintenanceUserRequests();
        }
    }
}