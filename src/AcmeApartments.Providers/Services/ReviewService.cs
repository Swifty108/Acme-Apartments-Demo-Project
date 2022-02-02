using AcmeApartments.Providers.DTOs;
using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Data.Provider.Interfaces;
using AcmeApartments.Data.Provider.Entities;
using System;
using System.Threading.Tasks;
using AcmeApartments.Data.Provider.Identity;

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

        public async Task<bool> AddReview(ReviewViewModelDto review, AptUser user)
        {
            try
            { 
                var newReview = new Review
                {
                    User = user,
                    DateReviewed = DateTime.Now,
                    ReviewText = review.ReviewText
                };
                await _unitOfWork.ReviewRepository.Insert(newReview);
                await _unitOfWork.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
            
        }
    }
}
