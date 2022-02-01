using AcmeApartments.Providers.DTOs;
using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Data.Provider.Interfaces;
using AcmeApartments.Data.Provider.Entities;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.Providers.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddReview(ReviewViewModelDto review)
        {
            var user = await _userService.GetUser();
            var newReview = new Review
            {
                User = user,
                DateReviewed = DateTime.Now,
                ReviewText = review.ReviewText
            };
            await _unitOfWork.ReviewRepository.Insert(newReview);
            await _unitOfWork.Save();
        }
    }
}
