using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NP.Data;
using NP.Models;

namespace NP.Controllers
{
    public class LocationsController : Controller
    {
        private readonly NPDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocationsController(NPDbContext context, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            var nPDbContext = _context.Locations.Include(l => l.AddedBy);
            return View(await nPDbContext.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.AddedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Latitude,Longtitude,Email,Name,Description,Country,City,Img,AddedById,isVerified")] Location location)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            location.Id = GenerateID();
            location.AddedById = currentUser.Id;
            location.AddedBy = currentUser;
            location.isVerified = false;

            if (location.Img == null)
            {
                location.Img = "https://i.postimg.cc/JnywVJzc/loc.png";
            }

            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id", location.AddedById);
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id", location.AddedById);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Latitude,Longtitude,Email,Name,Description,Country,City,Img,AddedById,isVerified")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id", location.AddedById);
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.AddedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Locations == null)
            {
                return Problem("Entity set 'NPDbContext.Locations'  is null.");
            }
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string GenerateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        private bool LocationExists(string id)
        {
          return (_context.Locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
