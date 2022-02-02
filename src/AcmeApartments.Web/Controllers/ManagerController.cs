using AcmeApartments.Data.Provider.Identity;
using AcmeApartments.Providers.DTOs;
using AcmeApartments.Providers.Interfaces;
using AcmeApartments.Web.BindingModels;
using AcmeApartments.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AptUser> _userManager;

        public ManagerController(
            IMapper mapper,
            IApplicationService applicationService,
            IMaintenanceService maintenanceService,
            IUserService userService,
            UserManager<AptUser> userManager)
        {
            _mapper = mapper;
            _applicationService = applicationService;
            _maintenanceService = maintenanceService;
            _userService = userService;
            _userManager = userManager;
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
            if (!ModelState.IsValid)
            {
                var applicationViewModel = _mapper.Map<ApplicationViewModel>(applicationBindingModel);
                return View(applicationViewModel);
            }

            var applicationDTO = _mapper.Map<ApplicationDto>(applicationBindingModel);
            var isEditSuccess = await _applicationService.EditApplication(applicationDTO);

            if (isEditSuccess)
            {
                TempData["AppEditSuccess"] = true;
            }
            else
            {
                TempData["AppEditFailed"] = true;
            }

            var user = await _userService.GetUserByID(applicationBindingModel.AptUserId);
            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
        }

        public async Task<IActionResult> CancelApplication(int Id)
        {
            var application = await _applicationService.GetApplication(Id);
            return View(application);
        }

        [HttpPost, ActionName("AppCancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplicationConfirmed(ApplicationBindingModel applicationBindingModel)
        {
            var isCancelSuccess = await _applicationService.CancelApplication(applicationBindingModel.ApplicationId);

            if (isCancelSuccess)
            {
                TempData["AppCanceledSuccess"] = true;
            }
            else
            {
                TempData["AppCanceledFailed"] = true;
            }

            var app = await _applicationService.GetApplication(applicationBindingModel.ApplicationId);
            var user = await _userService.GetUserByID(app.AptUserId);
           
            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
        }

        public async Task<IActionResult> ApproveApplication(ApproveAppBindingModel approveAppBindingModel)
        {
            var applicationUser = await _userService.GetUserByID(approveAppBindingModel.UserId);
            var roles = await _userManager.GetRolesAsync(applicationUser);

            var isApproveSuccess = await _applicationService.ApproveApplication(
                    applicationUser,
                    approveAppBindingModel.ApplicationId,
                    approveAppBindingModel.SSN,
                    approveAppBindingModel.AptNumber,
                    approveAppBindingModel.AptPrice,
                    roles
                    );

            if (isApproveSuccess)
            {
                TempData["AppApproveSuccess"] = true;
            }
            else
            {
                TempData["AppApproveFailed"] = true;
            }

            var user = await _userService.GetUserByID(approveAppBindingModel.UserId);
            return RedirectToAction("ShowApplications", new { userId = user.Id, firstName = user.FirstName, lastName = user.LastName });
        }

        public async Task<IActionResult> DenyApplication(string userId, string aptNumber, int applicationId)
        {
            var isDenySuccess = await _applicationService.DenyApplication(userId, aptNumber, applicationId);

            if (isDenySuccess)
            {
                TempData["AppDenySuccess"] = true;
            }
            else
            {
                TempData["AppDenyFailed"] = true;
            }

            var user = await _userService.GetUserByID(userId);
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
            if (!ModelState.IsValid)
            {    
                var maintenancRequestEditViewModel = _mapper.Map<MaintenanceRequestEditViewModel>(maintenanceRequestEditBindingModel);
                return View(maintenancRequestEditViewModel);
            }

            var maintenanceRequestEditDTO = _mapper.Map<MaintenanceRequestEditDto>(maintenanceRequestEditBindingModel);

            await _maintenanceService.EditMaintenanceRequest(maintenanceRequestEditDTO);
            TempData["MaintenanceEditSuccess"] = true;
            return RedirectToAction("ShowMaintenanceRequests", new { aptUserId = maintenanceRequestEditDTO.AptUserId });
        }

        public async Task<IActionResult> ApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            var isApproveSuccess = await _maintenanceService.ApproveMaintenanceRequest(userId, maintenanceId);

            if (isApproveSuccess)
            {
                TempData["MaintenanceApproveSuccess"] = true;
            }
            else
            {
                TempData["MaintenanceApproveFailed"] = true;
            }

            var user = await _userService.GetUserByID(userId);
            return RedirectToAction("ShowMaintenanceRequests", new { aptUserId = user.Id });
        }

        public async Task<IActionResult> DenyMaintenanceRequest(string userId, int maintenanceId)
        {
            var isDenySuccess = await _maintenanceService.DenyMaintenanceRequest(userId, maintenanceId);

            if (isDenySuccess)
            {
                TempData["MaintenanceDenySuccess"] = true;
            }
            else
            {
                TempData["MaintenanceDenyFailed"] = true;
            }

            var user = await _userService.GetUserByID(userId);
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