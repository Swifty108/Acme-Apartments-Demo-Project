using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AptUser> GetApplicationUser(string userId)
        {
            var applicationUser = await _dbContext.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            _dbContext.Attach(applicationUser);
            return applicationUser;
        }

        public async Task UpdateUser(AptUser user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Application> GetApplication(int appId)
        {
            var application = await _dbContext.Applications.Where(u => u.ApplicationId == appId).FirstOrDefaultAsync();
            return application;
        }

        public async Task<List<Application>> GetApplications(string userId)
        {
            return await _dbContext.Applications.Where(u => u.AptUserId == userId).ToListAsync();
        }

        public async Task UpdateApplication(Application app)
        {
            _dbContext.Applications.Update(app);
            await _dbContext.SaveChangesAsync();
        }

        public async void UpdateMaintenaceRequest(MaintenanceRequest mRequest)
        {
            _dbContext.MaintenanceRequests.Update(mRequest);
            await _dbContext.SaveChangesAsync();
        }
    }
}