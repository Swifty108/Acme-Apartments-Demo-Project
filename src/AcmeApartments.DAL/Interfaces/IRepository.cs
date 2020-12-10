using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IRepository
    {
        public Task<AptUser> GetApplicationUser(string userId);

        public Task UpdateUser(AptUser user);

        public Task<Application> GetApplication(int appId);

        public Task UpdateApplication(Application app);

        public Task<List<Application>> GetApplications(string userId);

        public void UpdateMaintenaceRequest(MaintenanceRequest mRequest);
    }
}