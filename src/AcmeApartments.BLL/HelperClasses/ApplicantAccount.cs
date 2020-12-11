using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Data;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class ApplicantAccount : IApplicantAccount
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IRepository<Application> _appRepository;
        private readonly IRepository _repository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AptUser> _userManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserService _userService;

        public ApplicantAccount(
            ApplicationDbContext dbContext,
            UserManager<AptUser> userManager,
            IManagerRepository managerRepository,
            IRepository repository,
            IApplicantRepository applicantRepository,
            IHttpContextAccessor accessor,
            IUserService userService,
            IRepository<Application> appRepository)
        {
            _managerRepository = managerRepository;
            _dbContext = dbContext;
            _userManager = userManager;
            _repository = repository;
            _userService = userService;
            _accessor = accessor;
            _applicantRepository = applicantRepository;

            _appRepository = appRepository;
        }

        public async Task<List<Application>> GetApplications()
        {
            var userId = _userService.GetUserId();

            return await _appRepository.GetApplications(userId);
        }

        public async Task<Application> GetApplication(int appId)
        {
            var application = await _applicantRepository.GetApplication(appId);
            return application;
        }
    }
}