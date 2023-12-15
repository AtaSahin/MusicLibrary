using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApp.Areas.Identity.Data;
using MusicLibraryApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibraryApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegistrationRequestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;

        public RegistrationRequestController(UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

                //Onaylı kullanıcı...
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

            
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
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
