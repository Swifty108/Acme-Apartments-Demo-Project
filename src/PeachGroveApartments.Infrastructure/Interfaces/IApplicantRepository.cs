using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Interfaces
{
    public interface IApplicantRepository
    {
        public Task<List<Application>> GetApplications(string userId);

        public Task<Application> GetApplication(int applicationId);
    }
}