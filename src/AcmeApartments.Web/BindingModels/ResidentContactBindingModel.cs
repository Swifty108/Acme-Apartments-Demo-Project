using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Web.BindingModels
{
    public class ResidentContactBindingModel
    {
        [Required]
        [MaxLength(10000)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Message { get; set; }
    }
}