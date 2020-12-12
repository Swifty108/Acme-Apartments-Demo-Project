using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IManagerAccount
    {
        public void EditApplication(ApplicationDTO application);

        public Application CancelApplication(int ApplicationId);

        public Task ApproveApplication(string userId, int appId, string ssn, string aptNumber, string aptPrice);

        public Task UnApproveApplication(string id, string aptNumber, int appid);

        public MaintenanceRequest GetMaintenanceRequest(int maintenanceId);

        public List<AptUser> GetMaintenanceRequestsUsers();

        public List<MaintenanceRequest> GetMaintenanceUserRequests();

        public MaintenanceRequest EditMaintenanceRequest(MaintenanceRequestDTO maintenanceViewModel);

        public void ApproveMaintenanceRequest(string userId, int maintenanceId);

        public void UnApproveMaintenanceRequest(string userId, int maintenanceId);
    }
}