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

        private Task<ApplicationUser> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        public HomeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            //var curUser = CurrentUser;
            return View(await CurrentUser);
        }

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
