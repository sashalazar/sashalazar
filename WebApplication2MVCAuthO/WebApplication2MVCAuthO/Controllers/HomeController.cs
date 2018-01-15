using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication2MVCAuthO.Data;
using WebApplication2MVCAuthO.Models;
using WebApplication2MVCAuthO.Models.HomeViewModels;
using WebApplication2MVCAuthO.Services;

namespace WebApplication2MVCAuthO.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        private Task<ApplicationUser> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string mode = null, string returnUrl = null)
        {
            var appUser = await CurrentUser;
            DriverModel driver = _context.Drivers.Include(m => m.User).FirstOrDefault(m => m.User.Id == appUser.Id);

            if (string.IsNullOrEmpty(appUser.PhoneNumber))
                return RedirectToAction("UserPhone");

            if(mode == "Driver" && (string.IsNullOrEmpty(driver?.CarModel) || string.IsNullOrEmpty(driver?.CarNum)))
                return RedirectToAction("EditInfo");

            return View(appUser);
        }

        //todo remove method
        [Authorize]
        public async Task<IActionResult> DriverLocations([FromRoute] string id, ClientRequestModel clientRequestModel)
        {
            if (string.IsNullOrEmpty(id))
            {
                /////////////////////////////////////////
                if (!string.IsNullOrEmpty(clientRequestModel.Id))
                {
                    HttpContext.Session.SetString("ClientRequestModelId", clientRequestModel.Id);
                }
                /////////////////////////////////////////
                return View((object)id);
            }
            else
                return Error();
        }

        [Authorize]
        public async Task<IActionResult> EditInfo()
        {
            var appUser = await CurrentUser;
            DriverModel driver = _context.Drivers.Include(m => m.User).FirstOrDefault(m => m.User.Id == appUser.Id);
            if (driver == null)
            {
                driver = new DriverModel();
            }
            return View(driver);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditInfo(
            [Required] DriverModel driverModel)
        {
            if (ModelState.IsValid)
            {
                driverModel.User = await CurrentUser;

                if (string.IsNullOrEmpty(driverModel.Id))
                {      
                    await _context.Drivers.AddAsync(driverModel);
                }
                else
                {
                    var driver = _context.Drivers.Find(driverModel.Id);
                    _context.Entry(driver).CurrentValues.SetValues(driverModel);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(driverModel);
        }

        [Authorize]
        public async Task<IActionResult> UserPhone()
        {
            return View(await CurrentUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserPhone(
            [Required] ApplicationUser usrApplicationUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await CurrentUser;
                user.PhoneNumber = usrApplicationUser.PhoneNumber.Trim();
                user.NormPhoneNum = usrApplicationUser.PhoneNumber.Replace("(", "").
                  Replace(")","").Replace("-", "").Replace(".", "").Replace(" ", "");
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(await CurrentUser);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
