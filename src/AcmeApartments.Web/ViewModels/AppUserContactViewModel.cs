using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.ViewModels
{
    public class AppUserContactViewModel
    {
        [Required]
        [Display(Name = "Your Name")]
        public string SenderName { get; set; }

        [Required]
        [Display(Name = "Your Email Address")]
        public string FromEmailAddress { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Message { get; set; }
    }
}