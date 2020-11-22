using PeachGroveApartments.Infrastructure.DTOs;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Interfaces
{
    public interface IHomeRepository
    {
        public Task<FloorPlansViewModelDTO> GetFloorPlans();
    }
}