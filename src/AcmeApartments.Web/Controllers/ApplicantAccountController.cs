using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcmeApartments.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class ApplicantAccountController : Controller
    {
        private readonly IApplicantAccount _applicantAccountLogic;

        public ApplicantAccountController(IApplicantAccount applicantAccountLogic)
        {
            _applicantAccountLogic = applicantAccountLogic;
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
            var userApplications = _applicantAccountLogic.GetApplications();

            return View(userApplications);
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
                return RedirectToAction("ContactUs");
            }
            return View(viewMessage);
        }
    }
}