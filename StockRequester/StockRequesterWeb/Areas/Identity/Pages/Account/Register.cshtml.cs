﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using StockRequester.DataAccess.Repository;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace StockRequesterWeb.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IOptions<RecaptchaSettings> _recaptchaSettings;
        private readonly HttpClient _httpClient;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IOptions<RecaptchaSettings> recaptchaSettings,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore   = userStore;
            _emailStore  = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _recaptchaSettings = recaptchaSettings;
            _httpClient = new HttpClient();
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string RecaptchaSiteKey => _recaptchaSettings.Value.SiteKey;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            
            [Required]
            [Display(Name =  "User profile name")]
            public string? Name { get; set; }

            //
            // Roles:
            //

            [Required(ErrorMessage = "Please select an option")] public string? Role { get; set; }
            [ValidateNever] public IEnumerable<SelectListItem> RegisterRoleTextList {  get; set; }           
            
            [Required(ErrorMessage = "Please complete the reCAPTCHA verification")]
            [Display(Name = "ReCaptcha Response")]
            public string RecaptchaResponse { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if(!_roleManager.RoleExistsAsync(SD.Role_SiteAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_SiteAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_CompanyAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_CompanyUser)).GetAwaiter().GetResult();
            }

            Input = new()
            {
                RegisterRoleTextList = new List<SelectListItem>()
                {
                    new SelectListItem(){
                        Text  = SD.RegisterRoleText_CompanyUser,
                        Value = SD.Role_CompanyUser
                    },
                    new SelectListItem(){
                        Text  = SD.RegisterRoleText_CompanyAdmin,
                        Value = SD.Role_CompanyAdmin
                    }
                }

                //RoleList = _roleManager.Roles.Select(x => x.Name).Select(
                //    i => new SelectListItem
                //    {
                //        Text  = i,
                //        Value = i
                //    }
                //),

            };
        
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Repopulate the role list regardless of validation state
            Input.RegisterRoleTextList = new List<SelectListItem>()
            {
                new SelectListItem(){
                    Text  = SD.RegisterRoleText_CompanyUser,
                    Value = SD.Role_CompanyUser
                },
                new SelectListItem(){
                    Text  = SD.RegisterRoleText_CompanyAdmin,
                    Value = SD.Role_CompanyAdmin
                }
            };

            if (string.IsNullOrEmpty(Input.RecaptchaResponse))
            {
                ModelState.AddModelError(string.Empty, "Please complete the reCAPTCHA verification.");
                return Page();
            }

            if (ModelState.IsValid)
            {
                // Verify reCAPTCHA
                var recaptchaResponse = await VerifyRecaptchaAsync(Input.RecaptchaResponse);
                if (!recaptchaResponse.Success)
                {
                    ModelState.AddModelError(string.Empty, "reCAPTCHA verification failed. Please try again.");
                    return Page();
                }

                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                user.Name      = Input.Name;
                
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!string.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_CompanyUser);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
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

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        private async Task<RecaptchaVerificationResponse> VerifyRecaptchaAsync(string recaptchaResponse)
        {
            var parameters = new Dictionary<string, string>
            {
                {"secret", _recaptchaSettings.Value.SecretKey},
                {"response", recaptchaResponse}
            };

            var response = await _httpClient.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(parameters)
            );

            var jsonResponse = await response.Content.ReadFromJsonAsync<RecaptchaVerificationResponse>();
            return jsonResponse;
        }
    }

    public class RecaptchaVerificationResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}
