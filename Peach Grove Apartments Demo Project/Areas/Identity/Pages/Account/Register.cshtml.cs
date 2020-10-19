using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;

namespace Peach_Grove_Apartments_Demo_Project.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AptUser> _signInManager;
        private readonly UserManager<AptUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private ApplicationDbContext _db;

        public RegisterModel(
            UserManager<AptUser> userManager,
            SignInManager<AptUser> signInManager,
            ApplicationDbContext db,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> States { get; } = new List<SelectListItem>
        {
            new SelectListItem() {Text="AL", Value="AL"},
            new SelectListItem() { Text="AK", Value="AK"},
            new SelectListItem() { Text="AZ", Value="AZ"},
            new SelectListItem() { Text="AR", Value="AR"},
            new SelectListItem() { Text="CA", Value="CA"},
            new SelectListItem() { Text="CO", Value="CO"},
            new SelectListItem() { Text="CT", Value="CT"},
            new SelectListItem() { Text="DC", Value="DC"},
            new SelectListItem() { Text="DE", Value="DE"},
            new SelectListItem() { Text="FL", Value="FL"},
            new SelectListItem() { Text="GA", Value="GA"},
            new SelectListItem() { Text="HI", Value="HI"},
            new SelectListItem() { Text="ID", Value="ID"},
            new SelectListItem() { Text="IL", Value="IL"},
            new SelectListItem() { Text="IN", Value="IN"},
            new SelectListItem() { Text="IA", Value="IA"},
            new SelectListItem() { Text="KS", Value="KS"},
            new SelectListItem() { Text="KY", Value="KY"},
            new SelectListItem() { Text="LA", Value="LA"},
            new SelectListItem() { Text="ME", Value="ME"},
            new SelectListItem() { Text="MD", Value="MD"},
            new SelectListItem() { Text="MA", Value="MA"},
            new SelectListItem() { Text="MI", Value="MI"},
            new SelectListItem() { Text="MN", Value="MN"},
            new SelectListItem() { Text="MS", Value="MS"},
            new SelectListItem() { Text="MO", Value="MO"},
            new SelectListItem() { Text="MT", Value="MT"},
            new SelectListItem() { Text="NE", Value="NE"},
            new SelectListItem() { Text="NV", Value="NV"},
            new SelectListItem() { Text="NH", Value="NH"},
            new SelectListItem() { Text="NJ", Value="NJ"},
            new SelectListItem() { Text="NM", Value="NM"},
            new SelectListItem() { Text="NY", Value="NY"},
            new SelectListItem() { Text="NC", Value="NC"},
            new SelectListItem() { Text="ND", Value="ND"},
            new SelectListItem() { Text="OH", Value="OH"},
            new SelectListItem() { Text="OK", Value="OK"},
            new SelectListItem() { Text="OR", Value="OR"},
            new SelectListItem() { Text="PA", Value="PA"},
            new SelectListItem() { Text="PR", Value="PR"},
            new SelectListItem() { Text="RI", Value="RI"},
            new SelectListItem() { Text="SC", Value="SC"},
            new SelectListItem() { Text="SD", Value="SD"},
            new SelectListItem() { Text="TN", Value="TN"},
            new SelectListItem() { Text="TX", Value="TX"},
            new SelectListItem() { Text="UT", Value="UT"},
            new SelectListItem() { Text="VT", Value="VT"},
            new SelectListItem() { Text="VA", Value="VA"},
            new SelectListItem() { Text="WA", Value="WA"},
            new SelectListItem() { Text="WV", Value="WV"},
            new SelectListItem() { Text="WI", Value="WI"},
            new SelectListItem() { Text="WY", Value="WY"}
        };

        public class InputModel
        {

            [Required]
            [Display(Name = "First Name")]
            public string FName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LName { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
           // [Column(TypeName = "date")]
            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateTime DateOfBirth { get; set; }
            [Required]
            [Display(Name = "Street Address")]
            public string StreetAddress { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required]
            [Display(Name = "Zip Code")]
            public string Zipcode { get; set; }
            [Required]
            [Display(Name = "Phone Number")]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string PhoneNumber { get; set; }
            public Boolean IsResident { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect(returnUrl);
            }

            //var tempUser = await _userManager.FindByNameAsync(Input.Email);

            //if (tempUser != null)
            //{
            //    ModelState.AddModelError("Email", "User with this email already exists");
            //   //return await this.OnGetAsync();
            //}

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AptUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FName, LastName = Input.LName, DateOfBirth = Input.DateOfBirth, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, Zipcode = Input.Zipcode, PhoneNumber = Input.PhoneNumber, IsResident = false };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
