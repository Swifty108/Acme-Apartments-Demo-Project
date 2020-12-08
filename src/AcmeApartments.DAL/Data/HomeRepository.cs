using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Data
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

        public void AddApplication(Application app)
        {
            _dbContext.Add(app);
            _dbContext.SaveChangesAsync();
        }
    }
}