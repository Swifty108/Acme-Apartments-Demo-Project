using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Data
{
    internal class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AptUser GetAptUser(string userId, string aptNumber = null)
        {
            var applicationUser = _dbContext.Users.Where(u => u.Id == userId && u.AptNumber == aptNumber).FirstOrDefault();
            return applicationUser;
        }

        public async void UpdateUser(AptUser user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Application>> GetApplications(string userId)
        {
            return await _dbContext.Applications.Where(u => u.AptUserId == userId).ToListAsync();
        }

        public async void UpdateApplication(Application app)
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