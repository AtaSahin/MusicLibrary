using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicLibraryApp;
using MusicLibraryApp.Areas.Identity.Data;
using MusicLibraryApp.Data;
using MusicLibraryApp.Controllers;
using AspNetCoreRateLimit;
using MusicLibraryApp.Service;
using System.Reflection;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using AutoMapper;
public class Program
{

    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAutoMapper(typeof(Program));
        #region Localizer
        builder.Services.AddSingleton<LanguageService>();
      
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddMvc()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider=(type,factory)=>
            {
               var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                return factory.Create(nameof(SharedResource),assemblyName.Name);
            });
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("tr-TR"),
            };
          
            options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR",uiCulture:"tr-TR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0,new QueryStringRequestCultureProvider());
        });

        var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ??
          throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

        builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<AuthDbContext>();
        builder.Services.AddTransient<IEmailService, SmtpEmailService>();
        builder.Services.AddHttpClient(); 
        builder.Services.AddSingleton<LastFmService>();

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.Configure<IdentityOptions>(options => {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;

        });
        // Rate Limit Kütphane Konfigürasyonlarý
        builder.Services.AddMemoryCache();
        builder.Services.Configure<IpRateLimitOptions>(options =>
        {
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 200, 
                    Period = "1m" // 5 dakikada 5 istek
                }
            };
        });

        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        // Session Kütüphanesi Konfigürasyonlarý

        builder.Services.AddSession(); 

        var app = builder.Build();
 
        app.UseIpRateLimiting();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseRouting();

        app.UseAuthorization();

        app.UseSession(); 

        app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
        using (var scope = app.Services.CreateScope())
        {

            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] {
        "Admin",
        "Moderator",
        "Verified",//Onaylý kullanýcý (Admin tarafýndan onaylanmýþ)
        "User",
        "Rejected User" //Reddedilmiþ kullanýcý (Admin tarafýndan reddedilmiþ)
      };
            foreach (var role in roles)
            {
                if (!await RoleManager.RoleExistsAsync(role))
                    await RoleManager.CreateAsync(new IdentityRole(role));
            }

        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string adminEmail = "admin@admin.com";
            string adminPassword = "Admin123!";

            string moderatorEmail = "moderator@moderator.com";
            string moderatorPassword = "moderator123";

            //  admin user hesabý yoksa oluþtur
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "Admin",
                    LastName = "Admin",
                };

                await userManager.CreateAsync(adminUser, adminPassword);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Moderator user hesabý yoksa oluþtur
            if (await userManager.FindByEmailAsync(moderatorEmail) == null)
            {
                var moderatorUser = new ApplicationUser
                {
                    Email = moderatorEmail,
                    UserName = moderatorEmail,
                    FirstName = "Moderator",
                    LastName = "Moderator",
                };

                await userManager.CreateAsync(moderatorUser, moderatorPassword);
                await userManager.AddToRoleAsync(moderatorUser, "Moderator");
            }
        }
     

        app.Run();
    }
}
#endregion