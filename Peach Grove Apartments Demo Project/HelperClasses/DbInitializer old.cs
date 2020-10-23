using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.HelperClasses

{
    public class DbInitializerold

    { 

        public static async Task SeedData
  (UserManager<AptUser> userManager,
  RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            SeedRoles(roleManager);
            await SeedUsers(userManager, dbcontext);
        }


        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    //initializing custom roles 
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] roleNames = { "Admin", "Manager", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            //create the roles and seed them to the database: Question 1
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }

        //    //Here you could create a super user who will maintain the web app
        //    var poweruser = new ApplicationUser
        //    {

        //        UserName = Configuration["AppSettings:UserName"],
        //        Email = Configuration["AppSettings:UserEmail"],
        //    };
        //    //Ensure you have these values in your appsettings.json file
        //    string userPWD = Configuration["AppSettings:UserPassword"];
        //    var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

        //    if (_user == null)
        //    {
        //        var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
        //        if (createPowerUser.Succeeded)
        //        {
        //            //here we tie the new user to the role
        //            await UserManager.AddToRoleAsync(poweruser, "Admin");

        //        }
        //    }
        //}



        public static async Task SeedUsers
    (UserManager<AptUser> userManager, ApplicationDbContext _context)
        {
            if (await userManager.FindByNameAsync
        ("john.doe@applicant.com") == null)
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

                IdentityResult result = await userManager.CreateAsync
                (user, "Hotel5r@j");

                if (result.Succeeded)
                {
                   await userManager.AddToRoleAsync(user,
                                        "Applicant");

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

            if (await userManager.FindByNameAsync
      ("tom.higgins@resident.com") == null)
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

                IdentityResult result = await userManager.CreateAsync
                (user, "Hotel5r@j");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user,
                                        "Resident");

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

            if (await userManager.FindByNameAsync
       ("jamie.jackson@manager.com") == null)
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

                IdentityResult result = await userManager.CreateAsync
                (user, "Hotel5r@j");

                if (result.Succeeded)
                {
                   await userManager.AddToRoleAsync(user,
                                        "Manager");
                }
            }
        }

        public static async Task SeedRoles
   (RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync
        ("Applicant"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Applicant";
                IdentityResult roleResult = await roleManager.
                CreateAsync(role);
            }


            if (!await roleManager.RoleExistsAsync
        ("Resident"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Resident";
                IdentityResult roleResult = await roleManager.
                CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync
        ("Manager"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Manager";
                IdentityResult roleResult = await roleManager.
                CreateAsync(role);
            }
        }
    }
}
