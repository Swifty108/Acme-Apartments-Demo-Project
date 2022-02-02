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

        Task<bool> EditApplication(ApplicationDto applicationDTO);

        Task<bool> ApproveApplication(
            string userId,
            int appId,
            string ssn,
            string aptNumber,
            string aptPrice
            );

        Task<bool> DenyApplication(string userId, string aptNumber, int appId);

        Task<bool> CancelApplication(int ApplicationId);
    }
}