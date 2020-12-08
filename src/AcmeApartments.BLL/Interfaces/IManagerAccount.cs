using AcmeApartments.BLL.HelperClasses;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IManagerAccount
    {
        public Task EditApplication(ApplicationDTO app);

        public Task<Application> CancelApplication(int ApplicationId);

        public Task ApproveApplication(string userId, int appId, string ssn, string aptNumber, string aptPrice);

        public Task UnApproveApplication(string id, string aptNumber, int appid);

        public Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId);

        public Task<List<AptUser>> GetMaintenanceRequestsUsers();

        public Task<List<MaintenanceRequest>> GetMaintenanceUserRequests();

        public Task<MaintenanceRequest> EditMaintenanceRequest(MaintenanceRequestDTO maintenanceViewModel);

        public Task ApproveMaintenanceRequest(string userId, int maintenanceId);

        public Task UnApproveMaintenanceRequest(string userId, int maintenanceId);
    }
}