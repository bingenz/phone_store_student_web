using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
namespace SmartMobileStore.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = "Admin")]
public class ContactController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.ContactMessages.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync());
    [HttpPost]
    public async Task<IActionResult> Handle(int id)
    {
        var message = await db.ContactMessages.FindAsync(id); if (message == null) return NotFound();
        message.IsHandled = true; await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
}
