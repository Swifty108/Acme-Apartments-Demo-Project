using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Core.Models;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Inerfaces;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AptUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<AptUser> userManager, ApplicationDbContext context, IRepository repository, IEmailService emailService, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _repository = repository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAmenities()
        {
            return View();
        }

        public IActionResult ShowGallery()
        {
            return View();
        }

        public async Task<IActionResult> ShowFloorPlans()
        {
            return View(_mapper.Map<FloorPlansViewModel>(await _repository.GetFloorPlans()));
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpGet]
        public async Task<IActionResult> Apply(string aptNumber, string price, string area)
        {
            var appViewModel = new ApplyViewModel();
            var user = await _userManager.GetUserAsync(User);
            appViewModel.AptNumber = aptNumber;
            appViewModel.Price = price;
            appViewModel.User = user;
            appViewModel.Area = area;

            return View(appViewModel);
        }

        [Authorize(Roles = "Applicant, Resident")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyViewModel applicationViewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            //if(_context.Applications.Where(a => a.AptNumber == applicationViewModel.AptNumber) != null)
            //{
            //    return RedirectToAction("index", "applicantaccount", new { IsApplySuccess = true });
            //}

            if (ModelState.IsValid)
            {
                var app = new Application { AptUser = user, Income = applicationViewModel.Income, Occupation = applicationViewModel.Occupation, Price = applicationViewModel.Price, ReasonForMoving = applicationViewModel.ReasonForMoving, AptNumber = applicationViewModel.AptNumber, Area = applicationViewModel.Area, DateApplied = DateTime.Now, SSN = applicationViewModel.SSN };
                _context.Add(app);

                await _context.SaveChangesAsync();

                var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

                return RedirectToAction("index", $"{userRole}account", new { IsApplySuccess = true });
            }

            return RedirectToAction("Apply", applicationViewModel.AptNumber, applicationViewModel.Price);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //TODO-P: implement caching like this in other areas of the app instead of using Redis cache framework. simpler.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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
                _emailService.Send(new EmailMessage
                {
                    FromAddresses = new EmailAddress
                    {
                        Name = viewModel.Name,
                        Address = viewModel.EmailAddress
                    },

                    ToAddresses = new EmailAddress
                    {
                        Name = "Raj Narayanan",
                        Address = "rajnarayanan2020@gmail.com"
                    },

                    Subject = viewModel.Subject,
                    Content = viewModel.Message
                }
                );

                return RedirectToAction("ContactUs");
            }
            return View(viewModel);
        }
    }
}