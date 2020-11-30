using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Peach_Grove_Apartments_Demo_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
            //    dbInitializer.Initialize();
            //    await dbInitializer.SeedData();
            //}

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