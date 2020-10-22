using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Peach_Grove_Apartments_Demo_Project.ViewModels
{
    public class ReviewViewModel
    { 
        
        [Required]
        [MaxLength(10000)]
        [DisplayName("Review Text")]
        public string ReviewText { get; set; }
    }
}
