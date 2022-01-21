using AcmeApartments.BLL.DTOs;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IFloorPlanService
    {
        Task<FloorPlansViewModelDto> GetFloorPlans();
    }
}