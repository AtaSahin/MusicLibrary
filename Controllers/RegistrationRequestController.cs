using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApp.Areas.Identity.Data;
using MusicLibraryApp.Data;
using AspNetCoreRateLimit;

namespace MusicLibraryApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [RateLimit]
    public class RegistrationRequestController : Controller

    {
        private readonly ILogger<RegistrationRequest> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;
        private readonly IEmailService _emailService;

        public RegistrationRequestController(UserManager<ApplicationUser> userManager, ILogger<RegistrationRequest> logger, AuthDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var registrationRequests = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            return View(registrationRequests);
        }


        public async Task<IActionResult> Approve(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Verified");
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                // Send email using RabbitMQ
                await _emailService.SendEmailAsyncWithQueue(user.Email, "Your Account Approved by Admin", "You are now a verified user.");

                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogError("User could not be found");
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Reject(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Rejected User");
                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);


                user.IsDeleted = true;
                await _userManager.UpdateAsync(user);

                // Rejected mesajı gönder
                await _emailService.SendEmailAsyncWithQueue(user.Email, "Account Rejected", "Your account has been rejected by admin.");

                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogError("User could not be found");
            }

            return RedirectToAction(nameof(Index));
        }
    }

}