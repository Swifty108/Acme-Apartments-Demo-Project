using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Entities;

namespace AcmeApartments.Web.ViewModels
{
    public class PaymentsViewModel
    {
        public WaterBill WaterBill { get; set; }
        public ElectricBill ElectricBill { get; set; }
        public AptUser User { get; set; }
    }
}