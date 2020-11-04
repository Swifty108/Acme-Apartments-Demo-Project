using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peach_Grove_Apartments_Demo_Project.Data;
using Peach_Grove_Apartments_Demo_Project.Models;

[assembly: HostingStartup(typeof(Peach_Grove_Apartments_Demo_Project.Areas.Identity.IdentityHostingStartup))]
namespace Peach_Grove_Apartments_Demo_Project.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}