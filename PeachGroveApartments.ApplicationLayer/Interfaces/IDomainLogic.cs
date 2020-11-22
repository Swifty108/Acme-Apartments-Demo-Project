using PeachGroveApartments.Infrastructure.Models;
using System.Threading.Tasks;

namespace PeachGroveApartments.ApplicationLayer.Interfaces
{
    public interface IDomainLogic
    {
        public Task<Application> CancelApplication(int ApplicationId);

        public Task ApproveApplication(string userId, string appId, string ssn, string aptNumber, string aptPrice)
    }
}