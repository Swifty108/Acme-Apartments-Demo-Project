using AcmeApartments.Common.DTOs;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.ViewModels
{
    public class ApplyViewModel
    {
        public AptUserDTO User { get; set; }

        [Required]
        public string Occupation { get; set; }

        public string AptNumber { get; set; }
        public string Area { get; set; }
        public string Price { get; set; }

        [Required]
        public int? Income { get; set; }

        [Required]
        [Display(Name = "Reason for Moving")]
        public string ReasonForMoving { get; set; }

        [Required]
        [RegularExpression(@"^\d{9}|\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid Social Security Number")]
        public string SSN { get; set; }

        public string FloorPlanType { get; set; }
    }
}