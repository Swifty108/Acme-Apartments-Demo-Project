﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PeachGroveApartments.ApplicationLayer.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        [MaxLength(10000)]
        [DisplayName("Review Text")]
        public string ReviewText { get; set; }
    }
}