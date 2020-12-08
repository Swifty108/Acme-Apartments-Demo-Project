using AcmeApartments.DAL.Models;

namespace AcmeApartments.BLL.HelperClasses
{
    public class PaymentsViewModelDTO
    {
        public WaterBill WaterBill { get; set; }
        public ElectricBill ElectricBill { get; set; }
        public AptUser AptUser { get; set; }
    }
}