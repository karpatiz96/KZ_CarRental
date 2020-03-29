using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using CarRental.Web.Resources;
using System.Reflection;
using CarRental.Web.ViewRender;
using CarRental.Bll.Dtos;
using CarRental.Dal.Users;
using CarRental.Bll.IServices;
using CarRental.Bll.Messages;

namespace CarRental.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IRazorViewToStringRender _render;
        private readonly IStringLocalizer _localizer;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ICloudStorageService cloudStorageService,
            IRazorViewToStringRender render,
            IStringLocalizerFactory factory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _cloudStorageService = cloudStorageService;
            _render = render;
            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("IdentityResource", assemblyName.Name);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            /*[Required]
            [DataType(DataType.Text)]
            [Display(Name = "NAME")]
            public string Name { get; set; }*/

            [Required(ErrorMessage = "EMAIL_REQUIRED")]
            [EmailAddress(ErrorMessage = "EMAIL_INVALID")]
            [Display(Name = "EMAIL")]
            public string Email { get; set; }

            [Required(ErrorMessage = "PASSWORD_REQUIRED")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "PASSWORD")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "CONFIRM_PASSWORD")]
            [Compare("Password", ErrorMessage = "CONFIRM_PASSWORD_NOT_MATCHING")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var addToRole = await _userManager.AddToRoleAsync(user, Roles.Customer);

                    if (addToRole.Succeeded)
                    {
                        _logger.LogInformation($"User {0} added to role {1}.", user.Id, Roles.Customer);
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    /*await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/

                    var model = new EmailConfirmationDto(user.Name ?? user.UserName, callbackUrl);

                    const string view = "/Views/Emails/ConfirmAccountEmail";
                    var body = await _render.RenderViewToStringAsync($"{view}Html.cshtml", model);

                    QueueEmailMessage queueEmail = new QueueEmailMessage(Input.Email, "", body, "Reset Password");
                    await _cloudStorageService.SendMessage(queueEmail);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", body);


                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
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
