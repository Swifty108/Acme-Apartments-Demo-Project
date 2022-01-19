using AcmeApartments.BLL.Interfaces;
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
        private readonly IMapper _mapper;

        public ApplicantController(
            IApplicationService applicationService,
            IMapper mapper)
        {
            _applicationService = applicationService;
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
        public IActionResult ShowApplications()
        {
            var userApplications = _applicationService.GetApplications(string.Empty);

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
            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                return RedirectToAction("ContactUs");
            }

            var applicantContactViewModel = _mapper.Map<ApplicantContactViewModel>(applicantContanctBindingModel);

            return View(applicantContactViewModel);
        }
    }
}