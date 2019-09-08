using CarRental.Dal.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewComponents
{
    public class VehicleDtoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(VehicleDto vehicle)
        {
            return View(vehicle);
        }
    }
}
