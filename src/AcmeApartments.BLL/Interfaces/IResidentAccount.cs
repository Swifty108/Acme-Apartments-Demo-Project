using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IResidentAccount
    {
        public Task<PaymentsViewModelDTO> GetBills(AptUser user);

        public Task<List<Application>> GetApplications();

        public Task<List<MaintenanceRequest>> GetMaintenanceRequests();

        public Task SubmitMaintenanceRequest(NewMaintenanceRequestDTO newMaintenanceRequestDTO);

        public Task AddReview(ReviewViewModelDTO review);
    }
}