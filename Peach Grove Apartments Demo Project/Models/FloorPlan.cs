using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Models
{
    public class FloorPlan
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
    }
}
