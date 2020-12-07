﻿using PeachGroveApartments.Infrastructure.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeachGroveApartments.Infrastructure.Models
{
    public class WaterBill
    {
        [Key]
        public int WaterBillId { get; set; }

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