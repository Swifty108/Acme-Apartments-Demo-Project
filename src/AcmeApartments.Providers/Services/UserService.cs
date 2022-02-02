using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AptUser> GetUserByID(string userId)
        {
            var user = await _unitOfWork.AptUserRepository.GetByID(userId);
            return user;
        }
    }
}