using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Models;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHome _homeAccountLogic;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public HomeController(
            ILogger<HomeController> logger,
            IHome homeAccountLogic,
            IUserService userService,
            IMapper mapper) 
        {
            _logger = logger;
            _homeAccountLogic = homeAccountLogic;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShowAmenities()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShowGallery()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowFloorPlans()
        {
            var floorPlansViewModelDTO = await _homeAccountLogic.GetFloorPlans();
            var floorPlansViewModel = _mapper.Map<FloorPlansViewModel>(floorPlansViewModelDTO);
            return View(floorPlansViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpGet]
        public async Task<IActionResult> Apply(ApplyReturnUrlBindingModel applyReturnUrlBindingModel)
        {
            var isAppliationExists = await _homeAccountLogic.CheckifApplicationExists(applyReturnUrlBindingModel.AptNumber);

            if (isAppliationExists)
            {
                return RedirectToAction("ShowApplicationAlreadyExistsError");
            }

            ModelState.Clear();
            var user = await _userService.GetUser();

            var applyViewModel = new ApplyViewModel
            {
                FloorPlanType = applyReturnUrlBindingModel.FloorPlanType,
                Price = applyReturnUrlBindingModel.Price,
                AptNumber = applyReturnUrlBindingModel.AptNumber,
                FirstName = user.FirstName,
                Area = applyReturnUrlBindingModel.Area,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.State,
                Zipcode = user.Zipcode
            };

            return View(applyViewModel);
        }

        [HttpGet]
        public IActionResult ShowApplicationAlreadyExistsError()
        {
            ViewBag.ApplicationFoundError = true;
            return View();
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyBindingModel applyBindingModel)
        {
            if (!ModelState.IsValid)
            {
                var applyViewModel = _mapper.Map<ApplyViewModel>(applyBindingModel);

                return View(applyViewModel);
            }

            var applyViewModelDTO = _mapper.Map<ApplyViewModelDTO>(applyBindingModel);
            var userRole = await _homeAccountLogic.Apply(applyViewModelDTO);

            return RedirectToAction("index", $"{userRole}account", new { IsApplySuccess = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        [HttpGet]
        public IActionResult ContactUs()
        {
            ViewBag.ContactUsSuccess = TempData["ContactUsSuccess"];
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(AppUserContactBindingModel appUserContactBindingModel)
        {
            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                return RedirectToAction("ContactUs");
            }

            var appUserContactViewModel = _mapper.Map<AppUserContactViewModel>(appUserContactBindingModel);

            return View(appUserContactViewModel);
        }
    }
}