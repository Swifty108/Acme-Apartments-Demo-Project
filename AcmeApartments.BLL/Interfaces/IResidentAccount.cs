using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Models;
using System.Threading.Tasks;

namespace PeachGroveApartments.ApplicationLayer.Interfaces
{
    public interface IResidentAccount
    {
        public Task<PaymentsViewModel> GetBills(AptUser user);

        public Task AddReview(Review review);
    }
}