using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IRepository
    {
        public AptUser GetAptUser(string userId, string aptNumber);

        public void UpdateUser(AptUser user);

        public Task<Application> GetApplication(int appId);

        public void UpdateApplication(Application app);

        public Task<List<Application>> GetApplications(string userId);

        public void UpdateMaintenaceRequest(MaintenanceRequest mRequest);
    }
}