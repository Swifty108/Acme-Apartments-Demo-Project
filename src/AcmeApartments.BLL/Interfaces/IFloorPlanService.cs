using AcmeApartments.DAL.DTOs;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IFloorPlanService
    {
        Task<FloorPlansViewModelDTO> GetFloorPlans();
    }
}