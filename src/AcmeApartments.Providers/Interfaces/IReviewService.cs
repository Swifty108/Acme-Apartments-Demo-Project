using AcmeApartments.Providers.DTOs;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Interfaces
{
    public interface IReviewService
    {
        Task AddReview(ReviewViewModelDto review);
    }
}
