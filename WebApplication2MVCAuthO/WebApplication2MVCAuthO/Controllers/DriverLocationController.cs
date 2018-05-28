using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication2MVCAuthO.Data;
using WebApplication2MVCAuthO.Models.HomeViewModels;
using WebApplication2MVCAuthO.Services;

namespace WebApplication2MVCAuthO.Controllers
{
    [Authorize]
    public class DriverLocationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public DriverLocationController(ApplicationDbContext context,
            IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        // GET: DriverLocations for special client request (id)
        // function getDriverLocations(requestId) - request
        public async Task<IActionResult> Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                HttpContext.Session.SetString("ClientRequestModelId", id);
            }

            var clientRequest = await _context.ClientRequests.SingleOrDefaultAsync(m => m.Id == id);
            if (clientRequest == null)
            {
                return NotFound();
            }

            var model = await _context.DriverLocations.Include(m => m.User)
                .Where(m => m.User != null && m.Status == "Open" && DistanceIsOk(m, clientRequest)).ToListAsync();
                //.GroupBy(m => m.User.Id, r => new {updDate = r.UpdDate}).Select(g => new {UserId=g.Key, UpdDate = g.Max(m => m.updDate)}).ToListAsync();

            var strUserIds = "";

            model.ForEach(m =>
            {
                if (strUserIds != "") strUserIds += ",";
                strUserIds += $"'{m.User.Id}'";
            });

            if (model.Count > 0)
            {
                var latestLocIdList = LatestLocations(strUserIds);
                model = model.FindAll(m => latestLocIdList.Contains(m.Id));
            }

            return PartialView("Index", model);

            //return View("Index1", model);
        }

        public List<string> LatestLocations(string strUserIds)
        {
            var latestLocIDs = new List<string>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand
                {
                    CommandText = $@"WITH locations AS (SELECT id, userid, UpdDate  FROM driverlocations
                    Where userid in ({strUserIds}) )
                    select dl.id as id 
                    from locations dl inner join
                    (select userid, max(UpdDate) as UpdDate
                    from locations
                    group by userid) gb on dl.userid = gb.userid and dl.UpdDate = gb.UpdDate",
                    CommandType = CommandType.Text,
                    Connection = connection
                })
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        latestLocIDs.Add(reader["id"] as string);
                    }                    
                }
            }

            return latestLocIDs;
        }

        // todo use it in further
        public bool ActiveByLastUpdate(DriverLocationModel dr)
        {
            var isActive = true;
            var lastUpdTime = dr.UpdDate;
            var nowDateTime = DateTime.Now;

            var timeDiff = (nowDateTime - lastUpdTime).Seconds;
            if (timeDiff > 300)
            {
                isActive = false;
            }

            return isActive;
        }

        public bool DistanceIsOk(DriverLocationModel dr, ClientRequestModel cr)
        {
            var distance = 100D;
            var isLat1 = double.TryParse(dr.Latitude, out var lat1);
            var isLng1 = double.TryParse(dr.Longitude, out var lng1);
            var isLat2 = double.TryParse(cr.Latitude, out var lat2);
            var isLng2 = double.TryParse(cr.Longitude, out var lng2);

            if (isLat1 && isLng1 && isLat2 && isLng2)
            {
                distance = FuncHelper.Distance(lat1, lng1, lat2, lng2, 2);
            }

            if (distance > 5D) return false;

            return true;
        }

        // GET: DriverLocation/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //DriverModel driver;

            if (id == null)
            {
                return NotFound();
            }

            var driverLocationModel = await _context.DriverLocations.Include(m => m.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (driverLocationModel == null)
            {
                return NotFound();
            }
            //else
            //{
            //    driver = await _context.Drivers.Include(m => m.User)
            //        .SingleOrDefaultAsync(m => m.User.Id == driverLocationModel.User.Id);
            //}

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
        public async Task<IActionResult> Create([FromBody] DriverLocationModel driverLocationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(driverLocationModel.User.Id))
            {
                return BadRequest();
            }

            var user = _context.Users.Find(driverLocationModel.User.Id);
            _context.Entry(driverLocationModel.User).CurrentValues.SetValues(user);

            // to do make enum Status
            driverLocationModel.Status = "Open";
            driverLocationModel.InsDate = DateTime.Now;
            driverLocationModel.UpdDate = DateTime.Now;

            var driverLocationModelId = HttpContext.Session.GetString("DriverLocationModelId");
            if (string.IsNullOrEmpty(driverLocationModelId))
            {
                _context.Add(driverLocationModel);
                if (!string.IsNullOrEmpty(driverLocationModel.Id))
                {
                    HttpContext.Session.SetString("DriverLocationModelId", driverLocationModel.Id);
                }
            }
            else
            {
                driverLocationModel.Id = driverLocationModelId;
                //_context.Update(driverLocationModel);
            }

            await _context.SaveChangesAsync();

            return PartialView("Display", driverLocationModel);
        }

        //// GET: DriverLocation/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var driverLocationModel = await _context.DriverLocations.SingleOrDefaultAsync(m => m.Id == id);
        //    if (driverLocationModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(driverLocationModel);
        //}

        // POST: DriverLocation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] DriverLocationModel driverLocationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driverLocationModel.Id)
            {
                return NotFound();
            }

            // get object from db
            var driverLocation = _context.DriverLocations.Find(id);

            //check status
            if (driverLocation.Status != driverLocationModel.Status)
            {
                driverLocation.Status = driverLocationModel.Status;
                driverLocation.UpdDate = DateTime.Now;
            }

            if (driverLocation.Status != "Closed")
            {
                driverLocation.Latitude = driverLocationModel.Latitude;
                driverLocation.Longitude = driverLocationModel.Longitude;
            }

            try
            {
                _context.Update(driverLocation);
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
            return PartialView("Display", driverLocation);
        }

        // GET: DriverLocation/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverLocationModel = await _context.DriverLocations
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
            var driverLocationModel = await _context.DriverLocations.SingleOrDefaultAsync(m => m.Id == id);
            _context.DriverLocations.Remove(driverLocationModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverLocationModelExists(string id)
        {
            return _context.DriverLocations.Any(e => e.Id == id);
        }

        //public double Distance(double Latitude1, double Longitude1, double Latitude2, double Longitude2, int type)
        //{
        //    //1- miles
        //    double R = (type == 1) ? 3960 : 6371;          // R is earth radius.
        //    double dLat = this.toRadian(Latitude2 - Latitude1);
        //    double dLon = this.toRadian(Longitude2 - Longitude1);

        //    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(this.toRadian(Latitude1)) * Math.Cos(this.toRadian(Latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        //    double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
        //    double d = R * c;

        //    return d;
        //}

        //private double toRadian(double val)
        //{
        //    return (Math.PI / 180) * val;
        //}

        //private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        //{
        //    double theta = lon1 - lon2;
        //    double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
        //    dist = Math.Acos(dist);
        //    dist = rad2deg(dist);
        //    dist = dist * 60 * 1.1515;
        //    if (unit == 'K')
        //    {
        //        dist = dist * 1.609344;
        //    }
        //    else if (unit == 'N')
        //    {
        //        dist = dist * 0.8684;
        //    }
        //    return (dist);
        //}

        ////:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        ////::  This function converts decimal degrees to radians             :::
        ////:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //private double deg2rad(double deg)
        //{
        //    return (deg * Math.PI / 180.0);
        //}

        ////:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        ////::  This function converts radians to decimal degrees             :::
        ////:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //private double rad2deg(double rad)
        //{
        //    return (rad / Math.PI * 180.0);
        //}

    }
}
