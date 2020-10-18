using System;
using System.Collections.Generic;
using System.Text;
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

        public DbSet<Peach_Grove_Apartments_Demo_Project.Models.AptUser> AptUsers { get; set; }
        public DbSet<Peach_Grove_Apartments_Demo_Project.Models.Application> Applications { get; set; }
    }
}
