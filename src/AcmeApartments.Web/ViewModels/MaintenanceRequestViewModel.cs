using AcmeApartments.DAL.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeApartments.Web.ViewModels
{
    public class MaintenanceRequestViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string AptUserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateRequested { get; set; }

        [Required]
        public string ProblemDescription { get; set; }

        public bool isAllowedToEnter { get; set; }
    }
}
