using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AptUser> _userManager;
        private readonly IRepository _repository;

        public UserService(
            IHttpContextAccessor accessor,
            UserManager<AptUser> userManager,
            IRepository repository)
        {
            _accessor = accessor;
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<AptUser> GetUser()
        {
            var userClaimsPrincipal = _accessor?.HttpContext.User;
            var user = await _userManager.GetUserAsync(userClaimsPrincipal);
            //var user = await _userManager.GetUserAsync(_accessor?.HttpContext.User);
            return user;
        }

        public async Task<AptUser> GetUser(string userId)
        {
            var user = await _repository.GetApplicationUser(userId);
            return user;
        }

        public string GetUserId()
        {
            return _userManager.GetUserId(_accessor?.HttpContext.User);
        }
    }
}