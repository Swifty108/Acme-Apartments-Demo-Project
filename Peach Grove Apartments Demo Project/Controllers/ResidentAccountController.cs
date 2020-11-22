using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeachGroveApartments.ApplicationLayer.Interfaces;
using PeachGroveApartments.ApplicationLayer.ViewModels;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    [Authorize(Roles = "Resident")]
    public class ResidentAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AptUser> _userManager;
        private readonly IResidentRepository _residentRepository;
        private readonly IResidentLogic _residentLogic;

        public ResidentAccountController(ApplicationDbContext context, UserManager<AptUser> userManager, IResidentRepository residentRepository, IResidentLogic residentLogic)
        {
            _context = context;
            _userManager = userManager;
            _residentRepository = residentRepository;
            _residentLogic = residentLogic;
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
            var userId = _userManager.GetUserAsync(User).Result.Id;

            return View(await _residentRepository.GetApplications(userId));
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
                    var maintenanceRequest = new MaintenanceRequest { AptUser = user, DateRequested = DateTime.Now, isAllowedToEnter = maintReqViewModel.isAllowedToEnter, ProblemDescription = maintReqViewModel.ProblemDescription, Status = MaintenanceRequestStatus.PENDINGAPPROVAL };

                    await _residentRepository.AddMaintenanceRequest(maintenanceRequest);

                    TempData["MaintenanceSuccess"] = true;

                    return RedirectToAction("SubmitMaintenanceRequest");
                }
                catch (Exception)
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
            //todo-p: get this out

            //var user = await _userManager.GetUserAsync(User);
            //var maintReqs = await _context.MaintenanceRequests.Where(a => a.AptUserId == user.Id).ToListAsync();

            var maintenanceRequests = await _residentRepository.GetMaintenanceUserRequests();
            return Json(new
            {
                list = maintenanceRequests
            });
        }

        public async Task<IActionResult> ShowPayments()
        {
            var user = await _userManager.GetUserAsync(User);

            var payViewModel = await _residentLogic.GetBills(user);

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

                await _residentLogic.AddReview(newReview);

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