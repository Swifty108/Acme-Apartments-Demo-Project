using AcmeApartments.Common.DTOs;

namespace AcmeApartments.Web.ViewModels
{
    public class PaymentsViewModel
    {
        public WaterBillDTO WaterBill { get; set; }
        public ElectricBillDTO ElectricBill { get; set; }
        public AptUserDTO AptUser { get; set; }
    }
}