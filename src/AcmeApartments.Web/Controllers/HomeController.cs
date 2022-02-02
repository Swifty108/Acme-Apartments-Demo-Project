using AcmeApartments.Providers.DTOs;
using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Data.Provider.Entities;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AcmeApartments.Data.Provider.Identity;

namespace AcmeApartments.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebUserService _webUserService;
        private readonly UserManager<AptUser> _userManager;
        private readonly IApplicationService _applicationService;
        private readonly IFloorPlanService _floorPlanService;
        public HomeController(
            IWebUserService webUserService,
            IApplicationService applicationService,
            IFloorPlanService floorPlansService,
            UserManager<AptUser> userManager,
            IMapper mapper) 
        {
            _userManager = userManager;
            _webUserService = webUserService;
            _floorPlanService = floorPlansService;
            _applicationService = applicationService;
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
            var floorPlansViewModelDTO = await _floorPlanService.GetFloorPlans();
            var floorPlansViewModel = _mapper.Map<FloorPlansViewModel>(floorPlansViewModelDTO);
            return View(floorPlansViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpGet]
        public async Task<IActionResult> Apply(ApplyReturnUrlBindingModel applyReturnUrlBindingModel)
        {
            var user = await _webUserService.GetUser();
            var isAppliationExists = await _applicationService.CheckifApplicationExists(applyReturnUrlBindingModel.AptNumber, user);

            if (isAppliationExists)
            {
                return RedirectToAction("ShowApplicationAlreadyExistsError");
            }

            ModelState.Clear();

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
                Zipcode = user.Zipcode,
                SSN = user.SSN
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

            var applyModelDTO = _mapper.Map<ApplyModelDto>(applyBindingModel);
            var user = await _webUserService.GetUser();
            await _applicationService.Apply(applyModelDTO, user);

            var userRole = await _userManager.GetRolesAsync(user);

            return RedirectToAction("index", $"{userRole[0]}", new { IsApplySuccess = true });
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
            if (!ModelState.IsValid)
            {
                var appUserContactViewModel = _mapper.Map<AppUserContactViewModel>(appUserContactBindingModel);

                return View(appUserContactViewModel);
            }

            TempData["ContactUsSuccess"] = true;
            return RedirectToAction("ContactUs");
        }
        
    }
}