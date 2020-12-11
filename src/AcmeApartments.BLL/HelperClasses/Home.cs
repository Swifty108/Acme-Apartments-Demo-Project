using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class Home : IHome
    {
        private readonly IRepository<FloorPlan> _floorPlanRepository;
        private readonly IRepository<Application> _appRepository;
        private readonly IUserService _userService;
        private readonly UserManager<AptUser> _userManager;

        public Home(
            IHomeRepository homeRepository,
            IHttpContextAccessor accessor,
            IUserService userService,
            UserManager<AptUser> userManager,
            IRepository<FloorPlan> floorPlanRepository,
            IRepository<Application> appRepository)
        {
            _floorPlanRepository = floorPlanRepository;
            _userManager = userManager;
            _userService = userService;
            _appRepository = appRepository;
        }

        public FloorPlansViewModelDTO GetFloorPlans()
        {
            var studioFloorPlans = _floorPlanRepository.Get(filter: f => f.FloorPlanType == "Studio").ToList();
            var oneBedFloorPlans = _floorPlanRepository.Get(filter: f => f.FloorPlanType == "1Bed").ToList();
            var twoBedFloorPlans = _floorPlanRepository.Get(filter: f => f.FloorPlanType == "2Bed").ToList();

            var floorPlans = new FloorPlansViewModelDTO
            {
                StudioPlans = studioFloorPlans,
                OneBedPlans = oneBedFloorPlans,
                TwoBedPlans = twoBedFloorPlans
            };

            return floorPlans;
        }

        public async Task<string> Apply(ApplyViewModelDTO applyViewModelDTO)
        {
            var user = await _userService.GetUser();
            var app = new Application
            {
                AptUser = user,
                Income = applyViewModelDTO.Income,
                Occupation = applyViewModelDTO.Occupation,
                Price = applyViewModelDTO.Price,
                ReasonForMoving = applyViewModelDTO.ReasonForMoving,
                AptNumber = applyViewModelDTO.AptNumber,
                Area = applyViewModelDTO.Area,
                DateApplied = DateTime.Now,
                SSN = applyViewModelDTO.SSN
            };
            _appRepository.Insert(app);

            var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            return userRole;
        }
    }
}