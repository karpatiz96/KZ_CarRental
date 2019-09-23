using CarRental.Bll.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewComponents
{
    public class AddressDtoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<AddressDto> address)
        {
            return View(address);
        }
    }
}
