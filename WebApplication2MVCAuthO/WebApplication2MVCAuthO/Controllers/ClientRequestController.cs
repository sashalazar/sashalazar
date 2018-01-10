using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using WebApplication2MVCAuthO.Data;
using WebApplication2MVCAuthO.Models;
using WebApplication2MVCAuthO.Models.HomeViewModels;


namespace WebApplication2MVCAuthO.Controllers
{
    [Produces("application/json")]
    [Route("api/ClientRequest")]
    public class ClientRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;

        //private Task<ApplicationUser> CurrentUser =>
        //    _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        //todo remove userManager
        public ClientRequestController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            //_userManager = userManager;
        }

        // GET: api/ClientRequest
        [HttpGet]
        public IEnumerable<ClientRequestModel> GetClientRequests()
        {
            return _context.ClientRequests;
        }

        // GET: api/ClientRequest/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientRequestModel([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientRequestModel = await _context.ClientRequests.SingleOrDefaultAsync(m => m.Id == id);

            if (clientRequestModel == null)
            {
                return NotFound();
            }

            return Ok(clientRequestModel);
        }

        // PUT: api/ClientRequest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientRequestModel([FromRoute] string id, [FromBody] ClientRequestModel clientRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientRequestModel.Id)
            {
                return BadRequest();
            }

            //clientRequestModel.User.Id = clientRequestModel.User.Id.Trim().ToUpper();
            //_context.Entry(clientRequestModel.User).CurrentValues.SetValues(_context.Users.Find(clientRequestModel.User.Id));

            _context.Entry(clientRequestModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientRequestModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClientRequest
        [HttpPost]
        public async Task<IActionResult> PostClientRequestModel([FromBody] ClientRequestModel clientRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(clientRequestModel.User.Id))
            {
                return BadRequest();
            }
            
            var user = _context.Users.Find(clientRequestModel.User.Id);
            _context.Entry(clientRequestModel.User).CurrentValues.SetValues(user);

            _context.ClientRequests.Add(clientRequestModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientRequestModel", new { id = clientRequestModel.Id }, clientRequestModel);
        }

        // DELETE: api/ClientRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientRequestModel([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientRequestModel = await _context.ClientRequests.SingleOrDefaultAsync(m => m.Id == id);
            if (clientRequestModel == null)
            {
                return NotFound();
            }

            _context.ClientRequests.Remove(clientRequestModel);
            await _context.SaveChangesAsync();

            return Ok(clientRequestModel);
        }

        private bool ClientRequestModelExists(string id)
        {
            return _context.ClientRequests.Any(e => e.Id == id);
        }
    }
}