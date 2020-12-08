using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.HelperClasses;
using AcmeApartments.DAL.Data;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class ManagerAccount : IManagerAccount
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IRepository _repository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AptUser> _userManager;

        public ManagerAccount(
            ApplicationDbContext dbContext,
            UserManager<AptUser> userManager,
            IManagerRepository managerRepository,
            IRepository repository)
        {
            _managerRepository = managerRepository;
            _dbContext = dbContext;
            _userManager = userManager;
            _repository = repository;
        }

        public async Task ApproveApplication(
            string userId,
            int appId,
            string ssn,
            string aptNumber,
            string aptPrice
            )
        {
            var applicationUser = _repository.GetAptUser(userId, aptNumber);
            applicationUser.SSN = ssn;

            applicationUser.AptNumber = aptNumber;
            applicationUser.AptPrice = aptPrice;

            var app = await _managerRepository.GetApplication(appId);

            app.Status = ApplicationStatus.APPROVED;

            _repository.UpdateApplication(app);

            await _userManager.RemoveFromRoleAsync(applicationUser, "Applicant");
            await _userManager.AddToRoleAsync(applicationUser, "Resident");

            await _dbContext.SaveChangesAsync();
        }

        public async Task UnApproveApplication(string userId, string aptNumber, int appId)
        {
            var applicationUser = _repository.GetAptUser(userId, aptNumber);

            if (applicationUser.AptNumber == aptNumber)
            {
                applicationUser.SSN = null;
                applicationUser.AptNumber = null;
                applicationUser.AptPrice = null;

                _repository.UpdateUser(applicationUser);

                await _userManager.RemoveFromRoleAsync(applicationUser, "Resident");
                await _userManager.AddToRoleAsync(applicationUser, "Applicant");

                await _dbContext.SaveChangesAsync();
            }

            var app = await _managerRepository.GetApplication(appId);

            app.Status = ApplicationStatus.UNAPPROVED;

            _repository.UpdateApplication(app);
        }

        public async Task<Application> CancelApplication(int ApplicationId)
        {
            var application = await _managerRepository.GetApplication(ApplicationId);
            application.Status = ApplicationStatus.CANCELED;

            _repository.UpdateApplication(application);
            await _dbContext.SaveChangesAsync();

            return application;
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
            var maintenanceRecord = await _managerRepository.GetMaintenanceRequest(maintenanceId);

            maintenanceRecord.AptUserId = userId;
            maintenanceRecord.Status = MaintenanceRequestStatus.APPROVED;

            _repository.UpdateMaintenaceRequest(maintenanceRecord);
        }

        public async Task UnApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var maintenanceRecord = await _dbContext.MaintenanceRequests.FindAsync(maintenanceId);
            maintenanceRecord.AptUserId = userId;
            maintenanceRecord.Status = MaintenanceRequestStatus.UNAPPROVED;

            _repository.UpdateMaintenaceRequest(maintenanceRecord);
        }

        public async Task EditApplication(Application application)
        {
            await _managerRepository.EditApplication(application);
        }
    }
}