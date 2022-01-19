using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Services
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

        public async Task AddReview(ReviewViewModelDTO review)
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
