using Peach_Grove_Apartments_Demo_Project.Models;
using System.Collections.Generic;

namespace PeachGroveApartments.Common.ViewModels
{
    public class CompositeMaintRequestViewModel
    {
        public MaintenanceRequestViewModel MaintenanceRequestViewModel { get; set; }
        public List<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}