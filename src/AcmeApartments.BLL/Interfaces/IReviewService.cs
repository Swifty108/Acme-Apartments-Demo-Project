using AcmeApartments.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Interfaces
{
    public interface IReviewService
    {
        Task AddReview(ReviewViewModelDto review);
    }
}
