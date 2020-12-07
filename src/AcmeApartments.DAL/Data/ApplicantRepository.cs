using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Data
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicantRepository(ApplicationDbContext dbContext, UserManager<AptUser> userManager)
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
    }
}