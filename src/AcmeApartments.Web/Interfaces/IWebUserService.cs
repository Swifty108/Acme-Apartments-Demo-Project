using AcmeApartments.Data.Provider.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Interfaces
{
    public interface IWebUserService
    {
        public Task<AptUser> GetUser();
        public string GetUserId();
    }
}