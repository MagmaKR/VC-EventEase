using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VCEventEase.Models;

namespace VCEventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDBContext _context;

        public EventController(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var Event = await _context.Event
                 .Include(e => e.Venue)
                 .ToListAsync();

                 return View(Event);

        }

        //Create Event
        public IActionResult Create()
        {
            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, repopulate the dropdown
            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName", eventModel.VenueID);

            return View(eventModel);
        }


        //Show a single event details
        public async Task<IActionResult> Details(int? id)
        {
             if (id == null)
            {
                return NotFound();
            }

             var Event = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (Event == null)
            {
                return NotFound();
            }
            return View(Event);
        }

        //Delete an event 
        public async Task<IActionResult> Delete(int? id)
        {
           var Event = await _context.Event
                .Include(e => e.Venue) 
                .FirstOrDefaultAsync(m => m.EventID == id);
           
            if (Event == null)
            {
                return NotFound();
            }
            return View(Event);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var Event = await _context.Event.FindAsync(id);
            _context.Event.Remove(Event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Edit the Event 
        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Event = await _context.Event.FindAsync(id);
            if (Event == null)
            {
                return NotFound();
            }

            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName", Event.VenueID);


            return View(Event);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event Event)
        {
            if (id != Event.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(Event.EventID))
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
            ViewBag.VenueName = new SelectList(_context.Venue, "VenueID", "VenueName", Event.VenueID);

            return View(Event);
        }


    }
}
