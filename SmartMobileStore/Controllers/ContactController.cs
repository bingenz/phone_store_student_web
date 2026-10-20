using Microsoft.AspNetCore.Mvc;
using SmartMobileStore.Data;
using SmartMobileStore.Models;
namespace SmartMobileStore.Controllers;

public class ContactController(StoreDbContext db) : Controller
{
    [HttpGet] public IActionResult Index() => View(new ContactMessage());
    [HttpPost]
    public async Task<IActionResult> Index([Bind("Name,Email,Phone,Message")] ContactMessage input)
    {
        if (!ModelState.IsValid) return View(input);
        db.ContactMessages.Add(input); await db.SaveChangesAsync();
        TempData["Success"] = "Đã lưu yêu cầu liên hệ. Website demo không gửi email tự động.";
        return RedirectToAction(nameof(Index));
    }
}
