using PeachGroveApartments.ApplicationLayer.ViewModels;
using PeachGroveApartments.Infrastructure.Models;
using System.Threading.Tasks;

namespace PeachGroveApartments.ApplicationLayer.Interfaces
{
    public interface IDomainLogic
    {
        public Task<Application> CancelApplication(int ApplicationId);

        public Task ApproveApplication(string userId, int appId, string ssn, string aptNumber, string aptPrice);

        public Task UnApproveApplication(string id, string aptNumber, int appid);

        public Task<MaintenanceRequest> EditMaintenanceRequest(MaintenanceRequestViewModel maintenanceViewModel);

        public Task ApproveMaintenanceRequest(string userId, int maintenanceId);

        public Task UnApproveMaintenanceRequest(string userId, int maintenanceId);
    }
}