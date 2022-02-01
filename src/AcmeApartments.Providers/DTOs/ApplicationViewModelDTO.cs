using AcmeApartments.Data.Provider.Entities;
using System.Collections.Generic;

namespace AcmeApartments.Providers.DTOs
{
    public class ApplicationViewModelDto
    {
        public IList<Application> Applications { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}