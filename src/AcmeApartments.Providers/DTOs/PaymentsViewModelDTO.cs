using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Data.Provider.Entities;

namespace AcmeApartments.Providers.DTOs
{
    public class PaymentsViewModelDto
    {
        public WaterBill WaterBill { get; set; }
        public ElectricBill ElectricBill { get; set; }
        public AptUser User { get; set; }
    }
}