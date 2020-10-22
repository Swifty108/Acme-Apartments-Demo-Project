using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    public class AppUserAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AptUser> _userManager;

        public AppUserAccountController(ApplicationDbContext context, UserManager<AptUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: AppUserAccount
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Applications.Include(a => a.AptUser);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Maintenance()
        {
            var user = await _userManager.GetUserAsync(User);

            //var maintreq = new MaintenanceRequest { AptUser = user, ProblemDescription = "My closet door is not opening smoothly. This started yesterday.", DateRequested = DateTime.Now, isAllowedToEnter = true };

            //await _context.MaintenanceRequests.AddAsync(maintreq);
            //await _context.SaveChangesAsync();

            var maintReq = await _context.MaintenanceRequests.Where(a => a.AptUserId == user.Id).FirstOrDefaultAsync();
            var maintReqs = await _context.MaintenanceRequests.ToListAsync();

            var maintenanceReqViewModel = new CompositeMaintRequestViewModel
            {
                MaintenanceRequestViewModel = new MaintenanceRequestViewModel { },
                MaintenanceRequests = maintReqs
            };

            TempData["isSuccess"] = "";

            return View(maintenanceReqViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Maintenance(CompositeMaintRequestViewModel compMaintReqViewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var maintReq = new MaintenanceRequest { AptUser = user, DateRequested = DateTime.Now, isAllowedToEnter = compMaintReqViewModel.MaintenanceRequestViewModel.isAllowedToEnter, ProblemDescription = compMaintReqViewModel.MaintenanceRequestViewModel.ProblemDescription };

            if (ModelState.IsValid) // Is User Input Valid?
            {
                try
                {
                    await _context.MaintenanceRequests.AddAsync(maintReq);
                    await _context.SaveChangesAsync();

                    //TempData["isSuccess"] = new MessageVM() { CssClassName = "alert-sucess", Title = "Success!", Message = "Operation Done." };
                    TempData["isSuccess"] = "success";

                }
                catch (Exception e)
                {
                    TempData["isSuccess"] = "falure";
                    TempData["maintReqError"] = e;

                }

            }

            return View(compMaintReqViewModel);
        }


        public async Task<IActionResult> Payments()
        {
            var user = await _userManager.GetUserAsync(User);
            var app = await _context.Applications.Where(u => u.AptUserId == user.Id).FirstOrDefaultAsync();

            //var waterBill = _context.WaterBills.Include(a => a.AptUser);
            var waterBill = await _context.WaterBills.FirstOrDefaultAsync();
            var electricBill = await _context.ElectricBills.FirstOrDefaultAsync();

            var payViewModel = new PaymentsViewModel
            {
                Application = app,
                WaterBill = waterBill,
                ElectricBill = electricBill
            };
            //return View(await applicationDbContext.ToListAsync());

            return View(payViewModel);
        }

        // GET: AppUserAccount/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: AppUserAccount/Create
        public IActionResult Create()
        {
            ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id");
            return View();
        }

        // POST: AppUserAccount/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicationId,AptUserId,Occupation,Income,ReasonForMoving,SSN,Room,Price")] Application application)
        {
            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        // GET: AppUserAccount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        // POST: AppUserAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,AptUserId,Occupation,Income,ReasonForMoving,SSN,Room,Price")] Application application)
        {
            if (id != application.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AptUserId"] = new SelectList(_context.AptUsers, "Id", "Id", application.AptUserId);
            return View(application);
        }

        // GET: AppUserAccount/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: AppUserAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.ApplicationId == id);
        }
    }
}
