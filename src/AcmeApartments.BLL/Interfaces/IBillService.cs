using AcmeApartments.BLL.DTOs;
using AcmeApartments.DAL.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IBillService
    {
        Task<PaymentsViewModelDto> GetBills(AptUser user);
    }
}