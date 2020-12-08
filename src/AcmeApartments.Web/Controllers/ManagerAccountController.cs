using AcmeApartments.Common.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeachGroveApartments.ApplicationLayer.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerAccountController : Controller
    {
        private readonly UserManager<AptUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        private SignInManager<AptUser> _signInManager;

        // private readonly IManagerRepository _managerAccount;
        private readonly IManagerAccount _managerAccount;

        private readonly ApplicationDbContext _context;

        public ManagerAccountController(
            ApplicationDbContext context,
            UserManager<AptUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper, SignInManager<AptUser> signInManager,
            IManagerRepository managerRepository,
            IManagerAccount managerAccount, IApplicationService applicationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager = signInManager;
            //   _managerAccount = managerRepository;
            _managerAccount = managerAccount;
            _context = context;
            _applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowApplicationUsers()
        {//todo-p: put whats in paraenth into own var better for debug and readablility
            var appUsers = await _applicationService.GetApplicationUsers();
            return View(appUsers);
        }

        [HttpGet]
        public async Task<IActionResult> ShowApplications(string userId)
        {
            var apps = await _applicationService.GetApplications(userId);
            var appsViewModel = _mapper.Map<ApplicationViewModel>(apps);
            return View(apps);
        }

        [HttpGet]
        public async Task<IActionResult> ViewApplication(int applicationId)
        {
            var application = await _applicationService.GetApplication(applicationId);
            return View(application);
        }

        [HttpGet]
        public async Task<IActionResult> EditApplication(int Id)
        {//if(Id == null) {
         //         BadRequest("Id not found")
         //   }
            var application = await _managerAccount.GetApplication(Id);
            if (application == null)
            {
                return NotFound();
            }

            var app = _mapper.Map<ApplicationViewModel>(application);

            ViewBag.AppEditSuccess = TempData["AppEditSuccess"];
            return View(app);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplication(ApplicationViewModel application)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // var app = _mapper.Map<Application>(application);
                    await _managerAccount.EditApplication(application);

                    TempData["AppEditSuccess"] = true;
                    return RedirectToAction("EditApplication");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ApplicationExists(application.ApplicationId) != null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        public async Task<IActionResult> CancelApplication(int Id)
        {
            var application = await _managerAccount.GetApplication(Id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: ApplicantAccount/Delete/5
        [HttpPost, ActionName("AppCancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplicationConfirmed(Application app)
        {
            var application = await _managerAccount.CancelApplication(app.ApplicationId);

            return RedirectToAction("ShowApplications", new { userId = application.AptUserId });
        }

        public async Task<IActionResult> ApproveApplication(ApproveAppViewModel approveViewModel)
        {
            try
            {
                await _managerAccount.ApproveApplication(
                    approveViewModel.UserId,
                    approveViewModel.ApplicationId,
                    approveViewModel.SSN,
                    approveViewModel.AptNumber,
                    approveViewModel.AptPrice
                    );
            }
            catch (Exception e)
            {
                TempData["ApproveFailedMessage"] = e.Message;
                return RedirectToAction("ShowApproveApplicationFailed");
            }
            return RedirectToAction("ShowApproveApplicationSuccess");
        }

        public IActionResult ShowApproveApplicationSuccess()
        {
            return View();
        }

        public IActionResult ShowApproveApplicationFailed()
        {
            ViewBag.ApproveApplicationFailed = TempData["ApproveFailedMessage"];
            return View();
        }

        private async Task<Application> ApplicationExists(int id)
        {
            return await _managerAccount.GetApplication(id) ?? null;
        }

        public async Task<IActionResult> UnApproveApplication(string userId, string aptNumber, int applicationId)
        {
            try
            {
                await _managerAccount.UnApproveApplication(userId, aptNumber, applicationId);
            }
            catch (Exception e)
            {
                TempData["UnApproveFailedMessage"] = e.Message;
                return RedirectToAction("ShowUnApproveApplicationFailed");
            }
            return RedirectToAction("ShowUnApproveApplicationSuccess");
        }

        public IActionResult ShowUnApproveApplicationSuccess()
        {
            return View();
        }

        public IActionResult ShowUnApproveApplicationFailed()
        {
            ViewBag.UnApproveApplicationFailed = TempData["UnApproveFailedMessage"];
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowMaintenanceRequestsUsers()
        {
            return View(await _managerAccount.GetMaintenanceRequestsUsers());
        }

        public async Task<IActionResult> ShowMaintenanceRequests(string firstName, string lastName)
        {
            var MaintenanceRecords = await _managerAccount.GetMaintenanceUserRequests();

            var mViewModel = new MaintenanceRequestViewModel
            {
                mRequests = MaintenanceRecords,
                userFName = firstName,
                userLName = lastName
            };

            ViewBag.MaintenanceEditSuccess = TempData["MaintenanceEditSuccess"];
            return View(mViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ViewMaintenanceRequest(int maintenanceId, string firstName, string lastName)
        {
            var maintenanceRecord = await _managerAccount.GetMaintenanceRequest(maintenanceId);

            if (maintenanceRecord == null)
            {
                return NotFound();
            }

            return View(new MaintenanceRequestViewModel
            {
                mRequest = maintenanceRecord,
                userFName = firstName,
                userLName = lastName
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditMaintenanceRequest(int maintenanceId, string firstName, string lastName)
        {
            var maintenanceRecord = await _managerAccount.GetMaintenanceRequest(maintenanceId);

            return View(new MaintenanceRequestViewModel
            {
                Id = maintenanceRecord.Id,
                AptUserId = maintenanceRecord.AptUserId,
                ProblemDescription = maintenanceRecord.ProblemDescription,
                isAllowedToEnter = maintenanceRecord.isAllowedToEnter,
                userFName = firstName,
                userLName = lastName
            });
        }

        // POST: ApplicantAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMaintenanceRequest(MaintenanceRequestViewModel maintenanceViewModel)
        {
            if (ModelState.IsValid)
            {
                await _managerAccount.EditMaintenanceRequest(maintenanceViewModel);
                TempData["MaintenanceEditSuccess"] = true;
                return RedirectToAction("ShowMaintenanceRequests", new
                {
                    firstName = maintenanceViewModel.userFName,
                    lastName = maintenanceViewModel.userLName
                });
            }

            //ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", mViewModel.Id);

            return View(maintenanceViewModel);
        }

        public async Task<IActionResult> ApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            try
            {
                await _managerAccount.ApproveMaintenanceRequest(userId, maintenanceId);
            }
            catch (Exception e)
            {
                TempData["ApproveFailedMessage"] = e.Message;
                return RedirectToAction("ShowApproveMaintenaceFailed");
            }

            return RedirectToAction("ShowApproveMaintenanceSuccess");
        }

        public IActionResult ShowApproveMaintenanceSuccess()
        {
            return View();
        }

        public IActionResult ShowApproveMaintenaceFailed()
        {
            ViewBag.ApproveMaintenanceFailed = TempData["ApproveFailedMessage"];
            return View();
        }

        private bool MaintenanceRequestExists(int id)
        {
            return _context.MaintenanceRequests.Any(e => e.Id == id);
        }

        public async Task<IActionResult> UnApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            try
            {
                await _managerAccount.UnApproveMaintenanceRequest(userId, maintenanceId);
            }
            catch (Exception e)
            {
                TempData["UnApproveFailedMessage"] = e.Message;
                return RedirectToAction("ShowUnApproveMaintenanceFailed");
            }
            return RedirectToAction("ShowUnApproveMaintenanceSuccess");
        }

        public IActionResult ShowUnApproveMaintenanceSuccess()
        {
            return View();
        }

        public IActionResult ShowUnApproveMaintenanceFailed()
        {
            ViewBag.UnApproveMaintenanceFailed = TempData["UnApproveFailedMessage"];
            return View();
        }
    }
}