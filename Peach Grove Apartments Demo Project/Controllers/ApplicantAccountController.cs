using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeachGroveApartments.ApplicationLayer.ViewModels;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class ApplicantAccountController : Controller
    {
        private readonly UserManager<AptUser> _userManager;
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantAccountController(UserManager<AptUser> userManager, IApplicantRepository applicantRepository)
        {
            _userManager = userManager;
            _applicantRepository = applicantRepository;
        }

        // GET: ApplicantAccount
        public async Task<IActionResult> Index(bool isApplySuccess = false)
        {
            if (isApplySuccess)
                ViewBag.ApplySuccess = isApplySuccess;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowApplications()
        {
            var userId = _userManager.GetUserAsync(User).Result.Id;

            return View(await _applicantRepository.GetApplications(userId));
        }

        // GET: ApplicantAccount/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var application = await _applicantRepository.GetApplication(id);
            return View(application);
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            ViewBag.ContactUsSuccess = TempData["ContactUsSuccess"];
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(ApplicantContactViewModel appContactViewModel)
        {
            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                return RedirectToAction("ContactUs");
            }
            return View(appContactViewModel);
        }
    }
}