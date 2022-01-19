using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IApplicationService
    {
        Task<bool> CheckifApplicationExists(string aptNumber);

        Task<string> Apply(ApplyViewModelDTO applyViewModelDTO);

        Task<Application> GetApplication(int applicationId);
        
        List<Application> GetApplications(string userId);
        
        Task<List<Application>> GetApplicationsByAptNumber(string aptNumber);
        
        Task<List<AptUser>> GetApplicationUsers();

        Task EditApplication(ApplicationDTO applicationDTO);

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