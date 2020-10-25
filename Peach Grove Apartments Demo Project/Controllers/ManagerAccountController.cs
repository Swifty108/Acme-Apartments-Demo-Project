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

      
        public async Task<IActionResult> ApplicationEdit(int? id)
        {
            var application = await _context.Applications.FindAsync(id);
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

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.ApplicationId == id);
        }


        [HttpGet]
        public IActionResult Maintenance()
        {

            ViewBag.MaintenanceSuccess = TempData["MaintenanceSuccess"];

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Maintenance(MaintenanceRequestViewModel maintReqViewModel)
        {

            if (ModelState.IsValid)
            {

                try
                {

                    var user = await _userManager.GetUserAsync(User);
                    var maintReq = new MaintenanceRequest { AptUser = user, DateRequested = DateTime.Now, isAllowedToEnter = maintReqViewModel.isAllowedToEnter, ProblemDescription = maintReqViewModel.ProblemDescription };
                    await _context.MaintenanceRequests.AddAsync(maintReq);
                    await _context.SaveChangesAsync();

                    TempData["MaintenanceSuccess"] = true;

                    return RedirectToAction("Maintenance");

                }
                catch (Exception e)
                {
                    TempData["MaintenanceSuccess"] = false;
                    return View(maintReqViewModel);
                }

            }

            return View(maintReqViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ReqHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            var maintReqs = await _context.MaintenanceRequests.Where(a => a.AptUserId == user.Id).ToListAsync();
            var maintReqViewModel = new MaintenanceReqHistoryViewModel { Requests = maintReqs };
            TempData["SuccessMessage"] = "";

            return View(maintReqViewModel);
        }


        public async Task<IActionResult> Payments()
        {
            var user = await _userManager.GetUserAsync(User);

            var waterBill = await _context.WaterBills.FirstOrDefaultAsync();
            var electricBill = await _context.ElectricBills.FirstOrDefaultAsync();
            var wbill = new WaterBill();
            var ebill = new ElectricBill();

            if (waterBill == null)
            {
                wbill = new WaterBill { AptUser = user, Amount = 42.53M, DateDue = DateTime.Now.AddDays(20) };
                await _context.AddAsync(wbill);
                await _context.SaveChangesAsync();
            }

            if (electricBill == null)
            {
                ebill = new ElectricBill { AptUser = user, Amount = 96.53M, DateDue = DateTime.Now.AddDays(20) };
                await _context.AddAsync(ebill);
                await _context.SaveChangesAsync();
            }

            var app = await _context.Applications.Where(u => u.AptUserId == user.Id).FirstOrDefaultAsync();


            var payViewModel = new PaymentsViewModel
            {
                Application = app,
                WaterBill = waterBill ?? wbill,
                ElectricBill = electricBill ?? ebill
            };

            return View(payViewModel);
        }

        public IActionResult WriteReview()
        {
            ViewBag.ReviewSuccess = TempData["ReviewSuccess"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteReview(ReviewViewModel review)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var newReview = new Review { AptUser = user, DateReviewed = DateTime.Now.AddDays(20), ReviewText = review.ReviewText };
                await _context.AddAsync(newReview);
                await _context.SaveChangesAsync();

                TempData["ReviewSuccess"] = true;
                return RedirectToAction("WriteReview");

            }
            return View();
        }

        [HttpGet]

        public IActionResult ContactUs()
        {
            ViewBag.ContactUsSuccess = TempData["ContactUsSuccess"];
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(AppUserContactViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                TempData["ContactUsSuccess"] = true;
                return RedirectToAction("ContactUs");
            }
            return View(viewModel);
        }



    }
}
