using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class ApplicantController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IWebUserService _webUserService;
        private readonly IMapper _mapper;

        public ApplicantController(
            IApplicationService applicationService,
            IWebUserService webUserService,
            IMapper mapper)
        {
            _webUserService = webUserService;
            _applicationService = applicationService;
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
            var userId = _webUserService.GetUserId();
            var userApplications = _applicationService.GetApplications(userId);
            return View(userApplications);
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            ViewBag.ContactUsSuccess = TempData["ContactUsSuccess"];
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(ApplicantContactBindingModel applicantContanctBindingModel)
        {
            if (!ModelState.IsValid)
            {
                var applicantContactViewModel = _mapper.Map<ApplicantContactViewModel>(applicantContanctBindingModel);
                return View(applicantContactViewModel);
            }

            TempData["ContactUsSuccess"] = true;
            return RedirectToAction("ContactUs");
        }
    }
}