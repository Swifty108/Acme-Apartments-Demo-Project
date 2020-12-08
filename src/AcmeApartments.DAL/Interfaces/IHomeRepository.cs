using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Models;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IHomeRepository
    {
        public Task<FloorPlansViewModelDTO> GetFloorPlans();

        public void AddApplication(Application app);
    }
}