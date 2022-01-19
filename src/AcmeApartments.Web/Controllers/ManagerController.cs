using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IUserService _userService;

        public ManagerController(
            IMapper mapper,
            IApplicationService applicationService,
            IMaintenanceService maintenanceService,
            IUserService userService)
        {
            _mapper = mapper;
            _applicationService = applicationService;
            _maintenanceService = maintenanceService;
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
        public IActionResult ShowApplications(string userId)
        {
            var userApps = _applicationService.GetApplications(userId);
            var userApplicationsViewModel = new UserApplicationsViewModel
            {
                Applications = userApps
            };

            ViewBag.AppEditSuccess = TempData["AppEditSuccess"];
            ViewBag.AppApproveSuccess = TempData["AppApproveSuccess"];
            ViewBag.AppUnApproveSuccess = TempData["AppUnApproveSuccess"];
            ViewBag.AppCanceledSuccess = TempData["AppCanceledSuccess"];

            ViewBag.AppEditFailed = TempData["AppEditFailed"];
            ViewBag.AppApproveFailed = TempData["AppApproveFailed"];
            ViewBag.AppUnApproveFailed = TempData["AppUnApproveFailed"];
            ViewBag.AppCanceledFailed = TempData["AppCanceledFailed"];

            return View(userApplicationsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ViewApplication(int Id)
        {
            var application = await _applicationService.GetApplication(Id);
            return View(application);
        }

        [HttpGet]
        public async Task<IActionResult> EditApplication(int Id)
        {
            var application = await _applicationService.GetApplication(Id);
            if (application == null)
            {
                return NotFound();
            }

            var app = _mapper.Map<ApplicationViewModel>(application);

            return View(app);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApplication(ApplicationBindingModel applicationBindingModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var applicationDTO = _mapper.Map<ApplicationDTO>(applicationBindingModel);
                    await _applicationService.EditApplication(applicationDTO);
                    var user = await _userService.GetUserByID(applicationBindingModel.AptUserId);

                    TempData["AppEditSuccess"] = true;
                    return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
                }
                catch (Exception)
                {
                    TempData["AppEditFailed"] = true;
                    throw;
                }
            }

            var applicationViewModel = _mapper.Map<ApplicationViewModel>(applicationBindingModel);

            return View(applicationViewModel);
        }

        public async Task<IActionResult> CancelApplication(int Id)
        {
            var application = await _applicationService.GetApplication(Id);
            return View(application);
        }

        // POST: ApplicantAccount/Delete/5
        [HttpPost, ActionName("AppCancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplicationConfirmed(ApplicationBindingModel applicationBindingModel)
        {
            try
            {
                await _applicationService.CancelApplication(applicationBindingModel.ApplicationId);
            }
            catch (Exception)
            {
                TempData["AppCanceledFailed"] = true;
                throw;
            }

            var app = await _applicationService.GetApplication(applicationBindingModel.ApplicationId);
            var user = await _userService.GetUserByID(app.AptUserId);
            TempData["AppCanceledSuccess"] = true;

            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
        }

        public async Task<IActionResult> ApproveApplication(ApproveAppBindingModel approveAppBindingModel)
        {
            try
            {
                await _applicationService.ApproveApplication(
                    approveAppBindingModel.UserId,
                    approveAppBindingModel.ApplicationId,
                    approveAppBindingModel.SSN,
                    approveAppBindingModel.AptNumber,
                    approveAppBindingModel.AptPrice
                    );
            }
            catch (Exception)
            {
                TempData["AppApproveFailed"] = true;
                throw;
            }

            var user = await _userService.GetUserByID(approveAppBindingModel.UserId);

            TempData["AppApproveSuccess"] = true;
            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
        }

        public async Task<IActionResult> UnApproveApplication(string userId, string aptNumber, int applicationId)
        {
            try
            {
                await _applicationService.UnApproveApplication(userId, aptNumber, applicationId);
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
            var maintenanceRequestsUsers = await _maintenanceService.GetMaintenanceRequestsUsers();
            return View(maintenanceRequestsUsers);
        }

        public async Task<IActionResult> ShowMaintenanceRequests(string aptUserId)
        {
            var maintenanceRecords = await _maintenanceService.GetMaintenanceUserRequests(aptUserId);

            var maintenanceRequestsListViewModel = new MaintenanceRequestsListViewModel
            {
                MaintenanceRequests = maintenanceRecords
            };

            ViewBag.MaintenanceEditSuccess = TempData["MaintenanceEditSuccess"];
            ViewBag.MaintenanceApproveSuccess = TempData["MaintenanceApproveSuccess"];
            ViewBag.MaintenanceUnApproveSuccess = TempData["MaintenanceUnApproveSuccess"];
            ViewBag.MaintenanceCanceledSuccess = TempData["MaintenanceCanceledSuccess"];

            ViewBag.MaintenanceEditFailed = TempData["MaintenanceEditFailed"];
            ViewBag.MaintenanceApproveFailed = TempData["MaintenanceApproveFailed"];
            ViewBag.MaintenanceUnApproveFailed = TempData["MaintenanceUnApproveFailed"];
            ViewBag.MaintenanceCanceledFailed = TempData["MaintenanceCanceledFailed"];

            return View(maintenanceRequestsListViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ViewMaintenanceRequest(int maintenanceId)
        {
            var maintenanceRequestViewModel = await GetMaintenanceRequest(maintenanceId);

            return View(maintenanceRequestViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditMaintenanceRequest(int maintenanceId)
        {
            var maintenanceRequestViewModel = await GetMaintenanceRequest(maintenanceId);

            return View(maintenanceRequestViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMaintenanceRequest(MaintenanceRequestEditBindingModel maintenanceRequestEditBindingModel)
        {
            if (ModelState.IsValid)
            {
                var maintenanceRequestEditDTO = _mapper.Map<MaintenanceRequestEditDTO>(maintenanceRequestEditBindingModel);

                await _maintenanceService.EditMaintenanceRequest(maintenanceRequestEditDTO);
                TempData["MaintenanceEditSuccess"] = true;
                return RedirectToAction("ShowMaintenanceRequests", new { aptUserId = maintenanceRequestEditDTO.AptUserId});
            }

            var maintenancRequestEditViewModel = _mapper.Map<MaintenanceRequestEditViewModel>(maintenanceRequestEditBindingModel);

            return View(maintenancRequestEditViewModel);
        }

        public async Task<IActionResult> ApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            try
            {
                await _maintenanceService.ApproveMaintenanceRequest(userId, maintenanceId);
            }
            catch (Exception)
            {
                throw;
            }

            var user = await _userService.GetUserByID(userId);

            TempData["MaintenanceApproveSuccess"] = true;

            return RedirectToAction("ShowMaintenanceRequests", new { aptUserId = user.Id });
        }

        public async Task<IActionResult> UnApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            try
            {
                await _maintenanceService.UnApproveMaintenanceRequest(userId, maintenanceId);
            }
            catch (Exception)
            {
                throw;
            }

            var user = await _userService.GetUserByID(userId);

            TempData["MaintenanceUnApproveSuccess"] = true;

            return RedirectToAction("ShowMaintenanceRequests", new { aptUserId = user.Id });
        }

        public async Task<MaintenanceRequestViewModel> GetMaintenanceRequest(int maintenanceId)
        {
            var maintenanceRecord = await _maintenanceService.GetMaintenanceRequest(maintenanceId);
            var maintenanceRequestViewModel = _mapper.Map<MaintenanceRequestViewModel>(maintenanceRecord);
            return maintenanceRequestViewModel;
        }
    }
}