using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicLibraryApp;
using MusicLibraryApp.Areas.Identity.Data;
using MusicLibraryApp.Data;
using MusicLibraryApp.Controllers;
public class Program
{

    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
      
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

        builder.Services.AddSession(); 

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

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