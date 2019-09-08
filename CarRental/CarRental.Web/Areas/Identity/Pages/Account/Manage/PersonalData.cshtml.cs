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
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly IStringLocalizer _localizer;

        public PersonalDataModel(
            UserManager<User> userManager,
            ILogger<PersonalDataModel> logger,
            IStringLocalizerFactory factory)
        {
            _userManager = userManager;
            _logger = logger;
            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("IdentityResource", assemblyName.Name);
        }

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
    }
}