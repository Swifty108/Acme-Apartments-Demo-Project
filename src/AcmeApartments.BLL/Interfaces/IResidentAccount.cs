using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Models;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IResidentAccount
    {
        public Task<PaymentsViewModel> GetBills(AptUser user);

        public Task AddReview(Review review);
    }
}