using System;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.BindingModels
{
    public class ApplyBindingModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

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