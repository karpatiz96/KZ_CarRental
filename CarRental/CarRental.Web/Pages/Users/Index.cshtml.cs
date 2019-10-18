using CarRental.Bll.Dtos;
using CarRental.Bll.Filters;
using CarRental.Bll.IServices;
using CarRental.Bll.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CarRental.Web.Pages.Users
{
    [Authorize(Roles = "Administrators")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IUserService userService, ILogger<IndexModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public PagedResult<UserDto> Users { get; private set; }

        public string IdSort { get; set; }
        public string NameSort { get; set; }
        public string EmailSort { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder, int? pageNumber)
        {
            UserFilter filter = new UserFilter();
            CurrentSort = sortOrder;
            IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            NameSort = sortOrder == "Name" ? "name_desc" : "Name";
            EmailSort = sortOrder == "Email" ? "email_desc" : "Email";

            filter.PageNumber = pageNumber ?? 0;

            switch (sortOrder)
            {
                case "id_desc":
                    filter.userOrder = UserFilter.UserOrder.IdDescending;
                    break;
                case "name_desc":
                    filter.userOrder = UserFilter.UserOrder.NameDescending;
                    break;
                case "Name":
                    filter.userOrder = UserFilter.UserOrder.NameAscending;
                    break;
                case "email_desc":
                    filter.userOrder = UserFilter.UserOrder.EmailDescending;
                    break;
                case "Email":
                    filter.userOrder = UserFilter.UserOrder.EmailAscending;
                    break;
                case "":
                    filter.userOrder = UserFilter.UserOrder.IdAscending;
                    break;
                default:
                    filter.userOrder = UserFilter.UserOrder.IdAscending;
                    break;
            }

            _logger.LogInformation(LoggingEvents.ListItems, "List Users");

            Users = await _userService.GetUsersAsync(filter);

            return Page();
        }
    }
}
