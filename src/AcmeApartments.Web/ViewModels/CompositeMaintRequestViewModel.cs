using AcmeApartments.DAL.Models;
using System.Collections.Generic;

namespace AcmeApartments.Web.ViewModels
{
    public class CompositeMaintRequestViewModel
    {
        public MaintenanceRequestViewModel MaintenanceRequestViewModel { get; set; }
        public List<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}