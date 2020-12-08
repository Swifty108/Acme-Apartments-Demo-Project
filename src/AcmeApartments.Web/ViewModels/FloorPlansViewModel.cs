using AcmeApartments.Common.DTOs;
using System.Collections.Generic;

namespace AcmeApartments.Web.ViewModels
{
    public class FloorPlansViewModel
    {
        public IList<FloorPlanDTO> StudioPlans { get; set; }
        public IList<FloorPlanDTO> OneBedPlans { get; set; }
        public IList<FloorPlanDTO> TwoBedPlans { get; set; }
        public string FloorPlanType { get; set; }
    }
}