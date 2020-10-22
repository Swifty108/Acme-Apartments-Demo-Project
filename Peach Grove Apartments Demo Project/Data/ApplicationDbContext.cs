using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Peach_Grove_Apartments_Demo_Project.Models;

namespace Peach_Grove_Apartments_Demo_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<AptUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AptUser> AptUsers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<WaterBill> WaterBills { get; set; }
        public DbSet<ElectricBill> ElectricBills { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    }
}
