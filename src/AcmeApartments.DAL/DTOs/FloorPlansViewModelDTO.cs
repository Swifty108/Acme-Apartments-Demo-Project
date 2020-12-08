using AcmeApartments.DAL.Models;
using System.Collections.Generic;

namespace AcmeApartments.DAL.DTOs
{
    public class FloorPlansViewModelDTO
    {
        public IList<FloorPlan> StudioPlans { get; set; }
        public IList<FloorPlan> OneBedPlans { get; set; }
        public IList<FloorPlan> TwoBedPlans { get; set; }
    }
}