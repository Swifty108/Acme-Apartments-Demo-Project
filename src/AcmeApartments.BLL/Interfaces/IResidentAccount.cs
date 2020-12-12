using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IResidentAccount
    {
        public PaymentsViewModelDTO GetBills(AptUser user);

        public Task AddReview(ReviewViewModelDTO review);

        public Task SubmitMaintenanceRequest(MaintenanceRequestDTO maintenanceRequestDTO);

        public List<MaintenanceRequest> GetMaintenanceRequests();

        public List<Application> GetApplications();
    }
}