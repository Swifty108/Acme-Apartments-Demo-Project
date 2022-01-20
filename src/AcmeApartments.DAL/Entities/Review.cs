﻿using AcmeApartments.DAL.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeApartments.DAL.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateReviewed { get; set; }

        [Required]
        [ForeignKey("User")]
        public string AptUserId { get; set; }

        public AptUser User { get; set; }

        [Required]
        [MaxLength(10000)]
        [DisplayName("Review Text")]
        public string ReviewText { get; set; }
    }
}