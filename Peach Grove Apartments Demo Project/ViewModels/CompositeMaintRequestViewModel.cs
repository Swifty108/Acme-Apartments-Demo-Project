using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;

namespace Peach_Grove_Apartments_Demo_Project.ViewModels
{
    public class CompositeMaintRequestViewModel
    {
        public MaintenanceRequestViewModel MaintenanceRequestViewModel { get; set; }
        public List<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}