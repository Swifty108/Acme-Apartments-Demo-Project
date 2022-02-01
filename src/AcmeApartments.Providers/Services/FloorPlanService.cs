using AcmeApartments.Providers.DTOs;
using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Data.Provider.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Services
{
    public class FloorPlanService : IFloorPlanService
    {
       private readonly IUnitOfWork _unitOfWork;

        public FloorPlanService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public async Task<FloorPlansViewModelDto> GetFloorPlans()
        {
            var studioFloorPlans = await _unitOfWork.FloorPlanRepository.Get(filter: f => f.FloorPlanType == "Studio").ToListAsync();
            var oneBedFloorPlans = await _unitOfWork.FloorPlanRepository.Get(filter: f => f.FloorPlanType == "1Bed").ToListAsync();
            var twoBedFloorPlans = await _unitOfWork.FloorPlanRepository.Get(filter: f => f.FloorPlanType == "2Bed").ToListAsync();

            var floorPlans = new FloorPlansViewModelDto
            {
                StudioPlans = studioFloorPlans,
                OneBedPlans = oneBedFloorPlans,
                TwoBedPlans = twoBedFloorPlans
            };

            return floorPlans;
        }
    }
}
