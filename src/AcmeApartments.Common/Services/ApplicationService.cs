using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AptUser> _userManager;
        private readonly IRepository _repository;

        public ApplicationService(
            IHttpContextAccessor accessor,
            UserManager<AptUser> userManager,
            IRepository repository)

        {
            _accessor = accessor;
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<List<Application>> GetApplications(string userId) => await _repository.GetApplications(userId);
    }
}