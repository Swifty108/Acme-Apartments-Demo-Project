using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Models;
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

        public HomeController(ILogger<HomeController> logger,
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
        public async Task<IActionResult> ShowFloorPlans(string floorPlanType = null)
        {
            var floorPlansViewModelDTO = await _homeAccountLogic.GetFloorPlans();
            var floorPlansViewModel = _mapper.Map<FloorPlansViewModel>(floorPlansViewModelDTO);
            floorPlansViewModel.FloorPlanType = floorPlanType;
            return View(floorPlansViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpGet]
        public async Task<IActionResult> Apply([Bind("AptNumber, Price, Area, FloorPlanType")] ApplyViewModel applyViewModel)
        {
            ModelState.Clear();
            var user = await _userService.GetUser();
            applyViewModel.User = user;

            return View(applyViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpPost]
        public async Task<IActionResult> ApplyPost(ApplyViewModel applyViewModel)
        {
            if (ModelState.IsValid)
            {
                var applyViewModelDTO = _mapper.Map<ApplyViewModelDTO>(applyViewModel);
                var userRole = await _homeAccountLogic.Apply(applyViewModelDTO);

                return RedirectToAction("index", $"{userRole}account", new { IsApplySuccess = true });
            }

            return RedirectToAction("Apply", applyViewModel.AptNumber, applyViewModel.Price);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            ViewBag.ContactUsSuccess = TempData["ContactUsSuccess"];
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(AppUserContactViewModel viewMessage)
        {
            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                return RedirectToAction("ContactUs");
            }
            return View(viewMessage);
        }
    }
}