using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebUI.Areas.Admin.ViewModels;
using WebUI.Models;

namespace WebUI.Areas.Admin.Controllers;
[Area(nameof(Admin))]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> AddRole(string userId)
    {
        AppUser findUser = await _userManager.FindByIdAsync(userId);
        if (findUser == null) return NotFound();

        var userRole = await _userManager.GetRolesAsync(findUser);

        // select Name from Roles

        IEnumerable<string> roles = await _roleManager.Roles
            .Select(x => x.Name)
            .Except(userRole)
            .ToListAsync();

        UserRoleVM userRoleVM = new()
        {
            AppUser = findUser,
            Roles = roles
        };

        return View(userRoleVM);
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(string userId, string role)
    {
        var findUser = await _userManager.FindByIdAsync(userId);
        await _userManager.AddToRoleAsync(findUser, role);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> EditRole(string userId)
    {
        var findUser = await _userManager.FindByIdAsync(userId);
        return View(findUser);
    }

    public async Task<IActionResult> DeleteRole(string userId, string role)
    {
        var findUser = await _userManager.FindByIdAsync(userId);

        await _userManager.RemoveFromRoleAsync(findUser, role);
        return RedirectToAction("Index");
    }
}
