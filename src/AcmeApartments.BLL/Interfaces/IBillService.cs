using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IBillService
    {
        Task<PaymentsViewModelDTO> GetBills(AptUser user);
    }
}