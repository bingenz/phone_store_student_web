using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartMobileStore.Models;
using SmartMobileStore.ViewModels;
namespace SmartMobileStore.Controllers;

public class AccountController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn) : Controller
{
    [HttpGet] public IActionResult Login(string? returnUrl) => View(new LoginInput { ReturnUrl = returnUrl });
    [HttpPost]
    public async Task<IActionResult> Login(LoginInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var user = await users.FindByEmailAsync(input.Email);
        if (user != null)
        {
            var result = await signIn.PasswordSignInAsync(user, input.Password, false, true);
            if (result.Succeeded) return Url.IsLocalUrl(input.ReturnUrl) ? LocalRedirect(input.ReturnUrl!) : RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Đăng nhập không thành công. Kiểm tra thông tin hoặc thử lại sau nếu bị khóa tạm thời.");
        return View(input);
    }
    [HttpGet] public IActionResult Register() => View(new RegisterInput());
    [HttpPost]
    public async Task<IActionResult> Register(RegisterInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var email = input.Email.Trim();
        var user = new ApplicationUser { Email = email, UserName = email };
        var result = await users.CreateAsync(user, input.Password);
        if (result.Succeeded)
        {
            var role = await users.AddToRoleAsync(user, "User");
            if (!role.Succeeded) { await users.DeleteAsync(user); ModelState.AddModelError("", "Chưa khởi tạo vai trò User. Hãy chạy seed demo."); return View(input); }
            await signIn.SignInAsync(user, false); return RedirectToAction("Index", "Home");
        }
        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        return View(input);
    }
    [Authorize, HttpPost] public async Task<IActionResult> Logout() { await signIn.SignOutAsync(); HttpContext.Session.Clear(); return RedirectToAction("Index", "Home"); }
    public IActionResult Denied() { Response.StatusCode = 403; return View(); }
}
