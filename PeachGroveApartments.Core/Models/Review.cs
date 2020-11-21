using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeachGroveApartments.Core.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateReviewed { get; set; }

        [Required]
        [ForeignKey("AptUser")]
        public string AptUserId { get; set; }

        public AptUser AptUser { get; set; }

        [Required]
        [MaxLength(10000)]
        [DisplayName("Review Text")]
        public string ReviewText { get; set; }
    }
}