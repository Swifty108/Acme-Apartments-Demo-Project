using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Peach_Grove_Apartments_Demo_Project.ViewModels
{
    public class MaintenanceRequestViewModel
    {
        public int Id { get; set; }
        public string AptUserId { get; set; }

        [Required]
        [MaxLength(10000)]
        [Display(Name = "Problem Description")]
        public string ProblemDescription { get; set; }

        [DisplayName("Permitted to enter the apartment?")]
        public bool isAllowedToEnter { get; set; }

        public bool isSuccess { get; set; }
        public string userFName { get; set; }
        public string userLName { get; set; }

        public MaintenanceRequest mRequest { get; set; }
        public List<MaintenanceRequest> mRequests { get; set; }
    }
}