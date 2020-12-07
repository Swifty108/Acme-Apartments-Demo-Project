using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IRepository
    {
        public AptUser GetAptUser(string userId, string aptNumber);

        public void UpdateApplication(Application app);
    }
}