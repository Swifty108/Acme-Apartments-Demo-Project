using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;
using System;
using System.Linq;

namespace Peach_Grove_Apartments_Demo_Project.Services
{
    public class DbInitializer
    { 

        public static void SeedData
  (UserManager<AptUser> userManager,
  RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager, dbcontext);
        }

        public static async void SeedUsers
    (UserManager<AptUser> userManager, ApplicationDbContext _context)
        {
            if (userManager.FindByNameAsync
        ("john.doe@applicant.com").Result == null)
            {
                var user = new AptUser {

                    UserName = "john.doe@applicant.com",
                    Email = "john.doe@applicant.com",
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = DateTime.Parse("08/29/1976"),
                    StreetAddress = "1295 Lanny Drive",
                    City = "Pittsburgh",
                    State = "PA",
                    Zipcode = "15213",
                    PhoneNumber = "412-555-1212",
                    DateRegistered = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync
                (user, "john4helix").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "Applicant").GetAwaiter().GetResult();

                    var ebill = await _context.ElectricBills.Where(a => a.AptUserId == user.Id).FirstOrDefaultAsync();
                    var wbill = await _context.WaterBills.Where(a => a.AptUserId == user.Id).FirstOrDefaultAsync();

                    if (ebill == null)
                    {
                        await _context.ElectricBills.AddAsync(new ElectricBill { AptUser = user, Amount = 98.53M, DateDue = DateTime.Parse("12/25/2020") });
                    }

                    if (wbill == null)
                    {
                        await _context.WaterBills.AddAsync(new WaterBill { AptUser = user, Amount = 98.53M, DateDue = DateTime.Parse("12/27/2020") });
                    }
                }
            }

            if (userManager.FindByNameAsync
      ("tom.higgins@resident.com").Result == null)
            {
                var user = new AptUser
                {

                    UserName = "tom.higgins@resident.com",
                    Email = "tom.higgins@resident.com",
                    FirstName = "Tom",
                    LastName = "Higgins",
                    DateOfBirth = DateTime.Parse("08/29/1966"),
                    StreetAddress = "907 Laurel Drive",
                    City = "Pittsburgh",
                    State = "PA",
                    Zipcode = "15213",
                    PhoneNumber = "412-555-2222",
                    DateRegistered = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync
                (user, "tom3helix").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "Resident").GetAwaiter().GetResult();

                    var ebill = await _context.ElectricBills.Where(a => a.AptUserId == user.Id).FirstOrDefaultAsync();
                    var wbill = await _context.WaterBills.Where(a => a.AptUserId == user.Id).FirstOrDefaultAsync();

                    if (ebill == null)
                    {
                        await _context.ElectricBills.AddAsync(new ElectricBill { AptUser = user, Amount = 98.53M, DateDue = DateTime.Parse("12/25/2020") });
                    }

                    if (wbill == null)
                    {
                        await _context.WaterBills.AddAsync(new WaterBill { AptUser = user, Amount = 98.53M, DateDue = DateTime.Parse("12/27/2020") });
                    }
                }
            }

            if (userManager.FindByNameAsync
       ("jamie.jackson@manager.com").Result == null)
            {
                var user = new AptUser
                {

                    UserName = "jamie.jackson@manager.com",
                    Email = "jamie.jackson@manager.com",
                    FirstName = "Jamie",
                    LastName = "Jackson",
                    DateOfBirth = DateTime.Parse("03/16/1987"),
                    StreetAddress = "3496 Larkinson Drive",
                    City = "Pittsburgh",
                    State = "PA",
                    Zipcode = "15222",
                    PhoneNumber = "412-555-4444",
                    DateRegistered = DateTime.Now
                };

                IdentityResult result = userManager.CreateAsync
                (user, "jamie5helix").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "Manager").GetAwaiter().GetResult();
                }
            }
        }

        public static void SeedRoles
   (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync
        ("Applicant").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Applicant";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
        ("Resident").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Resident";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync
        ("Manager").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Manager";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
