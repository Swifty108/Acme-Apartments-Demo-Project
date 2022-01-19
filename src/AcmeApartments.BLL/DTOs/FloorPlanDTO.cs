using System;
using System.ComponentModel.DataAnnotations;

namespace AcmeApartments.BLL.DTOs
{
    public class FloorPlanDTO
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