using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Data;
using AcmeApartments.DAL.Identity;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Controllers
{
    [Authorize(Roles = "Resident")]
    public class ResidentAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AptUser> _userManager;
        private readonly IResidentAccount _residentAccountLogic;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;

        public ResidentAccountController(
            ApplicationDbContext context,
            UserManager<AptUser> userManager,
            IResidentAccount residentLogic,
            IMapper mapper,
            IUserService userService,
            IApplicationService applicationService)
        {
            _context = context;
            _userManager = userManager;
            _userService = userService;
            _applicationService = applicationService;
            _residentAccountLogic = residentLogic;

            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(bool isApplySuccess = false)
        {
            if (isApplySuccess)
                ViewBag.ApplySuccess = isApplySuccess;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowApplications()
        {//dont use resultl.Id use await
            var userId = _userService.GetUserId();
            var applications = await _applicationService.GetApplications(userId);

            return View(applications);
        }

        [HttpGet]
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
                    var maintenanceRequestDTO = _mapper.Map<MaintenanceRequestDTO>(maintReqViewModel);
                    await _residentAccountLogic.SubmitMaintenanceRequest(maintenanceRequestDTO);

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
            var maintenanceRequests = await _residentAccountLogic.GetMaintenanceRequests();
            return Json(new
            {
                list = maintenanceRequests
            });
        }

        [HttpGet]
        public async Task<IActionResult> ShowPayments()
        {
            var user = await _userService.GetUser();
            var payments = await _residentAccountLogic.GetBills(user);
            var payViewModel = _mapper.Map<PaymentsViewModel>(payments);

            return View(payViewModel);
        }

        [HttpGet]
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
                var reviewViewModelDTO = _mapper.Map<ReviewViewModelDTO>(review);
                await _residentAccountLogic.AddReview(reviewViewModelDTO);

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
        public IActionResult ContactUs(ApplicantContactViewModel viewMessage)
        {
            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                // await _emailService.SendEmailAsync(viewMessage);

                return RedirectToAction("ContactUs");
            }
            return View(viewMessage);
        }
    }
}