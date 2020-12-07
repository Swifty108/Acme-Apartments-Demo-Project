using PeachGroveApartments.Core.Models;
using System.Collections.Generic;

namespace PeachGroveApartments.Infrastructure.DTOs
{
    public class FloorPlansViewModelDTO
    {
        public IList<FloorPlan> StudioPlans { get; set; }
        public IList<FloorPlan> OneBedPlans { get; set; }
        public IList<FloorPlan> TwoBedPlans { get; set; }
    }
}