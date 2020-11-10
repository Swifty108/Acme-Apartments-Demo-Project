using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<AptUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
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
            var studioPlans = await _context.FloorPlans.Where(f => f.FloorPlanType == "Studio").ToListAsync();
            var oneBedPlans = await _context.FloorPlans.Where(f => f.FloorPlanType == "1Bed").ToListAsync();
            var twoBedPlans = await _context.FloorPlans.Where(f => f.FloorPlanType == "2Bed").ToListAsync();

            var list = new FloorPlansViewModel
            {
                StudioPlans = studioPlans,
                OneBedPlans = oneBedPlans,
                TwoBedPlans = twoBedPlans
            };

            return View(list);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpGet]
        public async Task<IActionResult> Apply(string aptNumber, string price, string area)
        {
            var appViewModel = new ApplyViewModel();
            var user = await _userManager.GetUserAsync(User);
            appViewModel.AptNumber = aptNumber;
            appViewModel.Price = price;
            appViewModel.User = user;
            appViewModel.Area = area;

            return View(appViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyViewModel applicationViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
                
            //if(_context.Applications.Where(a => a.AptNumber == applicationViewModel.AptNumber) != null)
            //{
            //    return RedirectToAction("index", "applicantaccount", new { IsApplySuccess = true });
            //}

            if (ModelState.IsValid)
            {
                var app = new Application { AptUser = user, Income = applicationViewModel.Income, Occupation = applicationViewModel.Occupation, Price = applicationViewModel.Price, ReasonForMoving = applicationViewModel.ReasonForMoving, AptNumber = applicationViewModel.AptNumber, Area = applicationViewModel.Area, DateApplied = DateTime.Now, SSN = applicationViewModel.SSN };
                _context.Add(app);

                //Memo: applied apartments should not be marked "Unavailable" after application is approved since they will all disappear from the availibility table.
                //var fp = await _context.FloorPlans.Where(f => f.AptNumber == applicationViewModel.AptNumber).FirstOrDefaultAsync();
                //fp.Status = "UnAvailable";

                //_context.FloorPlans.Update(fp);

                await _context.SaveChangesAsync();

                if (User.IsInRole("Applicant"))
                {
                    return RedirectToAction("index", "applicantaccount", new { IsApplySuccess = true });
                }
                else if (User.IsInRole("Resident"))
                {
                    return RedirectToAction("index", "residentaccount", new { IsApplySuccess = true });
                }
            }

            return RedirectToAction("Apply", applicationViewModel.AptNumber, applicationViewModel.Price);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //TODO-P: implement caching like this in other areas of the app instead of using Redis cache framework. simpler.
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