using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AcmeApartments.Web.Areas.Identity.IdentityHostingStartup))]

namespace AcmeApartments.Web.Areas.Identity
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