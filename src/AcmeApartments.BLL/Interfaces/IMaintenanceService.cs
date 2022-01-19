using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IMaintenanceService
    {
        Task<MaintenanceRequest> GetMaintenanceRequest(int maintenanceId);

        Task<List<MaintenanceRequest>> GetMaintenanceRequests();

        Task<List<AptUser>> GetMaintenanceRequestsUsers();

        Task<List<MaintenanceRequest>> GetMaintenanceUserRequests(string aptUserId);

        Task<MaintenanceRequest> EditMaintenanceRequest(MaintenanceRequestEditDTO maintenanceRequestEditDTO);

        Task ApproveMaintenanceRequest(string userId, int maintenanceId);

        Task UnApproveMaintenanceRequest(string userId, int maintenanceId);

        Task SubmitMaintenanceRequest(NewMaintenanceRequestDTO newMaintenanceRequestDTO);
    }
}
