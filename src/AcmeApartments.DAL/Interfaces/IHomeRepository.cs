using System.Threading.Tasks;

namespace AcmeApartments.DAL.Interfaces
{
    public interface IHomeRepository
    {
        public Task<FloorPlansViewModelDTO> GetFloorPlans();
    }
}