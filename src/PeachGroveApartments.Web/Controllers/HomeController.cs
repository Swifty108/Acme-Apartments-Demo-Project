using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeachGroveApartments.ApplicationLayer.ViewModels;
using PeachGroveApartments.Core.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AptUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHomeRepository _homeRepository;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext db,
            UserManager<AptUser> userManager,
            ApplicationDbContext context,
            IHomeRepository homeRepository,
            IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _homeRepository = homeRepository;
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
            var floorPlans = await _homeRepository.GetFloorPlans();
            var floorPlansViewModel = _mapper.Map<FloorPlansViewModel>(floorPlans);
            floorPlansViewModel.FloorPlanType = floorPlanType;
            return View(floorPlansViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpGet]
        //todo-p: test with frombody
        public async Task<IActionResult> Apply([Bind("AptNumber, Price, Area, FloorPlanType")] ApplyViewModel applyViewModel)
        {
            ModelState.Clear();
            var user = await _userManager.GetUserAsync(User);
            applyViewModel.User = user;

            return View(applyViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpPost]
        public async Task<IActionResult> ApplyPost(ApplyViewModel applicationViewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var app = new Application
                {
                    AptUser = user,
                    Income = applicationViewModel.Income,
                    Occupation = applicationViewModel.Occupation,
                    Price = applicationViewModel.Price,
                    ReasonForMoving = applicationViewModel.ReasonForMoving,
                    AptNumber = applicationViewModel.AptNumber,
                    Area = applicationViewModel.Area,
                    DateApplied = DateTime.Now,
                    SSN = applicationViewModel.SSN
                };
                _context.Add(app);

                await _context.SaveChangesAsync();

                var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

                return RedirectToAction("index", $"{userRole}account", new { IsApplySuccess = true });
            }

            return RedirectToAction("Apply", applicationViewModel.AptNumber, applicationViewModel.Price);
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