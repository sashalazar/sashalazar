using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2MVCAuthO.Data;
using WebApplication2MVCAuthO.Models.HomeViewModels;

namespace WebApplication2MVCAuthO.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders
                .SingleOrDefaultAsync(m => m.Id == id);
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                //return RedirectToAction(nameof(Index));
            }

            var clientRequestId = orderModel.ClientRequest?.Id;
            var driverLocationId = orderModel.DriverLocation?.Id;

            if (string.IsNullOrEmpty(clientRequestId) || string.IsNullOrEmpty(driverLocationId))
            {
                return BadRequest();
            }

            var clientRequest = _context.ClientRequests.Find(clientRequestId);
            _context.Entry(orderModel.ClientRequest).CurrentValues.SetValues(clientRequest);

            var driverLocation = _context.DriverLocations.Find(driverLocationId);
            _context.Entry(orderModel.DriverLocation).CurrentValues.SetValues(driverLocation);

            // to do make enum Status
            orderModel.Status = "Open";
            orderModel.CreatDate = DateTime.Now;
            orderModel.UpdStatusDate = DateTime.Now;

            _context.Add(orderModel);
            if (!string.IsNullOrEmpty(orderModel.Id))
            {
                HttpContext.Session.SetString("orderModelId", orderModel.Id);
            }

            await _context.SaveChangesAsync();

            return PartialView("Details", orderModel);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (orderModel == null)
            {
                return NotFound();
            }
            return View(orderModel);
        }

        // POST: OrderModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Status,CreatDate,UpdStatusDate")] OrderModel orderModel)
        {
            if (id != orderModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderModelExists(orderModel.Id))
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
            return View(orderModel);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders
                .SingleOrDefaultAsync(m => m.Id == id);
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        // POST: OrderModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var orderModel = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            _context.Orders.Remove(orderModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderModelExists(string id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
