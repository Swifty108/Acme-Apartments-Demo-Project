using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Common.Interfaces;
using AcmeApartments.DAL.Models;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserService _userService;

        //todo-p: cqrs?

        public ManagerAccountController(
            IMapper mapper,
            IManagerAccount managerAccount,
            IApplicationService applicationService,
            IUserService userService)
        {
            _mapper = mapper;
            _managerAccount = managerAccount;
            _applicationService = applicationService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowApplicationUsers()
        {
            var appUsers = await _applicationService.GetApplicationUsers();
            return View(appUsers);
        }

        [HttpGet]
        public IActionResult ShowApplications(string userId, string firstName, string lastName)
        {
            var appsWithUser = _applicationService.GetApplications(userId);
            var applicationsViewModel = new ApplicationsViewModel
            {
                Applications = appsWithUser,
                FirstName = firstName,
                LastName = lastName
            };

            ViewBag.AppEditSuccess = TempData["AppEditSuccess"];
            ViewBag.AppApproveSuccess = TempData["AppApproveSuccess"];
            ViewBag.AppUnApproveSuccess = TempData["AppUnApproveSuccess"];
            ViewBag.AppCanceledSuccess = TempData["AppCanceledSuccess"];

            ViewBag.AppEditFailed = TempData["AppEditFailed"];
            ViewBag.AppApproveFailed = TempData["AppApproveFailed"];
            ViewBag.AppUnApproveFailed = TempData["AppUnApproveFailed"];
            ViewBag.AppCanceledFailed = TempData["AppCanceledFailed"];

            return View(applicationsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ViewApplication(int Id)
        {
            var application = await _applicationService.GetApplication(Id);
            return View(application);
        }

        public async Task<IActionResult> EditApplication(int Id)
        {
            var application = await _applicationService.GetApplication(Id);
            if (application == null)
            {
                return NotFound();
            }

            var app = _mapper.Map<ApplicationsViewModel>(application);

            return View(app);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplication(ApplicationsViewModel application)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var applicationDTO = _mapper.Map<ApplicationDTO>(application);
                    await _managerAccount.EditApplication(applicationDTO);
                    var user = await _userService.GetUserByID(application.AptUserId);

                    TempData["AppEditSuccess"] = true;
                    return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
                }
                catch (Exception)
                {
                    TempData["AppEditFailed"] = true;
                    throw;
                }
            }

            // ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        public async Task<IActionResult> CancelApplication(int Id)
        {
            var application = await _applicationService.GetApplication(Id);
            return View(application);
        }

        // POST: ApplicantAccount/Delete/5
        [HttpPost, ActionName("AppCancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplicationConfirmed(Application application)
        {
            try
            {
                await _managerAccount.CancelApplication(application.ApplicationId);
            }
            catch (Exception)
            {
                TempData["AppCanceledFailed"] = true;
                throw;
            }

            var app = await _applicationService.GetApplication(application.ApplicationId);
            var user = await _userService.GetUserByID(app.AptUserId);
            TempData["AppCanceledSuccess"] = true;

            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
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
            catch (Exception)
            {
                TempData["AppApproveFailed"] = true;
                throw;
            }

            var user = await _userService.GetUserByID(approveViewModel.UserId);

            TempData["AppApproveSuccess"] = true;
            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
        }

        public async Task<IActionResult> UnApproveApplication(string userId, string aptNumber, int applicationId)
        {
            try
            {
                await _managerAccount.UnApproveApplication(userId, aptNumber, applicationId);
            }
            catch (Exception)
            {
                TempData["AppUnApproveFailed"] = true;
                throw;
            }

            var user = await _userService.GetUserByID(userId);

            TempData["AppUnApproveSuccess"] = true;
            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
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
            ViewBag.MaintenanceApproveSuccess = TempData["MaintenanceApproveSuccess"];
            ViewBag.MaintenanceUnApproveSuccess = TempData["MaintenanceUnApproveSuccess"];
            ViewBag.MaintenanceCanceledSuccess = TempData["MaintenanceCanceledSuccess"];

            ViewBag.MaintenanceEditFailed = TempData["MaintenanceEditFailed"];
            ViewBag.MaintenanceApproveFailed = TempData["MaintenanceApproveFailed"];
            ViewBag.MaintenanceUnApproveFailed = TempData["MaintenanceUnApproveFailed"];
            ViewBag.MaintenanceCanceledFailed = TempData["MaintenanceCanceledFailed"];

            return View(mViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ViewMaintenanceRequest(int maintenanceId, string firstName, string lastName)
        {
            var maintenanceRecord = await _managerAccount.GetMaintenanceRequest(maintenanceId);

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
                throw;
            }

            var user = await _userService.GetUserByID(userId);

            TempData["MaintenanceApproveSuccess"] = true;

            return RedirectToAction("ShowMaintenanceRequests", new
            {
                firstName = user.FirstName,
                lastName = user.LastName
            });
        }

        public async Task<IActionResult> UnApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            try
            {
                await _managerAccount.UnApproveMaintenanceRequest(userId, maintenanceId);
            }
            catch (Exception e)
            {
                throw;
            }

            var user = await _userService.GetUserByID(userId);

            TempData["MaintenanceUnApproveSuccess"] = true;

            return RedirectToAction("ShowMaintenanceRequests", new
            {
                firstName = user.FirstName,
                lastName = user.LastName
            });
        }
    }
}