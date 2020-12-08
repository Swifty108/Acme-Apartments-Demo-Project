using AcmeApartments.BLL.Interfaces;
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
        private readonly IApplicantRepository _applicantRepository;
        private readonly IRepository _repository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AptUser> _userManager;
        private readonly IHttpContextAccessor _accessor;

        public ApplicantAccount(
            ApplicationDbContext dbContext,
            UserManager<AptUser> userManager,
            IManagerRepository managerRepository,
            IRepository repository,
            IApplicantRepository applicantRepository,
            IHttpContextAccessor accessor)
        {
            _managerRepository = managerRepository;
            _dbContext = dbContext;
            _userManager = userManager;
            _repository = repository;
            _accessor = accessor;
            _applicantRepository = applicantRepository;
        }

        public async Task<List<Application>> GetApplications()
        {
            var user = _accessor?.HttpContext.User;

            var userId = _userManager.GetUserId(user);

            return await _applicantRepository.GetApplications(userId);
        }

        public async Task<Application> GetApplication(int appId)
        {
            var application = await _applicantRepository.GetApplication(appId);
            return application;
        }
    }
}