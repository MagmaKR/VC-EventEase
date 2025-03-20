using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VCEventEase.Models;

namespace VCEventEase.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public BookingsController(ApplicationDBContext context)
        {
            _context = context;
        }
        //show all bookings
        public async Task<IActionResult> Index()
        {
            var Bookings = await _context.Booking
                 .Include(e => e.Venue)
                 .Include(e => e.Event)
                 .ToListAsync();

            return View(Bookings);

        }

        // Create Booking
        public IActionResult Create()
        {
            ViewBag.EventName = new SelectList(_context.Event, "EventID", "EventName");
            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If validation fails, repopulate the dropdown
            ViewBag.EventName = new SelectList(_context.Event, "EventID", "EventName", booking.EventId);
            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName", booking.VenueId);
            return View(booking);
        }


        //show a single booking detail
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookings = await _context.Booking
               .Include(e => e.Venue)
               .Include(e => e.Event)
               .FirstOrDefaultAsync(m => m.BookingId == id);

            if (bookings == null)
            {
                return NotFound();
            }
            return View(bookings);
        }

        //Delete a booking
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bookings = await _context.Booking
                .Include(e => e.Venue)
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (bookings == null)
            {
                return NotFound();
            }
            return View(bookings);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bookings = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(bookings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //edit a booking
        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Booking = await _context.Booking.FindAsync(id);
            if (Booking == null)
            {
                return NotFound();
            }

            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName", Booking.VenueId);
            ViewBag.EventName = new SelectList(_context.Event, "EventID", "EventName", Booking.EventId);

            return View(Booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking Booking)
        {
            if (id != Booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(Booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If we get here, there was a validation error, so repopulate the dropdown
            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName", Booking.VenueId);
            ViewBag.EventName = new SelectList(_context.Event, "EventID", "EventName", Booking.EventId);
            
            return View(Booking);
        }


    }
}
