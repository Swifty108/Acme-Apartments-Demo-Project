using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Peach_Grove_Apartments_Demo_Project.Areas.Identity.IdentityHostingStartup))]

namespace Peach_Grove_Apartments_Demo_Project.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}