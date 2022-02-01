using AcmeApartments.Providers.DTOs;
using AcmeApartments.Data.Provider.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Interfaces
{
    public interface IBillService
    {
        Task<PaymentsViewModelDto> GetBills(AptUser user);
    }
}