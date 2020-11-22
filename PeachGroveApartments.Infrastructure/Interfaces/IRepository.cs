using PeachGroveApartments.Infrastructure.DTOs;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Interfaces
{
    public interface IRepository
    {
        public Task<ApplicationViewModelDTO> GetApplications(string userId);

        public Task<FloorPlansViewModelDTO> GetFloorPlans();

        public Task<Application> GetApplication(int applicationId);

        public Task<List<AptUser>> GetApplicationUsers();

        public Task<List<AptUser>> GetMaintenanceRequestsUsers();

        public Task<List<MaintenanceRequest>> GetMaintenanceUserRequests()
    }
}