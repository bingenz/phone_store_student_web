using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
using SmartMobileStore.ViewModels;
namespace SmartMobileStore.Controllers;

public class ProductsController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? q, string? category, string sort = "name", int page = 1)
    {
        q = q?.Trim(); if (q?.Length > 100) q = q[..100];
        var query = db.Products.AsNoTracking().Include(x => x.Category).Where(x => x.IsActive);
        if (!string.IsNullOrEmpty(q)) query = query.Where(x => x.Name.Contains(q) || x.SKU.Contains(q));
        if (!string.IsNullOrEmpty(category)) query = query.Where(x => x.Category!.Slug == category);
        sort = sort is "price-asc" or "price-desc" ? sort : "name";
        query = sort switch { "price-asc" => query.OrderBy(x => x.Price).ThenBy(x => x.Id), "price-desc" => query.OrderByDescending(x => x.Price).ThenBy(x => x.Id), _ => query.OrderBy(x => x.Name).ThenBy(x => x.Id) };
        var pages = Math.Max(1, (int)Math.Ceiling(await query.CountAsync() / 8d)); page = Math.Clamp(page, 1, pages);
        return View(new CatalogView(await query.Skip((page - 1) * 8).Take(8).ToListAsync(), await db.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync(), q, category, sort, page, pages));
    }
    public IActionResult Accessories() => RedirectToAction(nameof(Index), new { category = "phu-kien" });
    public async Task<IActionResult> Details(string id)
    {
        var product = await db.Products.AsNoTracking().Include(x => x.Specifications).SingleOrDefaultAsync(x => x.Slug == id && x.IsActive);
        return product == null ? NotFound() : View(product);
    }
}
