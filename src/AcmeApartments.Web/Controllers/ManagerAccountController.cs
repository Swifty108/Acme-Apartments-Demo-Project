using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerAccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        private readonly IManagerAccount _managerAccount;

        public ManagerAccountController(
            IMapper mapper,
            IManagerAccount managerAccount,
            IApplicationService applicationService)
        {
            _mapper = mapper;
            //   _managerAccount = managerRepository;
            _managerAccount = managerAccount;
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
        public async Task<IActionResult> EditApplication(int applicationId)
        {
            var application = await _applicationService.GetApplication(applicationId);
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
                    var applicationDTO = _mapper.Map<ApplicationDTO>(application);
                    await _managerAccount.EditApplication(applicationDTO);

                    TempData["AppEditSuccess"] = true;
                    return RedirectToAction("EditApplication");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            // ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        public IActionResult CancelApplication(int applicationId)
        {
            return View(new { applicationId });
        }

        // POST: ApplicantAccount/Delete/5
        [HttpPost, ActionName("AppCancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplicationConfirmed(int applicationId)
        {
            var application = await _managerAccount.CancelApplication(applicationId);

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
            var maintenanceRequestsUsers = await _managerAccount.GetMaintenanceRequestsUsers();
            return View(maintenanceRequestsUsers);
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
                var maintenanceRequestDTO = _mapper.Map<MaintenanceRequestDTO>(maintenanceViewModel);

                await _managerAccount.EditMaintenanceRequest(maintenanceRequestDTO);
                TempData["MaintenanceEditSuccess"] = true;
                return RedirectToAction("ShowMaintenanceRequests", new
                {
                    firstName = maintenanceViewModel.userFName,
                    lastName = maintenanceViewModel.userLName
                });
            }

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