using PeachGroveApartments.ApplicationLayer.Interfaces;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PeachGroveApartments.ApplicationLayer.HelperClasses
{
    public class ResidentAccount : IResidentAccount
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AptUser> _userManager;
        private readonly IResidentRepository _residentRepository;

        public ResidentAccount(
            ApplicationDbContext dbContext,
            UserManager<AptUser> userManager,
            IResidentRepository residentRepository
            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _residentRepository = residentRepository;
        }

        public async Task<PaymentsViewModel> GetBills(AptUser user)
        {
            var waterBill = await _dbContext.WaterBills.FirstOrDefaultAsync();
            var electricBill = await _dbContext.ElectricBills.FirstOrDefaultAsync();
            var newWaterBill = new WaterBill();
            var newElectricBill = new ElectricBill();

            if (waterBill == null)
            {
                newWaterBill = new WaterBill { AptUser = user, Amount = 42.53M, DateDue = DateTime.Now.AddDays(20) };
                await _dbContext.AddAsync(newWaterBill);
                await _dbContext.SaveChangesAsync();
            }

            if (electricBill == null)
            {
                newElectricBill = new ElectricBill { AptUser = user, Amount = 96.53M, DateDue = DateTime.Now.AddDays(20) };
                await _dbContext.AddAsync(newElectricBill);
                await _dbContext.SaveChangesAsync();
            }

            var app = await _dbContext.Applications.Where(u => u.AptUserId == user.Id).FirstOrDefaultAsync();

            return new PaymentsViewModel
            {
                AptUser = user,
                WaterBill = waterBill ?? newWaterBill,
                ElectricBill = electricBill ?? newElectricBill
            };
        }

        public async Task AddReview(Review review)
        {
            await _dbContext.AddAsync(review);
            await _dbContext.SaveChangesAsync();
        }
    }
}