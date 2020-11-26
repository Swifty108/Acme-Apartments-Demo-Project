using PeachGroveApartments.Core.Models;
using System.Collections.Generic;

namespace PeachGroveApartments.ApplicationLayer.ViewModels
{
    public class FloorPlansViewModel
    {
        public IList<FloorPlan> StudioPlans { get; set; }
        public IList<FloorPlan> OneBedPlans { get; set; }
        public IList<FloorPlan> TwoBedPlans { get; set; }
        public string FloorPlanType { get; set; }
    }
}