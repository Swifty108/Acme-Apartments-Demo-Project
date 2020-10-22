using System.Collections.Generic;

namespace Peach_Grove_Apartments_Demo_Project.Models
{
    public class CompositeMaintRequestViewModel
    {
        public MaintenanceRequestViewModel MaintenanceRequestViewModel { get; set; }
        public List<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}