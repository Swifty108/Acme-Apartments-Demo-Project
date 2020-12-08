using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeApartments.Common.DTOs
{
    public class WaterBillDTO
    {
        [Key]
        public int WaterBillId { get; set; }

        [Required]
        [ForeignKey("AptUser")]
        public string AptUserId { get; set; }

        public AptUserDTO AptUser { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDue { get; set; }
    }
}