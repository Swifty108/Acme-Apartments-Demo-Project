using Peach_Grove_Apartments_Demo_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeachGroveApartments.Common.ViewModels
{
    public class FloorPlansViewModel
    {
        public IList<FloorPlan> StudioPlans { get; set; }
        public IList<FloorPlan> OneBedPlans { get; set; }
        public IList<FloorPlan> TwoBedPlans { get; set; }
    }
}