using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<AptUser> _signInManager;
        private readonly UserManager<AptUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly ApplicationDbContext _context;

        public ExternalLoginModel(
            SignInManager<AptUser> signInManager,
            UserManager<AptUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> Roles { get; } = new List<SelectListItem>
        {
            new SelectListItem() { Text="Applicant", Value="Applicant"},
            new SelectListItem() { Text="Resident", Value="Resident"},
            new SelectListItem() { Text="Manager", Value="Manager"},
        };

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

            [Required]
            public string Role { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                AptUser user = null;

                if (Input.Role == "Applicant")
                {
                    user = new AptUser { FirstName = Input.FName, LastName = Input.LName, UserName = Input.Email, Email = Input.Email, DateRegistered = DateTime.Now, DateOfBirth = Input.DateOfBirth, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, Zipcode = Input.Zipcode, PhoneNumber = Input.PhoneNumber };
                }
                else if (Input.Role == "Resident")
                {
                    user = new AptUser { FirstName = Input.FName, LastName = Input.LName, UserName = Input.Email, Email = Input.Email, DateRegistered = DateTime.Now, DateOfBirth = Input.DateOfBirth, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, Zipcode = Input.Zipcode, PhoneNumber = Input.PhoneNumber, AptNumber = "3185-329", AptPrice = "1150", SSN = "153731495" };
                }
                else if (Input.Role == "Manager")
                {
                    user = new AptUser { FirstName = Input.FName, LastName = Input.LName, UserName = Input.Email, Email = Input.Email, DateRegistered = DateTime.Now, DateOfBirth = Input.DateOfBirth, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, Zipcode = Input.Zipcode, PhoneNumber = Input.PhoneNumber };
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);

                        var rDate = new RandomDateTime();

                        await _context.ElectricBills.AddAsync(new ElectricBill { AptUser = user, Amount = 98.53M, DateDue = rDate.Next() });
                        await _context.WaterBills.AddAsync(new WaterBill { AptUser = user, Amount = 57.23M, DateDue = rDate.Next() });

                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();

            //returnUrl = returnUrl ?? Url.Content("~/");

            //// Get the information about the user from the external login provider
            //var info = await _signInManager.GetExternalLoginInfoAsync();
            //if (info == null)
            //{
            //    ErrorMessage = "Error loading external login information during confirmation.";
            //    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            //}

            //if (ModelState.IsValid)
            //{
            //    AptUser user = null;

            //    if (Input.Role == "Applicant")
            //    {
            //        user = new AptUser { FirstName = Input.FName, LastName = Input.LName, UserName = Input.Email, Email = Input.Email, DateRegistered = DateTime.Now, DateOfBirth = Input.DateOfBirth, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, Zipcode = Input.Zipcode, PhoneNumber = Input.PhoneNumber };
            //    }
            //    else if (Input.Role == "Resident")
            //    {
            //        user = new AptUser { FirstName = Input.FName, LastName = Input.LName, UserName = Input.Email, Email = Input.Email, DateRegistered = DateTime.Now, DateOfBirth = Input.DateOfBirth, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, Zipcode = Input.Zipcode, PhoneNumber = Input.PhoneNumber, AptNumber = "3185-329", AptPrice = "1150", SSN = "153731495" };
            //    }
            //    else if (Input.Role == "Manager")
            //    {
            //        user = new AptUser { FirstName = Input.FName, LastName = Input.LName, UserName = Input.Email, Email = Input.Email, DateRegistered = DateTime.Now, DateOfBirth = Input.DateOfBirth, StreetAddress = Input.StreetAddress, City = Input.City, State = Input.State, Zipcode = Input.Zipcode, PhoneNumber = Input.PhoneNumber };
            //    }

            //    var result = await _userManager.CreateAsync(user, Input.Password);
            //    await _userManager.AddToRoleAsync(user, Input.Role);

            //    if (result.Succeeded)
            //    {
            //        result = await _userManager.AddLoginAsync(user, info);
            //        if (result.Succeeded)
            //        {
            //            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

            //            var userId = await _userManager.GetUserIdAsync(user);
            //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //            var callbackUrl = Url.Page(
            //                "/Account/ConfirmEmail",
            //                pageHandler: null,
            //                values: new { area = "Identity", userId = userId, code = code },
            //                protocol: Request.Scheme);

            //            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            //                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            //            // If account confirmation is required, we need to show the link if we don't have a real email sender
            //            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            //            {
            //                return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
            //            }

            //            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

            //            return LocalRedirect(returnUrl);
            //        }
            //    }
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //}

            //ProviderDisplayName = info.ProviderDisplayName;
            //ReturnUrl = returnUrl;
            //return Page();
        }
    }
}