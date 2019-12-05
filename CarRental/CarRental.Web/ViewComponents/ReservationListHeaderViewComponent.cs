using CarRental.Bll.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewComponents
{
    public class ReservationListHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ReservationListHeader> reservations)
        {
            return View(reservations);
        }
    }
}
