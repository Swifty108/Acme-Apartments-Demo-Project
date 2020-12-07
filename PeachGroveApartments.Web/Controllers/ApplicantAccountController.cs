using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeachGroveApartments.ApplicationLayer.ViewModels;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Inerfaces;
using PeachGroveApartments.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    [Authorize(Roles = "Applicant")]
    public class ApplicantAccountController : Controller
    {
        private readonly UserManager<AptUser> _userManager;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMailService _emailService;

        public ApplicantAccountController(UserManager<AptUser> userManager, IApplicantRepository applicantRepository, IMailService emailService)
        {
            _userManager = userManager;
            _applicantRepository = applicantRepository;
            _emailService = emailService;
        }

        // GET: ApplicantAccount
        //Todo-p use [HTTPGet] attribute here 
        public IActionResult Index(bool isApplySuccess = false)
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
        //Todo-p use [HTTPDelete] attribute here 
        //todo-p addrroute attribute here as alias afte rchanging method name to ShowDetails()
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
        public IActionResult ContactUs(ApplicantContactViewModel viewMessage)
        {
            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                //await _emailService.SendEmailAsync(viewMessage);

                return RedirectToAction("ContactUs");
            }
            return View(viewMessage);
        }
    }
}