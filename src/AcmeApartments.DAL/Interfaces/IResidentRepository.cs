using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IResidentRepository
    {
        public Task<Application> GetApplication(int applicationId);

        public Task AddMaintenanceRequest(MaintenanceRequest maintenanceRequest);

        public Task<List<MaintenanceRequest>> GetMaintenanceUserRequests();

        public Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId);
    }
}