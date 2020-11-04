using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Models
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
        [ForeignKey("AptUser")]
        public string AptUserId { get; set; }
        public AptUser AptUser { get; set; }
        [Required]
        public string ProblemDescription { get; set; }
        public bool isAllowedToEnter { get; set; }
        public bool isApproved { get; set; }

    }
}
