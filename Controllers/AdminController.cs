using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MusicLibraryApp.Areas.Identity.Data;
using System.Linq;

[Authorize(Roles = "Admin")] // Admin rolüne sahip kullanıcılar için sınırlama
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }
    [HttpPost]
    public async Task<IActionResult> AssignModerator(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, "Moderator");
        }

        return RedirectToAction("Index");
    }
}
