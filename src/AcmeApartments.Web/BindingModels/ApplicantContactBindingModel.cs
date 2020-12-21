using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.BindingModels
{
    public class ApplicantContactBindingModel
    {
        [Required]
        [MaxLength(10000)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Message { get; set; }
    }
}