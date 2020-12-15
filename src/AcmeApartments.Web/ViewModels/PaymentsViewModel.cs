using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;

namespace AcmeApartments.Web.ViewModels
{
    public class PaymentsViewModel
    {
        public WaterBill WaterBill { get; set; }
        public ElectricBill ElectricBill { get; set; }
        public AptUser User { get; set; }
    }
}