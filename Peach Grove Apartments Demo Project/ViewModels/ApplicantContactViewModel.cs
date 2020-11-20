using System.ComponentModel.DataAnnotations;

namespace Peach_Grove_Apartments_Demo_Project.ViewModels
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