﻿using AcmeApartments.BLL.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AptUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IHttpContextAccessor accessor,
            UserManager<AptUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _accessor = accessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<AptUser> GetUser()
        {
            var userClaimsPrincipal = _accessor?.HttpContext.User;
            var user = await _userManager.GetUserAsync(userClaimsPrincipal);
            return user;
        }

        public async Task<AptUser> GetUserByID(string userId)
        {
            var user = await _unitOfWork.AptUserRepository.GetByID(userId);
            return user;
        }

        public string GetUserId()
        {
            return _userManager.GetUserId(_accessor?.HttpContext.User);
        }
    }
}