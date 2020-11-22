using PeachGroveApartments.Infrastructure.DTOs;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Interfaces
{
    public interface IRepository
    {
        public Task<ApplicationViewModelDTO> GetApplications(string userId);

        public Task<FloorPlansViewModelDTO> GetFloorPlans();
    }
}