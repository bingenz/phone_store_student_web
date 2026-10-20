using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
using SmartMobileStore.Models;
using SmartMobileStore.Services;
using SmartMobileStore.ViewModels;
namespace SmartMobileStore.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = "Admin")]
public class ProductsController(StoreDbContext db, ImageService images) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Products.AsNoTracking().Include(x => x.Category).OrderBy(x => x.Id).ToListAsync());
    private async Task Categories(int selected = 0) => ViewBag.Categories = new SelectList(await db.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync(), "Id", "Name", selected);
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        var input = new ProductInput();
        if (id.HasValue)
        {
            var p = await db.Products.AsNoTracking().Include(x => x.Specifications).SingleOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();
            input = new ProductInput { Id = p.Id, SKU = p.SKU, Slug = p.Slug, Name = p.Name, CategoryId = p.CategoryId, Price = p.Price, OldPrice = p.OldPrice, Stock = p.Stock, Description = p.Description, IsActive = p.IsActive, RowVersion = Convert.ToBase64String(p.RowVersion), Specifications = string.Join("\n", p.Specifications.OrderBy(x => x.Position).Select(x => x.Text)) };
        }
        await Categories(input.CategoryId); return View(input);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ProductInput input)
    {
        if (input.OldPrice < input.Price) ModelState.AddModelError("OldPrice", "Giá cũ không được nhỏ hơn giá bán.");
        if (!await db.Categories.AnyAsync(x => x.Id == input.CategoryId)) ModelState.AddModelError("CategoryId", "Danh mục không tồn tại.");
        if (await db.Products.AnyAsync(x => x.Id != input.Id && (x.SKU == input.SKU || x.Slug == input.Slug))) ModelState.AddModelError("", "SKU hoặc slug đã tồn tại.");
        var specs = (input.Specifications ?? "").Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (specs.Length > 30 || specs.Any(x => x.Length > 300)) ModelState.AddModelError("Specifications", "Tối đa 30 dòng, 300 ký tự mỗi dòng.");
        if (ModelState.IsValid)
        {
            var p = input.Id == 0 ? new Product() : await db.Products.Include(x => x.Specifications).SingleOrDefaultAsync(x => x.Id == input.Id);
            if (p == null) return NotFound();
            try
            {
                if (input.Id != 0)
                {
                    if (!Convert.TryFromBase64String(input.RowVersion ?? "", new byte[8], out var length) || length != 8) throw new StoreException("Phiên sửa không hợp lệ. Mở lại sản phẩm.");
                    db.Entry(p).Property(x => x.RowVersion).OriginalValue = Convert.FromBase64String(input.RowVersion!);
                }
                if (input.Image != null) p.ImagePath = await images.SaveAsync(input.Image);
                p.Name = input.Name.Trim(); p.SKU = input.SKU.Trim(); p.Slug = input.Slug.Trim(); p.CategoryId = input.CategoryId;
                p.Price = input.Price; p.OldPrice = input.OldPrice; p.Stock = input.Stock; p.Description = input.Description.Trim(); p.IsActive = input.IsActive;
                db.ProductSpecifications.RemoveRange(p.Specifications);
                p.Specifications = specs.Select((text, i) => new ProductSpecification { Text = text, Position = i }).ToList();
                if (input.Id == 0) db.Products.Add(p);
                await db.SaveChangesAsync(); TempData["Success"] = "Đã lưu sản phẩm."; return RedirectToAction(nameof(Index));
            }
            catch (StoreException ex) { ModelState.AddModelError("", ex.Message); }
            catch (DbUpdateConcurrencyException) { ModelState.AddModelError("", "Sản phẩm vừa được thay đổi hoặc đặt mua. Mở lại trang trước khi sửa để tránh ghi đè tồn kho."); }
            catch (DbUpdateException) { ModelState.AddModelError("", "Không lưu được. Kiểm tra SKU, slug và dữ liệu liên quan."); }
        }
        await Categories(input.CategoryId); return View(input);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id, string rowVersion)
    {
        var p = await db.Products.SingleOrDefaultAsync(x => x.Id == id); if (p == null) return NotFound();
        if (!Convert.TryFromBase64String(rowVersion, new byte[8], out var length) || length != 8) return BadRequest();
        db.Entry(p).Property(x => x.RowVersion).OriginalValue = Convert.FromBase64String(rowVersion);
        try
        {
            if (await db.OrderItems.AnyAsync(x => x.ProductId == id)) { p.IsActive = false; TempData["Success"] = "Sản phẩm có lịch sử đơn hàng: đã chuyển sang ngừng bán."; }
            else { db.Products.Remove(p); TempData["Success"] = "Đã xóa sản phẩm."; }
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException) { TempData.Remove("Success"); TempData["Error"] = "Dữ liệu vừa thay đổi. Tải lại danh sách trước khi xóa."; }
        return RedirectToAction(nameof(Index));
    }
}
