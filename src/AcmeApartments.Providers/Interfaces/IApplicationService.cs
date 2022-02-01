using AcmeApartments.Providers.DTOs;
using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Interfaces
{
    public interface IApplicationService
    {
        Task<bool> CheckifApplicationExists(string aptNumber);

        Task<string> Apply(ApplyModelDto applyViewModelDTO);

        Task<Application> GetApplication(int applicationId);
        
        List<Application> GetApplications(string userId);
        
        Task<List<Application>> GetApplicationsByAptNumber(string aptNumber);
        
        Task<List<AptUser>> GetApplicationUsers();

        Task EditApplication(ApplicationDto applicationDTO);

        Task ApproveApplication(
            string userId,
            int appId,
            string ssn,
            string aptNumber,
            string aptPrice
            );

        Task UnApproveApplication(string userId, string aptNumber, int appId);

        Task<Application> CancelApplication(int ApplicationId);
    }
}