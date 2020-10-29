using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Peach_Grove_Apartments_Demo_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();
         

        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var serviceProvider = scope.ServiceProvider;
        //        try
        //        {
        //            var userManager = serviceProvider.
        //GetRequiredService<UserManager<AptUser>>();

        //            var roleManager = serviceProvider.
        //GetRequiredService<RoleManager<IdentityRole>>();

        //            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        //            await DbInitializer.SeedData
        //(userManager, roleManager, dbContext);
        //        }
        //        catch
        //        {

        //        }
        //    }


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
