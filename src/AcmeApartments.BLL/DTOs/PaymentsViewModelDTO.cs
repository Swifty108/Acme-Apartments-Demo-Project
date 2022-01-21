using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Models;

namespace AcmeApartments.BLL.DTOs
{
    public class PaymentsViewModelDto
    {
        public WaterBill WaterBill { get; set; }
        public ElectricBill ElectricBill { get; set; }
        public AptUser User { get; set; }
    }
}