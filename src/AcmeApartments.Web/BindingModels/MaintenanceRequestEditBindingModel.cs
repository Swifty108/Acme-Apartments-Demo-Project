using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.BindingModels
{
    public class MaintenanceRequestEditBindingModel
    {
        public int Id { get; set; }
        public string AptUserId { get; set; }

        [Required]
        [MaxLength(10000)]
        [Display(Name = "Problem Description")]
        public string ProblemDescription { get; set; }

        [DisplayName("Permitted to enter the apartment?")]
        public bool IsAllowedToEnter { get; set; }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}