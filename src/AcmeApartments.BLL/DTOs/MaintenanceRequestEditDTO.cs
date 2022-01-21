using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.BLL.DTOs
{
    public class MaintenanceRequestEditDto
    {
        public int Id { get; set; }

        public string AptUserId { get; set; }

        [Required]
        [MaxLength(10000)]
        [Display(Name = "Problem Description")]
        public string ProblemDescription { get; set; }

        [DisplayName("Permitted to enter the apartment?")]
        public bool isAllowedToEnter { get; set; }
    }
}