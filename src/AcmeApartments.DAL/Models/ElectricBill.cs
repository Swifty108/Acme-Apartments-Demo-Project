using AcmeApartments.DAL.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeApartments.DAL.Models
{
    public class ElectricBill
    {
        [Key]
        public int ElectricBillId { get; set; }

        [Required]
        [ForeignKey("AptUser")]
        public string AptUserId { get; set; }

        public AptUser AptUser { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDue { get; set; }
    }
}