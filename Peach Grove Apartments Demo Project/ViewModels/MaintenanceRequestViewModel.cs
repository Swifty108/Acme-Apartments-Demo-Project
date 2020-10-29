using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Peach_Grove_Apartments_Demo_Project.Models;

namespace Peach_Grove_Apartments_Demo_Project.ViewModels
{
    public class MaintenanceRequestViewModel
    {
        [Required]
        [MaxLength(10000)]
        public string ProblemDescription { get; set; }
        [DisplayName("Permitted to enter the apartment?")]
        public bool isAllowedToEnter { get; set; }
        public bool isSuccess { get; set; }

        public string userFName { get; set; }
        public string userLName { get; set; }

        public List<MaintenanceRequest> mRequests { get; set; }
    }
}
