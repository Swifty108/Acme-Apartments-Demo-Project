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
        private readonly IManagerRepository _managerRepository;

        public ApplicationService(
            IHttpContextAccessor accessor,
            UserManager<AptUser> userManager,
            IRepository repository,
            IManagerRepository managerRepository)

        {
            _accessor = accessor;
            _userManager = userManager;
            _repository = repository;
            _managerRepository = managerRepository;
        }

        public async Task<Application> GetApplication(int applicationId) => await _repository.GetApplication(applicationId);

        public async Task<List<Application>> GetApplications(string userId) => await _repository.GetApplications(userId);

        public async Task<List<AptUser>> GetApplicationUsers()
        {
            return await _managerRepository.GetApplicationUsers();
        }
    }
}