using AcmeApartments.Data.Provider.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmeApartments.Data.Provider.Entities
{
    public class MaintenanceRequestEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("MaintenanceRequest")]
        public int MaintenanceRequestId { get; set; }

        public MaintenanceRequest MaintenanceRequest { get; set; }

        [Required]
        [ForeignKey("User")]
        public string AptUserId { get; set; }

        public AptUser User { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ProblemDescription { get; set; }

        [Required]
        public string Notes { get; set; }

        [DataType(DataType.Date)]
        public string StartTime { get; set; }
    }
}