using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private readonly UserManager<AptUser> _userManager;
        private ApplyViewModel appViewModel = new ApplyViewModel();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<AptUser> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Amenities()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }
        public IActionResult FloorPlans()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Apply(string room, string price)
        {
                    var user = await _userManager.GetUserAsync(User);
                    appViewModel.Room = room;
                    appViewModel.Price = price;
                    appViewModel.User = user;

                    return View(appViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyViewModel applicationViewModel)
        {
           var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
                {
                    var app = new Application { AptUser = user, Income = applicationViewModel.Income, Occupation = applicationViewModel.Occupation, Price = applicationViewModel.Price, ReasonForMoving = applicationViewModel.ReasonForMoving, Room = applicationViewModel.Room, SSN = applicationViewModel.SSN };
                    _db.Add(app);

                    //await _db.AddAsync(app);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            return RedirectToAction("Apply", applicationViewModel.Room, applicationViewModel.Price);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
