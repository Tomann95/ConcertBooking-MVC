using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcertBooking.Data;
using ConcertBooking.Models;
using ConcertBooking.Services;

namespace ConcertBooking.Controllers
{
    
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService; 

        
        public ReservationsController(ApplicationDbContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        
        public async Task<IActionResult> Book(int? concertId)
        {
            if (concertId == null) return NotFound();

            var concert = await _context.Concerts.FindAsync(concertId);
            if (concert == null) return NotFound();

            var reservation = new Reservation
            {
                ConcertId = concert.Id,
                Concert = concert 
            };

            return View(reservation);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([Bind("ConcertId")] Reservation reservation)
        {
            var user = await _userManager.GetUserAsync(User);
            var concert = await _context.Concerts.FindAsync(reservation.ConcertId);

            if (concert == null) return NotFound();

            
            if (concert.AvailableSeats <= 0)
            {
                ModelState.AddModelError("", "Przepraszamy, brak wolnych miejsc na to wydarzenie.");
                reservation.Concert = concert;
                return View(reservation);
            }

            
            reservation.UserId = user.Id;
            reservation.ReservationDate = DateTime.Now;
            reservation.Status = "Pending";

            
            concert.AvailableSeats -= 1;
            _context.Update(concert);

            
            
            reservation.Id = 0;

            
            _context.Add(reservation);
            await _context.SaveChangesAsync();

            
            var emailSubject = $"Potwierdzenie rezerwacji: {concert.ArtistName}";
            var emailBody = $@"
        <div style='font-family: sans-serif; padding: 20px;'>
            <h2>Witaj {user.FirstName ?? user.Email},</h2>
            <p>Twoja rezerwacja na wydarzenie <strong>{concert.ArtistName}</strong> (Data: {concert.EventDate:g}) została przyjęta.</p>
            <p>Aktualny status to: <span style='color: #ff007f; font-weight: bold;'>Oczekująca (Pending)</span>.</p>
            <p>Powiadomimy Cię, gdy administrator zatwierdzi Twoją rezerwację.</p>
            <br/>
            <p>Pozdrawiamy,<br/>Zespół ConcertBooking</p>
        </div>";

            try
            {
                await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd wysyłki e-mail: {ex.Message}");
            }

            return RedirectToAction(nameof(MyReservations));
        }

        
        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.GetUserAsync(User);

            
            if (user == null)
            {
                
                return Redirect("/Identity/Account/Login");
            }

            
            var reservations = await _context.Reservations
                .Include(r => r.Concert)
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return View(reservations);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int reservationId)
        {
            var user = await _userManager.GetUserAsync(User);

            
            var reservation = await _context.Reservations
                .Include(r => r.Concert)
                .FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == user.Id);

            if (reservation != null && reservation.Status != "Cancelled")
            {
                
                reservation.Status = "Cancelled";

                
                reservation.Concert.AvailableSeats += 1;
                _context.Update(reservation.Concert);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyReservations));
        }
    }
}