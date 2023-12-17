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

namespace MusicLibraryApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegistrationRequestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;
        private readonly IEmailService _emailService;

        public RegistrationRequestController(UserManager<ApplicationUser> userManager, AuthDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var registrationRequests = await _context.Users.ToListAsync();
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

                // onaylandı mesajı gönder
                await _emailService.SendEmailAsync(user.Email, "Your Account Approved by Admin", "You are now a verified user.");

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Reject(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);

                // Rejected mesajı gönder
                await _emailService.SendEmailAsync(user.Email, "Account Rejected", "Your account has been rejected by admin.");

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }

}
