using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public ApplicationService(
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Application> GetApplication(int applicationId) => await _unitOfWork.ApplicationRepository.GetByID(applicationId);

        public List<Application> GetApplicationsByAptNumber(string aptNumber)
        {
            var user = _userService.GetUser();

            var apps = _unitOfWork.ApplicationRepository.Get(
                filter: application => application.AptNumber == aptNumber
                && application.AptUserId == user.Result.Id
                && (application.Status == null || application.Status == "Approved")).ToList();

            return apps;
        }

        public List<Application> GetApplications(string userId) => _unitOfWork.ApplicationRepository.Get(filter: application => application.AptUserId == userId).ToList();

        public async Task<List<AptUser>> GetApplicationUsers()
        {
            var users = await (from userRecord in _unitOfWork.AptUserRepository.Get()
                               join applicationRecord in _unitOfWork.ApplicationRepository.Get() on userRecord.Id equals applicationRecord.AptUserId
                               select userRecord).Distinct().ToListAsync();

            return users;
        }
    }
}