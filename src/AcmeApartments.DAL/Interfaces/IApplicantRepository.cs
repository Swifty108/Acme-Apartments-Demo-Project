using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IApplicantRepository
    {
        public Task<List<Application>> GetApplications(string userId);

        public Task<Application> GetApplication(int applicationId);
    }
}