﻿using AcmeApartments.Data.Provider.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<AptUser> _userManager;
        private readonly SignInManager<AptUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<AptUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<AptUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        public bool IsDirectLogin { get; set; }
        public bool IsLogOutSuccess { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null, bool isDirectLogin = false, bool isLoggedOut = false)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl ?? Url.Content("~/");
            IsDirectLogin = isDirectLogin;

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (isLoggedOut)
                IsLogOutSuccess = isLoggedOut;
        }

        public async Task<IActionResult> OnPostAsync(bool isDirectLogin = false, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // await _signInManager.RefreshSignInAsync(await _userManager.FindByNameAsync(Input.Email));
                    _logger.LogInformation("User logged in.");

                    if (isDirectLogin)
                    {
                        var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                        IList<string> roles = await _signInManager.UserManager.GetRolesAsync(user);

                        if (roles.Contains("Applicant"))
                        {
                            return Redirect("~/applicant/index");
                        }
                        else if (roles.Contains("Resident"))
                        {
                            return LocalRedirect("/resident/index");
                        }
                        else if (roles.Contains("Manager"))
                        {
                            return LocalRedirect("/manager/index");
                        }
                    }
                    returnUrl = returnUrl ?? Url.Content("~/");

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            return Page();
        }
    }
}