// Import necessary namespaces
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MusicLibraryApp.Areas.Identity.Data;
using System.Linq;
using System.Threading.Tasks;
using MusicLibraryApp.Controllers;

[Authorize(Roles = "Admin")]
[RateLimit]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager, ILogger<AdminController> logger)
    {
        _logger = logger;
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
            TempData["AssignModeratorMessage"] = $"{user.UserName} Successfully assigned as Moderator";
        }
        else
        {
            _logger.LogInformation("**Invalid user Assinged as Moderator**");
        }

        return RedirectToAction("Index");
    }
}
