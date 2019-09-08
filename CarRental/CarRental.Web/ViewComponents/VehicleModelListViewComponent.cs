using CarRental.Dal.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewComponents
{
    public class VehicleModelListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PagedResult<VehicleDto> vehicles)
        {
            return View(vehicles);
        }
    }
}
