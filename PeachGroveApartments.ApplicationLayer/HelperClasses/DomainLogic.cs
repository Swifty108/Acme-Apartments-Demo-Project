using Microsoft.AspNetCore.Identity;
using PeachGroveApartments.ApplicationLayer.Interfaces;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System.Threading.Tasks;

namespace PeachGroveApartments.ApplicationLayer.HelperClasses
{
    public class DomainLogic : IDomainLogic
    {
        private readonly IRepository _repository;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AptUser> _userManager;

        public DomainLogic(IRepository repository, ApplicationDbContext dbContext, UserManager<AptUser> userManager)
        {
            _repository = repository;
            _dbContext = dbContext;
            _userManager = userManager;
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

            var app = await _dbContext.Applications.FindAsync(appId);

            app.Status = ApplicationStatus.APPROVED;

            _dbContext.Applications.Update(app);

            await _userManager.RemoveFromRoleAsync(applicationUser, "Applicant");
            await _userManager.AddToRoleAsync(applicationUser, "Resident");

            await _dbContext.SaveChangesAsync();
        }
    }
}