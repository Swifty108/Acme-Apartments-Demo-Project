using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeachGroveApartments.ApplicationLayer.HelperClasses;
using PeachGroveApartments.ApplicationLayer.Interfaces;
using PeachGroveApartments.Common.HelperClasses;
using PeachGroveApartments.Core.Interfaces;
using PeachGroveApartments.Infrastructure.Data;
using PeachGroveApartments.Infrastructure.Identity;
using PeachGroveApartments.Infrastructure.Inerfaces;
using PeachGroveApartments.Infrastructure.Interfaces;
using PeachGroveApartments.Infrastructure.Services;

namespace Peach_Grove_Apartments_Demo_Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnvironment { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AptUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false; options.User.RequireUniqueEmail = true; options.Password.RequireLowercase = false; options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddRoles<IdentityRole>();

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IResidentRepository, ResidentRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IManagerLogic, ManagerLogic>();
            services.AddScoped<IApplicantLogic, ApplicantLogic>();
            services.AddScoped<IResidentLogic, ResidentLogic>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAutoMapper(c => c.AddProfile<AutoMappingProfile>(), typeof(Startup));

            // services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            //services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IMailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "register",
              pattern: "identity/pages/account/login/{isdirectregister?}");

                endpoints.MapControllerRoute(name: "login",
             pattern: "identity/pages/account/login/{isdirectlogin?}");

                endpoints.MapRazorPages();
            });

            //Todo-p: move this to main method in the program.cs file
            //var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            //using (var scope = scopeFactory.CreateScope())
            //{
            //    var dbInitializer = scope.ServiceProvider.GetService<DbInitializer>();
            //    dbInitializer.Initialize();
            //    dbInitializer.SeedData();
            //}
        }
    }
}