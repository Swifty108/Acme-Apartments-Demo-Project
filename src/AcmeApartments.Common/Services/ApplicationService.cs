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
        private readonly IUserService _userService;

        public ApplicationService(
            IHttpContextAccessor accessor,
            UserManager<AptUser> userManager,
            IRepository repository,
            IManagerRepository managerRepository,
            IUserService userService)

        {
            _accessor = accessor;
            _userManager = userManager;
            _repository = repository;
            _managerRepository = managerRepository;
            _userService = userService;
        }

        public async Task<Application> GetApplication(int applicationId) => await _repository.GetApplication(applicationId);

        public async Task<List<Application>> GetApplications(string userId)
        {
            var apps = await _repository.GetApplications(userId);
            return apps;
        }

        public async Task<List<AptUser>> GetApplicationUsers()
        {
            return await _managerRepository.GetApplicationUsers();
        }
    }
}