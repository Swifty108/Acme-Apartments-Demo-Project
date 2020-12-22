using AcmeApartments.DAL.Models;
using System.Collections.Generic;

namespace AcmeApartments.Web.ViewModels
{
    public class UserApplicationsViewModel
    {
        public IList<Application> Applications { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}