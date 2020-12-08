using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IRepository
    {
        public AptUser GetAptUser(string userId, string aptNumber);

        public void UpdateUser(AptUser user);

        public void UpdateApplication(Application app);

        public void UpdateMaintenaceRequest(MaintenanceRequest mRequest);
    }
}