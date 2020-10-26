using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.VisualBasic;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.HelperClasses;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;



namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AptUser> _userManager;
        private AptUser _user;
        private readonly IMapper _mapper;

        public ManagerAccountController(ApplicationDbContext context, UserManager<AptUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;

        }

        // GET: AppUserAccount
        public IActionResult Index()
        {
            _user = _userManager.GetUserAsync(User).Result;
            return View();
        }

        public async Task<IActionResult> ViewApplicationAsync(int Id)
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
            var applicationUsers = from userRecord in _context.Users
                                   join applicationRecord in _context.Applications on userRecord.Id equals applicationRecord.AptUserId
                                   select userRecord;

            var vf = await applicationUsers.ToListAsync();


            return View(vf);
        }


        public IActionResult ApplicationUser(int Id)
        {

            var applicationUsers = from userRecord in _context.Users
                                   join applicationRecord in _context.Applications on userRecord.Id equals applicationRecord.AptUserId
                                   select applicationRecord;
            return View(applicationUsers.ToList());
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
        public async Task<IActionResult> ApplicationDelete(int? id)
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
        [HttpPost, ActionName("AppDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteApplicationConfirmed(Application app)
        {
            var application = await _context.Applications.FindAsync(app.ApplicationId);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApproveApplication(string id, string ssn, string aptNumber, int appid)
        {
            try
            {
                var applicationUser = await _context.Users.FindAsync(id);

                applicationUser.SSN = ssn;
                applicationUser.AptNumber = aptNumber;
                _context.Users.Update(applicationUser);
                _context.Applications.Remove(_context.Applications.Find(appid));
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
           //ViewBag.ApproveApplicationSuccess = TempData["ApproveSuccessMessage"];
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

        [HttpGet]
        public async Task<IActionResult> MaintenanceRequestsAsync()
        {
            var userRecords = from userRecord in _context.Users
                           join mRecord  in _context.MaintenanceRequests on userRecord.Id equals mRecord.AptUserId
                           select userRecord;

            var vf = await userRecords.ToListAsync();

            return View(vf);
        }

        public IActionResult MaintenanceUser(int Id)
        {

            var muRecords = from userRecord in _context.Users
                                   join mRecord in _context.MaintenanceRequests on userRecord.Id equals mRecord.AptUserId
                                   select mRecord;

            return View(muRecords.ToList());
        }

        public async Task<IActionResult> ViewMaintenanceRequestAsync(int Id)
        {
            var mRecord = await _context.Applications.FindAsync(Id);
            if (mRecord == null)
            {
                return NotFound();
            }

            return View(mRecord);

        }


        public async Task<IActionResult> MaintenanceEdit(int? Id)
        {
            var mRecord = await _context.MaintenanceRequests.FindAsync(Id);
            if (mRecord == null)
            {
                return NotFound();
            }


            var mRecordMapped = _mapper.Map<MaintenanceRequestViewModel>(mRecord);

            ViewBag.MaintenanceEditSuccess = TempData["MaintenanceEditSuccess"];

            // ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(mRecordMapped);
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
                try
                {
                    var mRecord = _mapper.Map<MaintenanceRequest>(mViewModel);

                    _context.Update(mRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }

                TempData["MaintenanceEditSuccess"] = true;
                return RedirectToAction("MaintenanceEdit");
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

        // POST: ApplicantAccount/Delete/5
        [HttpPost, ActionName("MaintenanceDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaintenaceConfirmed(MaintenanceRequest request)
        {
            var mRecord = await _context.Applications.FindAsync(request.Id);
            _context.Applications.Remove(mRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApproveMaintenace(string id, int mid)
        {
            try
            {
                var mUser = await _context.MaintenanceRequests.FindAsync(id);

                mUser.Id = mid;
                mUser.AptUserId = id;
                _context.MaintenanceRequests.Update(mUser);
                _context.MaintenanceRequests.Remove(_context.MaintenanceRequests.Find(mid));
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



    }
}
