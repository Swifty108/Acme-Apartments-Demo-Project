using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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


    }
}
