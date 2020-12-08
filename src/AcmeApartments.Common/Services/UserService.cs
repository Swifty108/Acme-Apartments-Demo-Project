using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AptUser> _userManager;

        public UserService(
            IHttpContextAccessor accessor,
            UserManager<AptUser> userManager)

        {
            _accessor = accessor;
            _userManager = userManager;
        }

        public async Task<AptUser> GetUser()
        {
            var userClaimsPrincipal = _accessor?.HttpContext.User;
            var user = await _userManager.GetUserAsync(userClaimsPrincipal);
            //var user = await _userManager.GetUserAsync(_accessor?.HttpContext.User);
            return user;
        }

        public string GetUserId()
        {
            return _userManager.GetUserId(_accessor?.HttpContext.User);
        }
    }
}