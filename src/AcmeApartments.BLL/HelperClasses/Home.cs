using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    internal class Home : IHome
    {
        private readonly IHomeRepository _homeRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserService _userService;
        private readonly UserManager<AptUser> _userManager;

        public Home(
            IHomeRepository homeRepository,
            IMapper mapper,
            IHttpContextAccessor accessor,
            IUserService userService,
            UserManager<AptUser> userManager)
        {
            _homeRepository = homeRepository;
            _accessor = accessor;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<FloorPlansViewModelDTO> GetFloorPlans()
        {
            var floorPlans = await _homeRepository.GetFloorPlans();
            return floorPlans;
        }

        public async Task<string> Apply(ApplicationDTO applicationDTO)
        {
            var user = await _userService.GetUser();
            var app = new Application
            {
                AptUser = user,
                Income = applicationDTO.Income,
                Occupation = applicationDTO.Occupation,
                Price = applicationDTO.Price,
                ReasonForMoving = applicationDTO.ReasonForMoving,
                AptNumber = applicationDTO.AptNumber,
                Area = applicationDTO.Area,
                DateApplied = DateTime.Now,
                SSN = applicationDTO.SSN
            };
            _homeRepository.AddApplication(app);

            var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            return userRole;
        }
    }
}