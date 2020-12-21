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

        public Task AddReview(ReviewViewModelDTO review);

        public Task SubmitMaintenanceRequest(NewMaintenanceRequestDTO newMaintenanceRequestDTO);

        public Task<List<MaintenanceRequest>> GetMaintenanceRequests();

        public List<Application> GetApplications();
    }
}