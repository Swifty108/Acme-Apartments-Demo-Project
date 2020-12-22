using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcmeApartments.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class ApplicantAccountController : Controller
    {
        private readonly IApplicantAccount _applicantAccountLogic;
        private readonly IMapper _mapper;

        public ApplicantAccountController(
            IApplicantAccount applicantAccountLogic,
            IMapper mapper)
        {
            _applicantAccountLogic = applicantAccountLogic;
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
            var userApplications = await _applicantAccountLogic.GetApplications();

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