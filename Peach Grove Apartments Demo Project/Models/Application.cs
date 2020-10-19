using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Models
{
    public class Application
    {
        public Guid ApplicationId { get; set; }
        [ForeignKey(nameof(AptUser))]
        public string AptUserId { get; set; }
        public AptUser AptUser { get; set; }
        [Required]
        public string Occupation { get; set; }
        public int? Income { get; set; }
        [Required]
        [Display(Name = "Reason for Moving")]
        public string ReasonForMoving { get; set; }
        [Required]
        [RegularExpression(@"^\d{9}|\d{3}-\d{2}-\d{4}$", ErrorMessage = "Invalid Social Security Number")]
        public string SSN { get; set; }
        public string Room { get; set; }
        public string Price{ get; set; }


    }
}
