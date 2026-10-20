using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
using SmartMobileStore.Models;
namespace SmartMobileStore.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = "Admin")]
public class CategoriesController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync());
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        var category = id == null ? new Category() : await db.Categories.FindAsync(id);
        return category == null ? NotFound() : View(category);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([Bind("Id,Name,Slug")] Category input)
    {
        if (await db.Categories.AnyAsync(x => x.Id != input.Id && x.Slug == input.Slug)) ModelState.AddModelError("Slug", "Slug đã tồn tại.");
        if (!ModelState.IsValid) return View(input);
        if (input.Id == 0) db.Categories.Add(input);
        else
        {
            var category = await db.Categories.FindAsync(input.Id); if (category == null) return NotFound();
            category.Name = input.Name.Trim(); category.Slug = input.Slug.Trim();
        }
        try { await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Không lưu được danh mục. Kiểm tra slug và thử lại."); return View(input); }
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await db.Categories.FindAsync(id); if (category == null) return NotFound();
        if (await db.Products.AnyAsync(x => x.CategoryId == id)) TempData["Error"] = "Danh mục còn sản phẩm. Hãy chuyển sản phẩm sang danh mục khác trước.";
        else
        {
            db.Categories.Remove(category);
            try { await db.SaveChangesAsync(); } catch (DbUpdateException) { TempData["Error"] = "Danh mục vừa được sử dụng, không thể xóa."; }
        }
        return RedirectToAction(nameof(Index));
    }
}
