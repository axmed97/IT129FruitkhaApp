using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.DTOs;
using WebUI.Models;

namespace WebUI.Controllers;
// DTO - Data Transfer Object
public class AuthController(UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager) : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var findEmail = await userManager.FindByEmailAsync(login.Email);
        if (findEmail == null)
        {
            ViewData["error"] = "Email or Password is not valid";
            return View();
        }

        Microsoft.AspNetCore.Identity.SignInResult result 
            = await signInManager.PasswordSignInAsync(findEmail, login.Password, login.RememberMe, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewData["error"] = "Email or Password is not valid";
            return View();
        }
    }
    [Authorize(Roles = "Admin, Moderator")]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        if(!ModelState.IsValid)
            return View(register);

        AppUser newUser = new()
        {
            FirstName = register.FirstName,
            LastName = register.LastName,
            Email = register.Email,
            UserName = register.Email
        };

        IdentityResult result = await userManager.CreateAsync(newUser, register.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Login");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }
        }
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
