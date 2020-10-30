using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private readonly UserManager<AptUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<AptUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Amenities()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }
        public async Task<IActionResult> FloorPlans()
        {
           var sPlans = await _context.FloorPlans.Where(f => f.FloorPlanType == "Studio").ToListAsync();
           var oneBedPlans = await _context.FloorPlans.Where(f => f.FloorPlanType == "1Bed").ToListAsync();
           var twoBedPlans = await _context.FloorPlans.Where(f => f.FloorPlanType == "2Bed").ToListAsync();

            var list = new FloorPlansViewModel
            {
                StudioPlans = sPlans,
                OneBedPlans = oneBedPlans,
                TwoBedPlans = twoBedPlans
            };
           
            return View(list);
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet]
        public async Task<IActionResult> Apply(string aptNumber, string price, string area )
        {
            var appViewModel = new ApplyViewModel();
                       var user = await _userManager.GetUserAsync(User);
                    appViewModel.AptNumber = aptNumber;
                    appViewModel.Price = price;
                    appViewModel.User = user;
                    appViewModel.Area = area;

            return View(appViewModel);
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyViewModel applicationViewModel)
        {
           var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
                {
                    var app = new Application { AptUser = user, Income = applicationViewModel.Income, Occupation = applicationViewModel.Occupation, Price = applicationViewModel.Price, ReasonForMoving = applicationViewModel.ReasonForMoving, AptNumber = applicationViewModel.AptNumber, Area = applicationViewModel.Area, DateApplied = DateTime.Now, SSN = applicationViewModel.SSN };
                    _db.Add(app);

                    //await _db.AddAsync(app);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            return RedirectToAction("Apply", applicationViewModel.AptNumber, applicationViewModel.Price);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ContactUs()
        {
            ViewBag.ContactUsSuccess = TempData["ContactUsSuccess"];
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(AppUserContactViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                return RedirectToAction("ContactUs");
            }
            return View(viewModel);
        }
    }
}
