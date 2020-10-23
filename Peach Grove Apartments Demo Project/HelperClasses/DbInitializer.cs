using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;
using Peach_Grove_Apartments_Demo_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.HelperClasses
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IList<AptUser> _users;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _users = new List<AptUser> {

            new AptUser
                            {

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
                            },
            new AptUser
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
                            },
            new AptUser
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
                            }



            };
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public async Task SeedData()
        {
            await CreateRoles();
            await CreateUsers(_users);
        }


        public async Task CreateRoles()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>())
                {
                    if (!roleManager.Roles.Any())
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
        }

        public async Task CreateUsers(IList<AptUser> users)
        {
            var rDate = new RandomDateTime();

            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    using (var userManager = serviceScope.ServiceProvider.GetService<UserManager<AptUser>>())
                    {
                        if (!context.Users.Any())
                        {
                            foreach (var user in users)
                            {
                                IdentityResult result = await userManager.CreateAsync
                                (user, "Hotel5r@j");

                                if (result.Succeeded)
                                {

                                    if (user.Email.Contains("applicant"))
                                    {
                                        await userManager.AddToRoleAsync(user,
                                                                "Applicant");
                                    }
                                    else if (user.Email.Contains("resident"))
                                    {
                                        await userManager.AddToRoleAsync(user,
                                                                "Resident");
                                    }
                                    else
                                    {
                                        await userManager.AddToRoleAsync(user,
                                                                "Manager");
                                    }

                                    if (!user.Email.Contains("manager"))
                                    {
                                        await context.ElectricBills.AddAsync(new ElectricBill { AptUser = user, Amount = 98.53M, DateDue = rDate.Next() });
                                        await context.WaterBills.AddAsync(new WaterBill { AptUser = user, Amount = 57.23M, DateDue = rDate.Next() });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}


