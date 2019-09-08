using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Web.Controllers
{
    [Authorize(Roles = "Administrators")]
    [Route("Reservations/[action]")]
    public class ReservationsController : Controller
    {
        private readonly CarRentalDbContext _context;

        public ReservationsController(CarRentalDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int? id)
        {
            var reservation = await _context.Reservations.Where(r => r.Id == id).FirstOrDefaultAsync();

            if(reservation == null)
            {
                return NotFound();
            }

            reservation.State = Dal.Entities.Reservation.ReservationStates.Cancled;

            try
            {
                reservation.State = Dal.Entities.Reservation.ReservationStates.Cancled;
                _context.Attach(reservation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return RedirectToPage("/Reservations/List");
        }
    }
}