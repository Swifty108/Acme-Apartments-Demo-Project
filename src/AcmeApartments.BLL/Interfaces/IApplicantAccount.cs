using AcmeApartments.DAL.Models;
using System.Collections.Generic;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IApplicantAccount
    {
        public List<Application> GetApplications();
    }
}