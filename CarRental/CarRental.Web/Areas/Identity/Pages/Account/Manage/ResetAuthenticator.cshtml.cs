using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CarRental.Dal.Entities;
using CarRental.Web.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CarRental.Web.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
    {
        UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        ILogger<ResetAuthenticatorModel> _logger;
        private readonly IStringLocalizer _localizer;

        public ResetAuthenticatorModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ResetAuthenticatorModel> logger,
            IStringLocalizerFactory factory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("IdentityResource", assemblyName.Name);
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return NotFound(_localizer["USER_NOTFOUND", _userManager.GetUserId(User)]);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return NotFound(_localizer["USER_NOTFOUND", _userManager.GetUserId(User)]);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

            await _signInManager.RefreshSignInAsync(user);
            //StatusMessage = "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";
            StatusMessage = _localizer["RESET_AUTHENTICATOR_STATUS"];

            return RedirectToPage("./EnableAuthenticator");
        }
    }
}