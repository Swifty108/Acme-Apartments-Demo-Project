using AcmeApartments.Providers.DTOs;
using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Controllers
{
    [Authorize(Roles = "Resident")]
    public class ResidentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IBillService _billService;
        private readonly IReviewService _reviewService;

        public ResidentController(
            IMapper mapper,
            IUserService userService,
            IMaintenanceService maintenanceService,
            IApplicationService applicationService,
            IBillService billService,
            IReviewService reviewService)
        {
            _userService = userService;
            _applicationService = applicationService;
            _billService = billService;
            _reviewService = reviewService;
            _maintenanceService = maintenanceService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(bool isApplySuccess = false)
        {
            ViewBag.ApplySuccess = isApplySuccess;

            return View();
        }

        [HttpGet]
        public IActionResult ShowApplications()
        {
            var applications = _applicationService.GetApplications(string.Empty);
            return View(applications);
        }

        [HttpGet]
        public IActionResult SubmitMaintenanceRequest()
        {
            ViewBag.MaintenanceSuccess = TempData["MaintenanceSuccess"];

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitMaintenanceRequest(NewMaintenanceRequestBindingModel newMaintRequestBindingModel)
        {
            if (!ModelState.IsValid)
            {
                var newMaintReqViewModel = _mapper.Map<NewMaintenanceRequestViewModel>(newMaintRequestBindingModel);
                return View(newMaintReqViewModel);
            }

            var maintenanceRequestDTO = _mapper.Map<NewMaintenanceRequestDto>(newMaintRequestBindingModel);
            var isSubmitSuccess = await _maintenanceService.SubmitMaintenanceRequest(maintenanceRequestDTO);

            if (isSubmitSuccess)
            {
                TempData["MaintenanceSuccess"] = true;
            }
            else
            {
                TempData["MaintenanceSuccess"] = false;
            }
                
            return RedirectToAction("SubmitMaintenanceRequest");
        }

        [HttpGet]
        public async Task<JsonResult> GetReqHistory()
        {
            var maintenanceRequests = await _maintenanceService.GetMaintenanceRequests();
            return Json(new
            {
                list = maintenanceRequests
            });
        }

        [HttpGet]
        public async Task<IActionResult> ShowPayments()
        {
            var user = await _userService.GetUser();
            var payments = await _billService.GetBills(user);
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
        public async Task<IActionResult> WriteAReview(ReviewBindingModel reviewBindingModel)
        {
            if (!ModelState.IsValid)
            {
                var reviewViewModel = _mapper.Map<ReviewViewModel>(reviewBindingModel);
                return View(reviewViewModel);
            }

            var reviewViewModelDTO = _mapper.Map<ReviewViewModelDto>(reviewBindingModel);
            var isAddReviewSuccess = await _reviewService.AddReview(reviewViewModelDTO);

            if(isAddReviewSuccess)
            {
                TempData["ReviewSuccess"] = true;
            }
            else
            {
                TempData["ReviewFailure"] = true;
            }

            return RedirectToAction("WriteAReview");
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            ViewBag.ContactUsSuccess = TempData["ContactUsSuccess"];
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(ResidentContactBindingModel residentContanctBindingModel)
        {
            if (!ModelState.IsValid)
            {
                var residentContactViewModel = _mapper.Map<ResidentContactViewModel>(residentContanctBindingModel);
                return View(residentContactViewModel);
            }

            TempData["ContactUsSuccess"] = true;
            return RedirectToAction("ContactUs");
        }
    }
}