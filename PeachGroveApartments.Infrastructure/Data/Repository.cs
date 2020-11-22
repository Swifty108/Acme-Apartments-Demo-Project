using Microsoft.EntityFrameworkCore;
using PeachGroveApartments.Infrastructure.DTOs;
using PeachGroveApartments.Infrastructure.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Data
{
    public class Repository : IRepository
    {
        protected readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationViewModelDTO> GetApplications(string userId)
        {
            var applications = await _dbContext.Applications.Where(u => u.AptUserId == userId).ToListAsync();
            var user = await _dbContext.Users.FindAsync(userId);

            return new ApplicationViewModelDTO
            {
                Applications = applications,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<FloorPlansViewModelDTO> GetFloorPlans()
        {
            var studioPlans = await _dbContext.FloorPlans.Where(f => f.FloorPlanType == "Studio").ToListAsync();
            var oneBedPlans = await _dbContext.FloorPlans.Where(f => f.FloorPlanType == "1Bed").ToListAsync();
            var twoBedPlans = await _dbContext.FloorPlans.Where(f => f.FloorPlanType == "2Bed").ToListAsync();

            return new FloorPlansViewModelDTO
            {
                StudioPlans = studioPlans,
                OneBedPlans = oneBedPlans,
                TwoBedPlans = twoBedPlans
            };
        }
    }
}