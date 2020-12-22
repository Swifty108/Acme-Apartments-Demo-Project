using AcmeApartments.DAL.Models;
using System.Collections.Generic;

namespace AcmeApartments.Web.ViewModels
{
    public class MaintenanceRequestsListViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public List<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}