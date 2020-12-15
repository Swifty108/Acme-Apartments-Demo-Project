using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.ViewModels
{
    public class ApplicantContactViewModel
    {
        [Required]
        [MaxLength(10000)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Message { get; set; }
    }
}