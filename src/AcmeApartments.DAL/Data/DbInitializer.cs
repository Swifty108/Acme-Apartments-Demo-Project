using AcmeApartments.Common.HelperClasses;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeApartments.DAL.Data
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
                                SSN = "189424545",
                                AptNumber = "3185-335",
                                AptPrice = "850",
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
                                SSN = "189224142",
                                AptNumber = "3185-209",
                                AptPrice = "850",
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
                    context.Database.EnsureCreatedAsync();
                }
            }
        }

        public async Task SeedData()
        {
            await CreateRoles();
            await CreateUsers(_users);
            await CreateFloorPlans();
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
                                (user, "Apt5helix!");

                                if (result.Succeeded)
                                {
                                    if (user.Email.Contains("applicant"))
                                    {
                                        await userManager.AddToRoleAsync(user, "Applicant");
                                    }
                                    else if (user.Email.Contains("resident"))
                                    {
                                        await userManager.AddToRoleAsync(user, "Resident");
                                    }
                                    else
                                    {
                                        await userManager.AddToRoleAsync(user, "Manager");
                                    }

                                    if (!user.Email.Contains("manager"))
                                    {
                                        await context.ElectricBills.AddAsync(new ElectricBill
                                        {
                                            User = user,
                                            Amount = 98.53M,
                                            DateDue = rDate.Next()
                                        });
                                        await context.WaterBills.AddAsync(new WaterBill
                                        {
                                            User = user,
                                            Amount = 57.23M,
                                            DateDue = rDate.Next()
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task CreateFloorPlans()
        {
            var aDate = new RandomDateTime();

            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    if (!context.FloorPlans.Any())
                    {
                        var fpList = new List<FloorPlan>()
                    {
                        new FloorPlan {
                            FloorPlanType = "Studio",
                            AptNumber = "5475-315",
                            DateAvailable = aDate.Next(),
                            SF = "750",
                            Price = "850"  },
                        new FloorPlan {
                            FloorPlanType = "Studio",
                            AptNumber = "5475-720",
                            DateAvailable = aDate.Next(),
                            SF = "750",
                            Price = "850"  },
                        new FloorPlan {
                            FloorPlanType = "Studio",
                            AptNumber = "5475-403",
                            DateAvailable = aDate.Next(),
                            SF = "750",
                            Price = "850"  },
                        new FloorPlan {
                            FloorPlanType = "1Bed",
                            AptNumber = "5495-501",
                            DateAvailable = aDate.Next(),
                            SF = "1050",
                            Price = "1,150"  },
                        new FloorPlan {
                            FloorPlanType = "1Bed",
                            AptNumber = "5495-309",
                            DateAvailable = aDate.Next(),
                            SF = "1050",
                            Price = "1,150"  },
                        new FloorPlan {
                            FloorPlanType = "1Bed",
                            AptNumber = "5495-404",
                            DateAvailable = aDate.Next(),
                            SF = "1050",
                            Price = "1,150"  },
                        new FloorPlan {
                            FloorPlanType = "2Bed",
                            AptNumber = "5475-310",
                            DateAvailable = aDate.Next(),
                            SF = "1050",
                            Price = "1,250"  },
                        new FloorPlan {
                            FloorPlanType = "2Bed",
                            AptNumber = "5475-419",
                            DateAvailable = aDate.Next(),
                            SF = "1050",
                            Price = "1,250"  },
                        new FloorPlan {
                            FloorPlanType = "2Bed",
                            AptNumber = "5475-328",
                            DateAvailable = aDate.Next(),
                            SF = "1050",
                            Price = "1,250"  }
                    };

                        await context.FloorPlans.AddRangeAsync(fpList);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}