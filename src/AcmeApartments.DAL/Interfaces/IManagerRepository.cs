using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IManagerRepository
    {
        public Task<ApplicationViewModelDTO> GetApplications(string userId);

        public Task<Application> GetApplication(int applicationId);

        public Task<List<AptUser>> GetApplicationUsers();

        public Task<List<AptUser>> GetMaintenanceRequestsUsers();

        public Task<List<MaintenanceRequest>> GetMaintenanceUserRequests();

        public Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId);

        public Task EditApplication(Application application);
    }
}