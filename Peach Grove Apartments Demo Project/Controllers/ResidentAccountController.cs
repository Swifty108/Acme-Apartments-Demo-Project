using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    [Authorize(Roles = "Resident")]
    public class ResidentAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AptUser> _userManager;

        public ResidentAccountController(ApplicationDbContext context, UserManager<AptUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AppUserAccount
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Maintenance()
        {
 
            ViewBag.MaintenanceSuccess = TempData["MaintenanceSuccess"];

            return View(new MaintenanceRequestViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> Maintenance(MaintenanceRequestViewModel maintReqViewModel)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    
                    var user = await _userManager.GetUserAsync(User);
                    var maintReq = new MaintenanceRequest { AptUser = user, DateRequested = DateTime.Now, isAllowedToEnter = maintReqViewModel.isAllowedToEnter, ProblemDescription = maintReqViewModel.ProblemDescription };
                    await _context.MaintenanceRequests.AddAsync(maintReq);
                    await _context.SaveChangesAsync();

                    TempData["MaintenanceSuccess"] = true;

                    return RedirectToAction("Maintenance");

                }
                catch (Exception e)
                {
                    TempData["MaintenanceSuccess"] = false;
                    return View(maintReqViewModel);
                }

            }

            return View(maintReqViewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetReqHistory()
        {
           var user = await _userManager.GetUserAsync(User);
           var maintReqs = await _context.MaintenanceRequests.Where(a => a.AptUserId == user.Id).ToListAsync();

            // var result = JsonConvert.SerializeObject(maintReqs);

            // var maintReqViewModel = new MaintenanceReqHistoryViewModel { Requests = maintReqs };



            // TempData["SuccessMessage"] = "";


            return Json(new
            {
                list = maintReqs
            });

        }


        public async Task<IActionResult> Payments()
        {
            var user = await _userManager.GetUserAsync(User);

            var waterBill = await _context.WaterBills.FirstOrDefaultAsync();
            var electricBill = await _context.ElectricBills.FirstOrDefaultAsync();
            var wbill = new WaterBill();
            var ebill = new ElectricBill();

            if (waterBill == null)
            {
                wbill = new WaterBill { AptUser = user, Amount = 42.53M, DateDue = DateTime.Now.AddDays(20) };
                await _context.AddAsync(wbill);
                await _context.SaveChangesAsync();
            }

            if (electricBill == null)
            {
                ebill = new ElectricBill { AptUser = user, Amount = 96.53M, DateDue = DateTime.Now.AddDays(20) };
                await _context.AddAsync(ebill);
                await _context.SaveChangesAsync();
            }

            var app = await _context.Applications.Where(u => u.AptUserId == user.Id).FirstOrDefaultAsync();


            var payViewModel = new PaymentsViewModel
            {
                AptUser = user,
                WaterBill = waterBill ?? wbill,
                ElectricBill = electricBill ?? ebill
            };

            return View(payViewModel);
        }

        public IActionResult WriteReview()
        {
            ViewBag.ReviewSuccess = TempData["ReviewSuccess"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteReview(ReviewViewModel review)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var newReview = new Review { AptUser = user, DateReviewed = DateTime.Now, ReviewText = review.ReviewText  };
                await _context.AddAsync(newReview);
                await _context.SaveChangesAsync();

                TempData["ReviewSuccess"] = true;
                return RedirectToAction("WriteReview");

            }
            return View();
        }

        [HttpGet]

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
