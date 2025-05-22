using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUI.Models;

namespace WebUI.ViewComponents;

public class AdminViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<AppUser> _userManager;
    public AdminViewComponent(IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = await _userManager.FindByIdAsync(userId);
        return View("Admin", user);
    }
}
