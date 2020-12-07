using Microsoft.EntityFrameworkCore;
using PeachGroveApartments.Infrastructure.DTOs;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Data
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ManagerRepository(ApplicationDbContext dbContext)
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

        public async Task EditApplication(Application application)
        {
            _dbContext.Update(application);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<AptUser>> GetMaintenanceRequestsUsers()
        {
            var users = (from userRecord in _dbContext.Users
                         join mRecord in _dbContext.MaintenanceRequests on userRecord.Id equals mRecord.AptUserId
                         select userRecord).Distinct();

            return await users.ToListAsync();
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceUserRequests()
        {
            return await (from userRecord in _dbContext.Users
                          join mRecord in _dbContext.MaintenanceRequests on userRecord.Id equals mRecord.AptUserId
                          select mRecord).ToListAsync();
        }

        public async Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId)
        {
            return await _dbContext.MaintenanceRequests.FindAsync(maintenanceId);
        }
    }
}