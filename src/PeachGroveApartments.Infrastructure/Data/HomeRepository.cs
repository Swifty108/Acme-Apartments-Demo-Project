using Microsoft.EntityFrameworkCore;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace PeachGroveApartments.Infrastructure.Interfaces
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FloorPlansViewModelDTO> GetFloorPlans()
        {
            var studioPlans = await _dbContext.FloorPlans.Where(f => f.FloorPlanType == "Studio").ToListAsync();
            var oneBedPlans = await _dbContext.FloorPlans.Where(f => f.FloorPlanType == "1Bed").ToListAsync();
            var twoBedPlans = await _dbContext.FloorPlans.Where(f => f.FloorPlanType == "2Bed").ToListAsync();

            return new FloorPlansViewModelDTO
            {
                StudioPlans = studioPlans,
                OneBedPlans = oneBedPlans,
                TwoBedPlans = twoBedPlans
            };
        }
    }
}