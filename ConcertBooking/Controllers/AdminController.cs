using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcertBooking.Data;

namespace ConcertBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            
            var reservations = await _context.Reservations
                .Include(r => r.Concert)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            
            ViewBag.TotalPending = reservations.Count(r => r.Status == "Pending");
            ViewBag.TotalApproved = reservations.Count(r => r.Status == "Approved");

            return View(reservations);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string newStatus)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                reservation.Status = newStatus;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}