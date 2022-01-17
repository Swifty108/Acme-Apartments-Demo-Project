using System;

namespace AcmeApartments.Web.ViewModels
{
    public class MaintenanceRequestViewModel
    {
        public int Id { get; set; }
        public string AptUserId { get; set; }
        public DateTime DateRequested { get; set; }
        public string ProblemDescription { get; set; }
        public bool isAllowedToEnter { get; set; }
    }
}
