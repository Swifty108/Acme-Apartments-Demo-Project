using AcmeApartments.BLL.DTOs;
using System.Collections.Generic;

namespace AcmeApartments.Web.ViewModels
{
    public class FloorPlansViewModel
    {
        public IList<FloorPlanDTO> StudioPlans { get; set; }
        public IList<FloorPlanDTO> OneBedPlans { get; set; }
        public IList<FloorPlanDTO> TwoBedPlans { get; set; }
    }
}