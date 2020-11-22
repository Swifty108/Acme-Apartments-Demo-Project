using Microsoft.AspNetCore.Identity;
using PeachGroveApartments.ApplicationLayer.Interfaces;
using PeachGroveApartments.ApplicationLayer.ViewModels;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System.Threading.Tasks;

namespace PeachGroveApartments.ApplicationLayer.HelperClasses
{
    public class ApplicantLogic : IManagerLogic
    {
        private readonly IManagerRepository _repository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AptUser> _userManager;
        private readonly IManagerRepository _managerRepository;

        public ApplicantLogic(IManagerRepository repository, ApplicationDbContext dbContext, UserManager<AptUser> userManager, IManagerRepository managerRepository)
        {
            _repository = repository;
            _dbContext = dbContext;
            _userManager = userManager;
            _managerRepository = managerRepository;
        }

        public async Task<Application> CancelApplication(int ApplicationId)
        {
            var application = await _repository.GetApplication(ApplicationId);
            application.Status = ApplicationStatus.CANCELED;

            _dbContext.Applications.Update(application);
            await _dbContext.SaveChangesAsync();

            return application;
        }

        public async Task ApproveApplication(string userId, int appIdint, string ssn, string aptNumber, string aptPrice)
        {
            var applicationUser = await _dbContext.Users.FindAsync(userId);

            applicationUser.SSN = ssn;

            applicationUser.AptNumber = aptNumber;
            applicationUser.AptPrice = aptPrice;
            _dbContext.Users.Update(applicationUser);

            var app = await _dbContext.Applications.FindAsync(appIdint);

            app.Status = ApplicationStatus.APPROVED;

            _dbContext.Applications.Update(app);

            await _userManager.RemoveFromRoleAsync(applicationUser, "Applicant");
            await _userManager.AddToRoleAsync(applicationUser, "Resident");

            await _dbContext.SaveChangesAsync();
        }

        public async Task UnApproveApplication(string id, string aptNumber, int appid)
        {
            var applicationUser = await _dbContext.Users.FindAsync(id);

            if (applicationUser.AptNumber == aptNumber)
            {
                applicationUser.SSN = null;
                applicationUser.AptNumber = null;
                applicationUser.AptPrice = null;

                await _userManager.RemoveFromRoleAsync(applicationUser, "Resident");
                await _userManager.AddToRoleAsync(applicationUser, "Applicant");

                _dbContext.Users.Update(applicationUser);
            }
            var app = await _dbContext.Applications.FindAsync(appid);

            app.Status = ApplicationStatus.UNAPPROVED;

            _dbContext.Applications.Update(app);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<MaintenanceRequest> EditMaintenanceRequest(MaintenanceRequestViewModel maintenanceViewModel)
        {
            var maintenanceRecord = await _managerRepository.GetMaintenanceRequest(maintenanceViewModel.Id);

            maintenanceRecord.ProblemDescription = maintenanceViewModel.ProblemDescription;
            maintenanceRecord.isAllowedToEnter = maintenanceViewModel.isAllowedToEnter;
            //  var mRecord = _mapper.Map<MaintenanceRequest>(mViewModel);

            _dbContext.Update(maintenanceRecord);
            await _dbContext.SaveChangesAsync();

            return maintenanceRecord;
        }

        public async Task ApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var mRecord = await _dbContext.MaintenanceRequests.FindAsync(maintenanceId);
            mRecord.AptUserId = userId;
            mRecord.Status = MaintenanceRequestStatus.APPROVED;

            _dbContext.MaintenanceRequests.Update(mRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var maintenanceRecord = await _dbContext.MaintenanceRequests.FindAsync(maintenanceId);
            maintenanceRecord.AptUserId = userId;
            maintenanceRecord.Status = MaintenanceRequestStatus.UNAPPROVED;

            _dbContext.MaintenanceRequests.Update(maintenanceRecord);
            await _dbContext.SaveChangesAsync();
        }
    }
}