using AcmeApartments.Providers.DTOs;
using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Interfaces
{
    public interface IApplicationService
    {
        Task<bool> CheckifApplicationExists(string aptNumber, string userId);

        public Task Apply(ApplyModelDto applyViewModelDTO, AptUser user);

        Task<Application> GetApplication(int applicationId);

        List<Application> GetApplications(string userId);

        Task<List<Application>> GetApplicationsByAptNumber(string aptNumber, string userId);

        Task<List<AptUser>> GetApplicationUsers();

        Task<bool> EditApplication(ApplicationDto applicationDTO);

        public Task<bool> ApproveApplication(
            AptUser user,
            int appId,
            string ssn,
            string aptNumber,
            string aptPrice,
            IList<string> roles
        );

        Task<bool> DenyApplication(string userId, string aptNumber, int appId);

        Task<bool> CancelApplication(int ApplicationId);
    }
}