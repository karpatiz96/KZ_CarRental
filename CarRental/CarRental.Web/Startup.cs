using CarRental.Bll.Configurations;
using CarRental.Bll.IServices;
using CarRental.Bll.Services;
using CarRental.Dal;
using CarRental.Dal.Entities;
using CarRental.Dal.EntityConfigurations;
using CarRental.Dal.SeedInterfaces;
using CarRental.Dal.SeedServices;
using CarRental.Web.Hubs;
using CarRental.Web.Resources;
using CarRental.Web.ViewRender;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace CarRental.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options => 
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
                options.SlidingExpiration = true;
            });

            services.AddScoped<IVehicleModelService, VehicleModelService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IViewRender, ViewRender.ViewRender>();
            services.AddScoped<IRazorViewToStringRender, RazorViewToStringRender>();

            services.AddTransient<ICommentService, CommentService>();

            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddIdentity<User, IdentityRole<int>>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<CarRentalDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<CarRentalDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(CarRentalDbContext))));

            services.AddScoped<IEntityTypeConfiguration<Address>, AddressEntityConfiguration>()
                .AddScoped(provider => new Lazy<IEntityTypeConfiguration<Address>>(() => provider.GetService<IEntityTypeConfiguration<Address>>()))
                .AddScoped<IEntityTypeConfiguration<Car>, CarEntityConfiguration>()
                .AddScoped(provider => new Lazy<IEntityTypeConfiguration<Car>>(() => provider.GetService<IEntityTypeConfiguration<Car>>()))
                .AddScoped<IEntityTypeConfiguration<VehicleModel>, VehicleModelEntityConfiguration>()
                .AddScoped(provider => new Lazy<IEntityTypeConfiguration<VehicleModel>>(() => provider.GetService<IEntityTypeConfiguration<VehicleModel>>()));

            services.AddScoped<ISeedService, SeedService>()
                .AddScoped(provider => new Lazy<ISeedService>(() => provider.GetService<ISeedService>()));

            services.AddScoped<IRoleSeedService, RoleSeedService>();
            services.AddScoped<IUserSeedService, UserSeedService>();

            services.AddAuthentication()
              .AddGoogle(googleOptions => 
              {
                  IConfigurationSection googleSection =
                    Configuration.GetSection("Authentication:Google");

                  googleOptions.ClientId = googleSection["ClientId"];
                  googleOptions.ClientSecret = googleSection["ClientSecret"];
              });

            services.AddTransient<SharedLocalizationService>();
            services.AddTransient<IdentityLocalizationService>();
            services.AddTransient<PagesLocalizationService>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options => 
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("hu-HU")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });

            services.AddSignalR();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddViewLocalization()
                .AddDataAnnotationsLocalization( options => 
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(IdentityResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("IdentityResource", assemblyName.Name);
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseRequestLocalization();
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes => 
            {
                routes.MapHub<VehicleModelsHub>("/vehiclemodelshub"); 
            });

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
