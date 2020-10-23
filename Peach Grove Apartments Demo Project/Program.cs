using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Peach_Grove_Apartments_Demo_Project.Models;
using Microsoft.AspNetCore.Identity;
using Peach_Grove_Apartments_Demo_Project.Services;
using Peach_Grove_Apartments_Demo_Project.Data;
using System.ComponentModel.DataAnnotations;

namespace Peach_Grove_Apartments_Demo_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();
         

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var userManager = serviceProvider.
        GetRequiredService<UserManager<AptUser>>();

                    var roleManager = serviceProvider.
        GetRequiredService<RoleManager<IdentityRole>>();

                    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

                    DbInitializer.SeedData
        (userManager, roleManager, dbContext);
                }
                catch
                {

                }
            }


            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
