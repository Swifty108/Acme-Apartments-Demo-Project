using AcmeApartments.Data.Provider.Entities;
using System.Collections.Generic;

namespace AcmeApartments.Providers.DTOs
{
    public class FloorPlansViewModelDto
    {
        public IList<FloorPlan> StudioPlans { get; set; }
        public IList<FloorPlan> OneBedPlans { get; set; }
        public IList<FloorPlan> TwoBedPlans { get; set; }
    }
}