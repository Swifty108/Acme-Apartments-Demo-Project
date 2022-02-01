using AcmeApartments.Providers.DTOs;
using System.Collections.Generic;

namespace AcmeApartments.Web.ViewModels
{
    public class FloorPlansViewModel
    {
        public IList<FloorPlanDto> StudioPlans { get; set; }
        public IList<FloorPlanDto> OneBedPlans { get; set; }
        public IList<FloorPlanDto> TwoBedPlans { get; set; }
    }
}