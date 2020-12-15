using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.BLL.DTOs
{
    public class ReviewViewModelDTO
    {
        [Required]
        [MaxLength(10000)]
        [DisplayName("Review Text")]
        public string ReviewText { get; set; }
    }
}