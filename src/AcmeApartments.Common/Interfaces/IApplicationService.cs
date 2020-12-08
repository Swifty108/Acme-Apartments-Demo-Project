using AcmeApartments.DAL.Identity;
using System.Collections.Generic;

namespace AcmeApartments.Common.Interfaces
{
    public interface IApplicationService
    {
        public List<AptUser> GetApplications(string userId);
    }
}