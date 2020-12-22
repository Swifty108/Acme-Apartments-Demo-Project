using AcmeApartments.DAL.Models;

namespace AcmeApartments.Web.ViewModels
{
    public class MaintenanceRequestViewViewModel
    {
        public int Id { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        public MaintenanceRequest MaintenanceRequest { get; set; }
    }
}