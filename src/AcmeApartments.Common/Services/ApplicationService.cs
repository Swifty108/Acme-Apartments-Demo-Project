using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AptUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(
            IHttpContextAccessor accessor,
            UserManager<AptUser> userManager,
            IUnitOfWork unitOfWork,
            IUserService userService)

        {
            _accessor = accessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Application> GetApplication(int applicationId) => await _unitOfWork.ApplicationRepository.GetByID(applicationId);

        public async Task<List<Application>> GetApplications(string userId) => await _unitOfWork.ApplicationRepository.Get(filter: application => application.AptUserId == userId).ToListAsync();

        public async Task<List<AptUser>> GetApplicationUsers()
        {
            var users = await (from userRecord in _unitOfWork.AptUserRepository.Get()
                               join applicationRecord in _unitOfWork.ApplicationRepository.Get() on userRecord.Id equals applicationRecord.AptUserId
                               select userRecord).Distinct().ToListAsync();

            return users;
        }
    }
}