using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Controllers
{
    [Authorize(Roles = "Resident")]
    public class ResidentAccountController : Controller
    {
        private readonly IResidentAccount _residentAccountLogic;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ResidentAccountController(
            IMapper mapper,
            IResidentAccount residentAccountLogic,
            IUserService userService)
        {
            _residentAccountLogic = residentAccountLogic;
            _userService = userService;
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
        {
            var applications = _residentAccountLogic.GetApplications();
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
                    _residentAccountLogic.SubmitMaintenanceRequest(maintenanceRequestDTO);

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
                _residentAccountLogic.AddReview(reviewViewModelDTO);

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