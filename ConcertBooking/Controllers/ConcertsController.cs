using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcertBooking.Data;
using ConcertBooking.Models;
using Microsoft.AspNetCore.Authorization;

namespace ConcertBooking.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly ApplicationDbContext _context;

        
        public ConcertsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var concerts = await _context.Concerts.ToListAsync();
            return View(concerts);
        }

        
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Concert concert)
        
        {
            if (ModelState.IsValid)
            {
                _context.Add(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            return View(concert); 
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            var concerts = from c in _context.Concerts select c;

            if (!string.IsNullOrEmpty(query))
            {
                
                concerts = concerts.Where(c => c.ArtistName.Contains(query));
            }

            
            return PartialView("_ConcertListPartial", await concerts.ToListAsync());
        }
        
        [AllowAnonymous]
        public IActionResult Calendar()
        {
            return View();
        }

        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetConcertsForCalendar()
        {
            var concerts = await _context.Concerts.ToListAsync();

            
            var events = concerts.Select(c => new
            {
                id = c.Id,
                title = c.ArtistName,
                
                start = c.EventDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                
                url = Url.Action("Book", "Reservations", new { concertId = c.Id })
            });

            return Json(events);
        }
    }
}