using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Data.Provider.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Services
{
    public class WebUserService : IWebUserService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AptUser> _userManager;

        public WebUserService(
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
            return user;
        }

        public string GetUserId()
        {
            return _userManager.GetUserId(_accessor?.HttpContext.User);
        }
    }
}