using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.BindingModels
{
    public class NewMaintenanceRequestBindingModel
    {
        [Required]
        [MaxLength(10000)]
        public string ProblemDescription { get; set; }
        public bool isAllowedToEnter { get; set; }
    }
}