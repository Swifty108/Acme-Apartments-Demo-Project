using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Models
{
    public class PaymentsViewModel
    {
        public WaterBill WaterBill { get; set; }
        public ElectricBill ElectricBill { get; set; }
        public Application Application { get; set; }
    }
}
