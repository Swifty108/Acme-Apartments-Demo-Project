using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
using PeachGroveApartments.ApplicationLayer.Interfaces;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AptUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private SignInManager<AptUser> _signInManager;
        private readonly IRepository _repository;
        private readonly IDomainLogic _applicationLayer;

        public ManagerAccountController(ApplicationDbContext context, UserManager<AptUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, SignInManager<AptUser> signInManager, IRepository repository, IDomainLogic applicationLayer)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _repository = repository;
            _applicationLayer = applicationLayer;
        }

        // GET: ManagerAccount
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewApplication(int Id)
        {
            var application = await _repository.GetApplication(Id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        public async Task<IActionResult> ShowApplicationUsers()
        {
            return View(await _repository.GetApplicationUsers());
        }

        public async Task<IActionResult> ShowApplications(string userId)
        {
            return View(_mapper.Map<ApplicationViewModel>(await _repository.GetApplications(userId)));
        }

        public async Task<IActionResult> EditApplication(int Id)
        {
            var application = await _repository.GetApplication(Id);
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
                    // var appRecord = _context.Applications.FindAsync(application.ApplicationId).Result;
                    var app = _mapper.Map<Application>(application);
                    _context.Update(app);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["AppEditSuccess"] = true;
                return RedirectToAction("EditApplication");
            }

            ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        // GET: ApplicantAccount/Delete/5
        public async Task<IActionResult> CancelApplication(int Id)
        {
            //var application = await _context.Applications
            //    .Include(a => a.AptUser)
            //    .FirstOrDefaultAsync(m => m.ApplicationId == id);

            var application = await _repository.GetApplication(Id);

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
            var application = await _applicationLayer.CancelApplication(app.ApplicationId);

            return RedirectToAction("ShowApplications", new { userId = application.AptUserId });
        }

        public async Task<IActionResult> ApproveApplication(string userId, string appid, string ssn, string aptNumber, string aptPrice)
        {
            try
            {
                int appIdint = int.Parse(appid);
                await _applicationLayer.ApproveApplication(userId, "fds", ssn, aptNumber, aptPrice);
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

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.ApplicationId == id);
        }

        public async Task<IActionResult> UnApproveApplication(string id, string aptNumber, int appid)
        {
            try
            {
                var applicationUser = await _context.Users.FindAsync(id);

                if (applicationUser.AptNumber == aptNumber)
                {
                    applicationUser.SSN = null;
                    applicationUser.AptNumber = null;
                    applicationUser.AptPrice = null;

                    await _userManager.RemoveFromRoleAsync(applicationUser, "Resident");
                    await _userManager.AddToRoleAsync(applicationUser, "Applicant");

                    _context.Users.Update(applicationUser);
                }

                var app = await _context.Applications.FindAsync(appid);

                app.Status = ApplicationStatus.UNAPPROVED;

                _context.Applications.Update(app);

                await _context.SaveChangesAsync();
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
            var userRecords = (from userRecord in _context.Users
                               join mRecord in _context.MaintenanceRequests on userRecord.Id equals mRecord.AptUserId
                               select userRecord).Distinct();

            var vf = await userRecords.ToListAsync();

            return View(vf);
        }

        public async Task<IActionResult> ShowMaintenanceRequests(string firstName, string lastName)
        {
            // var user = _userManager.GetUserAsync(User).Result;
            var mURecords = await (from userRecord in _context.Users
                                   join mRecord in _context.MaintenanceRequests on userRecord.Id equals mRecord.AptUserId
                                   select mRecord).ToListAsync();

            var mViewModel = new MaintenanceRequestViewModel
            {
                mRequests = mURecords,
                userFName = firstName,
                userLName = lastName
            };

            ViewBag.MaintenanceEditSuccess = TempData["MaintenanceEditSuccess"];
            return View(mViewModel);
        }

        public async Task<IActionResult> ViewMaintenanceRequest(int Id, string firstName, string lastName)
        {
            var mRecord = await _context.MaintenanceRequests.FindAsync(Id);
            if (mRecord == null)
            {
                return NotFound();
            }

            return View(new MaintenanceRequestViewModel
            {
                mRequest = mRecord,
                userFName = firstName,
                userLName = lastName
            });
        }

        public async Task<IActionResult> EditMaintenanceRequest(int? Id, string firstName, string lastName)
        {
            var mRecord = await _context.MaintenanceRequests.FindAsync(Id);

            //var mRecordMapped = _mapper.Map<MaintenanceRequestViewModel>(mRecord);

            return View(new MaintenanceRequestViewModel { Id = mRecord.Id, AptUserId = mRecord.AptUserId, ProblemDescription = mRecord.ProblemDescription, isAllowedToEnter = mRecord.isAllowedToEnter, userFName = firstName, userLName = lastName });
        }

        // POST: ApplicantAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMaintenanceRequest(MaintenanceRequestViewModel mViewModel)
        {
            if (ModelState.IsValid)
            {
                var mRecord = await _context.MaintenanceRequests.FindAsync(mViewModel.Id);

                mRecord.ProblemDescription = mViewModel.ProblemDescription;
                mRecord.isAllowedToEnter = mViewModel.isAllowedToEnter;
                //  var mRecord = _mapper.Map<MaintenanceRequest>(mViewModel);

                _context.Update(mRecord);
                await _context.SaveChangesAsync();

                TempData["MaintenanceEditSuccess"] = true;
                return RedirectToAction("ShowMaintenanceRequests", new { firstName = mViewModel.userFName, lastName = mViewModel.userLName });
            }

            //ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", mViewModel.Id);

            return View(mViewModel);
        }

        // GET: ApplicantAccount/Delete/5
        public async Task<IActionResult> DeleteMaintenanceRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceRecord = await _context.MaintenanceRequests
                .Include(a => a.AptUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maintenanceRecord == null)
            {
                return NotFound();
            }

            return View(maintenanceRecord);
        }

        [HttpPost, ActionName("MaintenanceDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaintenaceConfirmed(MaintenanceRequest request)
        {
            var maintenanceRecord = await _context.MaintenanceRequests.FindAsync(request.Id);
            _context.MaintenanceRequests.Remove(maintenanceRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowMaintenanceRequests));
        }

        public async Task<IActionResult> ApproveMaintenanceRequest(string userId, int maintenanceId)
        {
            try
            {
                var mRecord = await _context.MaintenanceRequests.FindAsync(maintenanceId);
                mRecord.AptUserId = userId;
                mRecord.Status = MaintenanceRequestStatus.APPROVED;

                _context.MaintenanceRequests.Update(mRecord);
                await _context.SaveChangesAsync();
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
                var maintenanceRecord = await _context.MaintenanceRequests.FindAsync(maintenanceId);
                maintenanceRecord.AptUserId = userId;
                maintenanceRecord.Status = MaintenanceRequestStatus.UNAPPROVED;

                _context.MaintenanceRequests.Update(maintenanceRecord);
                await _context.SaveChangesAsync();
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