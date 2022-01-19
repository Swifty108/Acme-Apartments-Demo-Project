﻿using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.DTOs;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.HelperClasses
{
    public class Home : IHome
    {
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;
        private readonly UserManager<AptUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public Home(
            IUserService userService,
            UserManager<AptUser> userManager,
            IUnitOfWork unitOfWork,
            IApplicationService applicationService)
        {
            _userManager = userManager;
            _userService = userService;
            _applicationService = applicationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<FloorPlansViewModelDTO> GetFloorPlans()
        {
            var studioFloorPlans = await _unitOfWork.FloorPlanRepository.Get(filter: f => f.FloorPlanType == "Studio").ToListAsync();
            var oneBedFloorPlans = await _unitOfWork.FloorPlanRepository.Get(filter: f => f.FloorPlanType == "1Bed").ToListAsync();
            var twoBedFloorPlans = await _unitOfWork.FloorPlanRepository.Get(filter: f => f.FloorPlanType == "2Bed").ToListAsync();

            var floorPlans = new FloorPlansViewModelDTO
            {
                StudioPlans = studioFloorPlans,
                OneBedPlans = oneBedFloorPlans,
                TwoBedPlans = twoBedFloorPlans
            };

            return floorPlans;
        }

        public async Task<bool> CheckifApplicationExists(string aptNumber)
        {
            var apps = await _applicationService.GetApplicationsByAptNumber(aptNumber);
            return apps.Count > 0 ? true : false;
        }

        public async Task<string> Apply(ApplyViewModelDTO applyViewModelDTO)
        {
            var user = await _userService.GetUser();
            var app = new Application
            {
                AptUserId = user.Id,
                Income = applyViewModelDTO.Income,
                Occupation = applyViewModelDTO.Occupation,
                Price = applyViewModelDTO.Price,
                ReasonForMoving = applyViewModelDTO.ReasonForMoving,
                AptNumber = applyViewModelDTO.AptNumber,
                Area = applyViewModelDTO.Area,
                DateApplied = DateTime.Now,
                SSN = applyViewModelDTO.SSN
            };
            await _unitOfWork.ApplicationRepository.Insert(app);
            await _unitOfWork.Save();

            var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            return userRole;
        }
    }
}