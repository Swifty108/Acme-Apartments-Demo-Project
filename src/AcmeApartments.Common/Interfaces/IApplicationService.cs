using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;

namespace AcmeApartments.Common.Interfaces
{
    public interface IApplicationService
    {
        Application GetApplication(int applicationId);

        public List<Application> GetApplications(string userId);

        public List<AptUser> GetApplicationUsers();
    }
}