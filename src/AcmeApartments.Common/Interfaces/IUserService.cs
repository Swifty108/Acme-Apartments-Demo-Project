using AcmeApartments.DAL.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Interfaces
{
    public interface IUserService
    {
        public Task<AptUser> GetUser();

        public string GetUserId();
    }
}