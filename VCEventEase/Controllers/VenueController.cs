using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VCEventEase.Models;

namespace VCEventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDBContext _context;

        public VenueController(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ViewAll ()
        {
            var Venue = await _context.Venue.ToListAsync();
            return View(Venue);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Venue venues)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venues);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewAll));
            }
            return View(venues);
        }



        //Details about about a single Venue
        //Show a single event details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Venue = await _context.Venue
              
               .FirstOrDefaultAsync(m => m.VenueID == id);

            if (Venue == null)
            {
                return NotFound();
            }
            return View(Venue);
        }

        //Edit Venue

        //Edit the Event 
        private bool VenueExits(int id)
        {
            return _context.Venue.Any(e => e.VenueID == id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Venue = await _context.Venue.FindAsync(id);
            if (Venue == null)
            {
                return NotFound();
            }


            return View(Venue);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue Venue)
        {
            if (id != Venue.VenueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExits(Venue.VenueID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewAll));
            }

            return View(Venue);
        }

        //Delete a venue 
        
        public async Task<IActionResult> Delete(int? id)
        {
            var venue = await _context.Venue
                 
                 .FirstOrDefaultAsync(m => m.VenueID == id);

            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venue.FindAsync(id);
            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewAll));
        }
    }
}
