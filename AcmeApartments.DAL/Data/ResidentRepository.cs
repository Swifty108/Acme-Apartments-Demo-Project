using Microsoft.EntityFrameworkCore;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Data
{
    public class ResidentRepository : IResidentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ResidentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Application> GetApplication(int applicationId)
        {
            return await _dbContext.Applications.FindAsync(applicationId);
        }

        public async Task<List<Application>> GetApplications(string userId)
        {
            return await _dbContext.Applications.Where(u => u.AptUserId == userId).ToListAsync();
        }

        public async Task AddMaintenanceRequest(MaintenanceRequest maintenanceRequest)
        {
            await _dbContext.MaintenanceRequests.AddAsync(maintenanceRequest);
            await _dbContext.SaveChangesAsync();
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

        public Task EditApplication(Application application)
        {
            throw new System.NotImplementedException();
        }
    }
}