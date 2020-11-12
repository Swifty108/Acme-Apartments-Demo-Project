using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.HelperClasses;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
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

        public ManagerAccountController(ApplicationDbContext context, UserManager<AptUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, SignInManager<AptUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager = signInManager;

        }

        // GET: ManagerAccount
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewApplication(int Id)
        {
            var application = await _context.Applications.FindAsync(Id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);

        }
        public async Task<IActionResult> ApplicationUsers()
        {
            var applicationUsers = await (from userRecord in _context.Users
                                   join applicationRecord in _context.Applications on userRecord.Id equals applicationRecord.AptUserId
                                   select userRecord).Distinct().ToListAsync();
            return View(applicationUsers);
        }


        public async Task<IActionResult> ApplicationUser(string userId)
        {

            var applications = await _context.Applications.Where(u => u.AptUserId == userId).ToListAsync();
            var user = await _context.Users.FindAsync(userId);

            return View(new ApplicationViewModel
            {
                Apps = applications,
                FirstName = user.FirstName,
                LastName = user.LastName

            });
        }


        public async Task<IActionResult> ApplicationEdit(int? Id)
        {
            var application = await _context.Applications.FindAsync(Id);
            if (application == null)
            {
                return NotFound();
            }


            var app = _mapper.Map<ApplicationViewModel>(application);

            ViewBag.AppEditSuccess = TempData["AppEditSuccess"];

            // ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(app);
        }

        // POST: ApplicantAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplicationEdit(ApplicationViewModel application)
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
                return RedirectToAction("ApplicationEdit");
            }

            ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        // GET: ApplicantAccount/Delete/5
        public async Task<IActionResult> ApplicationCancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.AptUser)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);

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
            var application = await _context.Applications.FindAsync(app.ApplicationId);
            application.Status = ApplicationStatus.CANCELED;

            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
            return RedirectToAction("ApplicationUser", new { userId = application.AptUserId });
        }

        public async Task<IActionResult> ApproveApplication(string id, string ssn, string aptNumber, string aptPrice, int appid)
        {
            try
            {
                var applicationUser = await _context.Users.FindAsync(id);

                applicationUser.SSN = ssn;
                applicationUser.AptNumber = aptNumber;
                applicationUser.AptPrice = aptPrice;
                _context.Users.Update(applicationUser);

                var app = await _context.Applications.FindAsync(appid);

                app.Status = ApplicationStatus.APPROVED;
                
                _context.Applications.Update(app);

                await _userManager.RemoveFromRoleAsync(applicationUser, "Applicant");
                await _userManager.AddToRoleAsync(applicationUser, "Resident");

               // await _signInManager.RefreshSignInAsync(applicationUser);

                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                TempData["ApproveFailedMessage"] = e.Message;
                return RedirectToAction("ApproveApplicationFailed");
            }
            return RedirectToAction("ApproveApplicationSuccess");
        }

        public IActionResult ApproveApplicationSuccess()
        {

            return View();
        }

        public IActionResult ApproveApplicationFailed()
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
                return RedirectToAction("UnApproveApplicationFailed");
            }
            return RedirectToAction("UnApproveApplicationSuccess");
        }

        public IActionResult UnApproveApplicationSuccess()
        {
            return View();
        }

        public IActionResult UnApproveApplicationFailed()
        {
            ViewBag.UnApproveApplicationFailed = TempData["UnApproveFailedMessage"];
            return View();
        }






        [HttpGet]
        public async Task<IActionResult> MaintenanceRequests()
        {
            var userRecords = (from userRecord in _context.Users
                               join mRecord in _context.MaintenanceRequests on userRecord.Id equals mRecord.AptUserId
                               select userRecord).Distinct();

            var vf = await userRecords.ToListAsync();

            return View(vf);
        }

        public async Task<IActionResult> MaintenanceUser(string firstName, string lastName)
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

            return View(new MaintenanceRequestViewModel{
                mRequest = mRecord,
                userFName = firstName,
                userLName = lastName });

        }


        public async Task<IActionResult> MaintenanceEdit(int? Id, string firstName, string lastName)
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
        public async Task<IActionResult> MaintenanceEdit(MaintenanceRequestViewModel mViewModel)
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
                return RedirectToAction("MaintenanceUser", new { firstName = mViewModel.userFName, lastName = mViewModel.userLName });
            }

            //ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", mViewModel.Id);

            return View(mViewModel);
        }

        // GET: ApplicantAccount/Delete/5
        public async Task<IActionResult> MaintenanceDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mRecord = await _context.MaintenanceRequests
                .Include(a => a.AptUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mRecord == null)
            {
                return NotFound();
            }

            return View(mRecord);
        }

        [HttpPost, ActionName("MaintenanceDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaintenaceConfirmed(MaintenanceRequest request)
        {
            var mRecord = await _context.MaintenanceRequests.FindAsync(request.Id);
            _context.MaintenanceRequests.Remove(mRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MaintenanceUser));
        }

        public async Task<IActionResult> ApproveMaintenance(string uid, int mid)
        {
            try
            {
                var mRecord = await _context.MaintenanceRequests.FindAsync(mid);
                mRecord.AptUserId = uid;
                mRecord.Status = MaintenanceRequestStatus.APPROVED;

                _context.MaintenanceRequests.Update(mRecord);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {

                TempData["ApproveFailedMessage"] = e.Message;
                return RedirectToAction("ApproveMaintenaceFailed");

            }

            return RedirectToAction("ApproveMaintenanceSuccess");
        }

        public IActionResult ApproveMaintenanceSuccess()
        {
            //ViewBag.ApproveApplicationSuccess = TempData["ApproveSuccessMessage"];
            return View();
        }

        public IActionResult ApproveMaintenaceFailed()
        {
            ViewBag.ApproveMaintenanceFailed = TempData["ApproveFailedMessage"];
            return View();
        }

        private bool MaintenanceRequestExists(int id)
        {
            return _context.MaintenanceRequests.Any(e => e.Id == id);
        }

        public async Task<IActionResult> UnApproveMaintenance(string userId, int maintenanceId)
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
                return RedirectToAction("UnApproveMaintenanceFailed");
            }
            return RedirectToAction("UnApproveMaintenanceSuccess");
        }

        public IActionResult UnApproveMaintenanceSuccess()
        {
            return View();
        }

        public IActionResult UnApproveMaintenanceFailed()
        {
            ViewBag.UnApproveMaintenanceFailed = TempData["UnApproveFailedMessage"];
            return View();
        }
    }
}
