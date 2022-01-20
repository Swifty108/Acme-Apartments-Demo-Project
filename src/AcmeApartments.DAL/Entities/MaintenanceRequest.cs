﻿using AcmeApartments.DAL.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeApartments.DAL.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        [Required]
        [ForeignKey("User")]
        public string AptUserId { get; set; }

        public AptUser User { get; set; }

        [Required]
        public string ProblemDescription { get; set; }

        public bool isAllowedToEnter { get; set; }
        public string Status { get; set; }
    }
}