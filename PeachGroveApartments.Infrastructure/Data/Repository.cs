using Microsoft.EntityFrameworkCore;
using PeachGroveApartments.Infrastructure.DTOs;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Application> GetApplication(int applicationId)
        {
            return await _dbContext.Applications.FindAsync(applicationId);
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

        public async Task<List<AptUser>> GetApplicationUsers()
        {
            return await (from userRecord in _dbContext.Users
                          join applicationRecord in _dbContext.Applications on userRecord.Id equals applicationRecord.AptUserId
                          select userRecord).Distinct().ToListAsync();
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