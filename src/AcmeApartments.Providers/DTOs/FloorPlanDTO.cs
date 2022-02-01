using System;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.Providers.DTOs
{
    public class FloorPlanDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FloorPlanType { get; set; }

        [Required]
        public string AptNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAvailable { get; set; }

        [Required]
        public string SF { get; set; }

        [Required]
        public string Price { get; set; }

        public string Status { get; set; }
    }
}