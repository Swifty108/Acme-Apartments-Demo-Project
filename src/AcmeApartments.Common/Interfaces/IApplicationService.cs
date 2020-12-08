using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.Common.Interfaces
{
    public interface IApplicationService
    {
        public Task<List<AptUser>> GetApplications(string userId);

        public Task<List<AptUser>> GetApplicationUsers();

        Task<Application> GetApplication(int applicationId);
    }
}