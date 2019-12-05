using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using CarRental.Bll.IServices;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Dtos;
using CarRental.Bll.Logging;
using CarRental.Bll.Filters;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Web.Pages.Addresses
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IAddressService _addressService;

        private readonly ILogger<IndexModel> _logger;

        private readonly UserManager<User> _userManager;

        public IndexModel(IAddressService addressService, ILogger<IndexModel> logger, UserManager<User> userManager)
        {
            _addressService = addressService;
            _logger = logger;
            _userManager = userManager;
        }

        [BindProperty]
        public PagedResult<AddressDto> Addresses { get; private set; }

        public string NameSort { get; set; }
        public string ZipCodeSort { get; set; }
        public string CitySort { get; set; }
        public string StreetAddressSort { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder, int? pageNumber)
        {
            AddressFilter filter = new AddressFilter();
            CurrentSort = sortOrder;
            ZipCodeSort = string.IsNullOrEmpty(sortOrder) ? "zip_desc" : "";
            CitySort = sortOrder == "City" ? "city_desc" : "City";
            StreetAddressSort = sortOrder == "Street" ? "street_desc" : "Street";
            NameSort = sortOrder == "Name" ? "name_desc" : "Name";

            filter.PageNumber = pageNumber ?? 0;
            filter.Active = true;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if(_userManager.IsInRoleAsync(user, "Administrators").Result || _userManager.IsInRoleAsync(user, "Assistant").Result)
                {
                    filter.Active = false;
                }
            }

            switch (sortOrder)
            {
                case "zip_desc":
                    filter.addressOrder = AddressFilter.AddressOrder.ZipCodeDescending;
                    break;
                case "city_desc":
                    filter.addressOrder = AddressFilter.AddressOrder.CityDescending;
                    break;
                case "City":
                    filter.addressOrder = AddressFilter.AddressOrder.CityAscending;
                    break;
                case "street_desc":
                    filter.addressOrder = AddressFilter.AddressOrder.StreetAddressDescending;
                    break;
                case "Street":
                    filter.addressOrder = AddressFilter.AddressOrder.StreetAddressAscending;
                    break;
                case "name_desc":
                    filter.addressOrder = AddressFilter.AddressOrder.NameDescending;
                    break;
                case "Name":
                    filter.addressOrder = AddressFilter.AddressOrder.NameAscending;
                    break;
                case "":
                    filter.addressOrder = AddressFilter.AddressOrder.ZipCodeAscending;
                    break;
                default:
                    filter.addressOrder = AddressFilter.AddressOrder.ZipCodeAscending;
                    break;
            }

            _logger.LogInformation(LoggingEvents.ListItems, "List Addresses");
            Addresses = await _addressService.GetAddresses(filter);

            return Page();
        }
    }
}
