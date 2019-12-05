using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarRental.Dal;
using CarRental.Dal.Entities;
using CarRental.Bll.IServices;
using CarRental.Bll.Filters;
using CarRental.Bll.Dtos;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CarRental.Bll.Logging;

namespace CarRental.Web.Pages.VehicleModels
{
    public class IndexModel : PageModel
    {
        private readonly IVehicleModelService _vehicleModelService;

        private readonly ILogger<IndexModel> _logger;

        private readonly UserManager<User> _userManager;

        public PagedResult<VehicleDto> Vehicles { get; private set; }

        [BindProperty(SupportsGet = true)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        public string VehicleType { get; set; }
        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Currency)]
        public decimal? MinPrice { get; set; }
        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Currency)]
        public decimal? MaxPrice { get; set; }

        public IndexModel(IVehicleModelService vehicleModelService, UserManager<User> userManager, ILogger<IndexModel> logger)
        {
            _vehicleModelService = vehicleModelService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnGetAsync(string vehicleType, decimal? minPrice, decimal? maxPrice, string currentVehicleType, decimal? currentMinPrice, decimal? currentMaxPrice, int? pageNumber)
        {
            VehicleModelFilter filter = new VehicleModelFilter();
            filter.Active = true;
            User user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                if(_userManager.IsInRoleAsync(user, "Administrators").Result  || _userManager.IsInRoleAsync(user, "Assistant").Result)
                {
                    filter.Active = false;
                }
            }

            if (vehicleType != null || minPrice != null || maxPrice != null)
            {
                pageNumber = 0;
            }
            else
            {
                if (string.IsNullOrEmpty(vehicleType))
                    vehicleType = currentVehicleType;
                if (minPrice == null)
                    minPrice = currentMinPrice;
                if (maxPrice == null)
                    maxPrice = currentMaxPrice;
            }

            VehicleType = vehicleType;
            MinPrice = minPrice;
            MaxPrice = maxPrice;

            filter.PageNumber = pageNumber ?? 0;
            filter.VehicleType = vehicleType;
            filter.MinPricePerDay = minPrice;
            filter.MaxPricePerDay = maxPrice;

            _logger.LogInformation(LoggingEvents.ListItems, "List VehicleModels");
            Vehicles = await _vehicleModelService.GetVehicles(filter);
        }
    }
}
