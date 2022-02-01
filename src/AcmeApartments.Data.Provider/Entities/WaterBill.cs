using AcmeApartments.Data.Provider.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeApartments.Data.Provider.Entities
{
    public class WaterBill
    {
        [Key]
        public int WaterBillId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string AptUserId { get; set; }

        public AptUser User { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDue { get; set; }
    }
}