using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.HelperClasses;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Index(bool isApplySuccess = false)
        {
            if (isApplySuccess)
                ViewBag.ApplySuccess = isApplySuccess;

            return View();
        }

        public async Task<IActionResult> ShowApplications()
        {
            var apps = await _context.Applications.Where(u => u.AptUserId == _userManager.GetUserAsync(User).Result.Id).ToListAsync();
            return View(apps);
        }

        public IActionResult SubmitMaintenanceRequest()
        {
            ViewBag.MaintenanceSuccess = TempData["MaintenanceSuccess"];

            return View(new MaintenanceRequestViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitMaintenanceRequest(MaintenanceRequestViewModel maintReqViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    var maintReq = new MaintenanceRequest { AptUser = user, DateRequested = DateTime.Now, isAllowedToEnter = maintReqViewModel.isAllowedToEnter, ProblemDescription = maintReqViewModel.ProblemDescription, Status = MaintenanceRequestStatus.PENDINGAPPROVAL };
                    await _context.MaintenanceRequests.AddAsync(maintReq);
                    await _context.SaveChangesAsync();

                    TempData["MaintenanceSuccess"] = true;

                    return RedirectToAction("SubmitMaintenanceRequest");
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
            return Json(new
            {
                list = maintReqs
            });
        }

        public async Task<IActionResult> ShowPayments()
        {
            var user = await _userManager.GetUserAsync(User);

            var waterBill = await _context.WaterBills.FirstOrDefaultAsync();
            var electricBill = await _context.ElectricBills.FirstOrDefaultAsync();
            var newWaterBill = new WaterBill();
            var newElectricBill = new ElectricBill();

            if (waterBill == null)
            {
                newWaterBill = new WaterBill { AptUser = user, Amount = 42.53M, DateDue = DateTime.Now.AddDays(20) };
                await _context.AddAsync(newWaterBill);
                await _context.SaveChangesAsync();
            }

            if (electricBill == null)
            {
                newElectricBill = new ElectricBill { AptUser = user, Amount = 96.53M, DateDue = DateTime.Now.AddDays(20) };
                await _context.AddAsync(newElectricBill);
                await _context.SaveChangesAsync();
            }

            var app = await _context.Applications.Where(u => u.AptUserId == user.Id).FirstOrDefaultAsync();

            var payViewModel = new PaymentsViewModel
            {
                AptUser = user,
                WaterBill = waterBill ?? newWaterBill,
                ElectricBill = electricBill ?? newElectricBill
            };

            return View(payViewModel);
        }

        public IActionResult WriteAReview()
        {
            ViewBag.ReviewSuccess = TempData["ReviewSuccess"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteAReview(ReviewViewModel review)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var newReview = new Review { AptUser = user, DateReviewed = DateTime.Now, ReviewText = review.ReviewText };
                await _context.AddAsync(newReview);
                await _context.SaveChangesAsync();

                TempData["ReviewSuccess"] = true;
                return RedirectToAction("WriteAReview");
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