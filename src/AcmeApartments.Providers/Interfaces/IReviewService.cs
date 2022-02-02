using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Providers.DTOs;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Interfaces
{
    public interface IReviewService
    {
        Task<bool> AddReview(ReviewViewModelDto review, AptUser user);
    }
}
