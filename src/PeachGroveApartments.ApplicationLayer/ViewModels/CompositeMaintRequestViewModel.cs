using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;

namespace PeachGroveApartments.ApplicationLayer.ViewModels
{
    public class CompositeMaintRequestViewModel
    {
        public MaintenanceRequestViewModel MaintenanceRequestViewModel { get; set; }
        public List<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}