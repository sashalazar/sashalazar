using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2MVCAuthO.Data;
using WebApplication2MVCAuthO.Models;
using WebApplication2MVCAuthO.Models.HomeViewModels;

namespace WebApplication2MVCAuthO.Services
{
    public class DataSeeder
    {
        public static async Task SeedDriverLocations(ApplicationDbContext context)
        {
            if (!context.DriverLocations.Any())
            {
                var driverLocations = new List<DriverLocationModel>
                {
                    new DriverLocationModel
                    {
                        Status = "Open",
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now,
                        Latitude = "49.80665133201114",
                        Longitude = "30.12262928356344",
                        User = new ApplicationUser()
                    },
                    new DriverLocationModel
                    {
                        Status = "Open",
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now,
                        Latitude = "49.806602863577126",
                        Longitude = "30.128444312705426",
                        User = new ApplicationUser()
                    },
                    new DriverLocationModel
                    {
                        Status = "Open",
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now,
                        Latitude = "49.80224050589581",
                        Longitude = "30.128390668537122",
                        User = new ApplicationUser()
                    },
                    new DriverLocationModel
                    {
                        Status = "Open",
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now,
                        Latitude = "49.80243439680805",
                        Longitude = "30.122607825900786",
                        User = new ApplicationUser()
                    },
                    new DriverLocationModel
                    {
                        Status = "Open",
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now,
                        Latitude = "49.812394497227444",
                        Longitude = "30.10973322566099",
                        User = new ApplicationUser()
                    }
                };

                for (var i = 0; i < driverLocations.Count; i++)
                {
                    var appUser = new ApplicationUser
                    {
                        //AccessFailedCount = 0,
                        //EmailConfirmed = true,
                        //LockoutEnabled = true,
                        Email = $"user{i}@email{i}.com",
                        UserName = $"User{i}"
                        //PhoneNumberConfirmed = true,
                        //TwoFactorEnabled = false

                    };

                    context.Entry(driverLocations[i].User).CurrentValues.SetValues(appUser);
                }

                context.AddRange(driverLocations);
                await context.SaveChangesAsync();


            }
        }
    }

}
