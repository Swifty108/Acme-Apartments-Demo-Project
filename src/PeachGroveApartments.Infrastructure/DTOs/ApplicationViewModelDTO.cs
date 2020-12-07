using PeachGroveApartments.Infrastructure.Models;
using System.Collections.Generic;

namespace PeachGroveApartments.Infrastructure.DTOs
{
    public class ApplicationViewModelDTO
    {
        public IList<Application> Applications { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}