using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Models;

namespace PeachGroveApartments.ApplicationLayer.ViewModels
{
    public class PaymentsViewModel
    {
        public WaterBill WaterBill { get; set; }
        public ElectricBill ElectricBill { get; set; }
        public AptUser AptUser { get; set; }
    }
}