using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Interfaces
{
    public interface IApplicationService
    {
        public Task<Application> GetApplication(int applicationId);

        public List<Application> GetApplications(string userId);

        public List<Application> GetApplicationsByAptNumber(string aptNumber);

        public Task<List<AptUser>> GetApplicationUsers();
    }
}