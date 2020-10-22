using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Models
{
    public class MaintenanceRequestViewModel
    {
        //[TempData]
        //public string isSuccess { get; set; }
        [Required]
        [MaxLength(10000)]
        public string ProblemDescription { get; set; }
        [Required]
        [DisplayName("Permitted to enter the apartment?")]
        public bool isAllowedToEnter { get; set; }


    }
}
