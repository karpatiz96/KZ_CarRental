using CarRental.Dal.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewComponents
{
    public class CarDtoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<CarDto> cars)
        {
            return View(cars);
        }
    }
}
