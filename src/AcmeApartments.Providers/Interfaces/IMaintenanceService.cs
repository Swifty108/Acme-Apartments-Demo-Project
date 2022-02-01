using AcmeApartments.Providers.DTOs;
using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Interfaces
{
    public interface IMaintenanceService
    {
        Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId);

        Task<List<MaintenanceRequest>> GetMaintenanceRequests();

        Task<List<AptUser>> GetMaintenanceRequestsUsers();

        Task<List<MaintenanceRequest>> GetMaintenanceUserRequests(string aptUserId);

        Task<MaintenanceRequest> EditMaintenanceRequest(MaintenanceRequestEditDto maintenanceRequestEditDTO);

        Task ApproveMaintenanceRequest(string userId, int maintenanceId);

        Task UnApproveMaintenanceRequest(string userId, int maintenanceId);

        Task SubmitMaintenanceRequest(NewMaintenanceRequestDto newMaintenanceRequestDTO);
    }
}
