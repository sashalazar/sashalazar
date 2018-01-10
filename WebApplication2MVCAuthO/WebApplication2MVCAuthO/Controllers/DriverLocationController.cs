using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2MVCAuthO.Data;
using WebApplication2MVCAuthO.Models.HomeViewModels;

namespace WebApplication2MVCAuthO.Controllers
{
    public class DriverLocationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriverLocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DriverLocation
        public async Task<IActionResult> Index()
        {
            return View(await _context.DriverLocationModel.Where(m => m.User != null).ToListAsync());
        }

        // GET: DriverLocation/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverLocationModel = await _context.DriverLocationModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (driverLocationModel == null)
            {
                return NotFound();
            }

            return View(driverLocationModel);
        }

        // GET: DriverLocation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DriverLocation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Latitude,Longitude")] DriverLocationModel driverLocationModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driverLocationModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driverLocationModel);
        }

        // GET: DriverLocation/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverLocationModel = await _context.DriverLocationModel.SingleOrDefaultAsync(m => m.Id == id);
            if (driverLocationModel == null)
            {
                return NotFound();
            }
            return View(driverLocationModel);
        }

        // POST: DriverLocation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Latitude,Longitude")] DriverLocationModel driverLocationModel)
        {
            if (id != driverLocationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driverLocationModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverLocationModelExists(driverLocationModel.Id))
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
            return View(driverLocationModel);
        }

        // GET: DriverLocation/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverLocationModel = await _context.DriverLocationModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (driverLocationModel == null)
            {
                return NotFound();
            }

            return View(driverLocationModel);
        }

        // POST: DriverLocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var driverLocationModel = await _context.DriverLocationModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.DriverLocationModel.Remove(driverLocationModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverLocationModelExists(string id)
        {
            return _context.DriverLocationModel.Any(e => e.Id == id);
        }
    }
}
