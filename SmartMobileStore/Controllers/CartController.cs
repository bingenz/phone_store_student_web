using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
using SmartMobileStore.Services;
using SmartMobileStore.ViewModels;
namespace SmartMobileStore.Controllers;

public class CartController(StoreDbContext db, CartService cart) : Controller
{
    public async Task<IActionResult> Index()
    {
        var lines = cart.Get(); var ids = lines.Select(x => x.ProductId).ToArray();
        var products = await db.Products.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
        return View(products.Select(x => new CartDisplay(x, lines.Single(l => l.ProductId == x.Id).Quantity)).ToList());
    }
    [HttpPost]
    public async Task<IActionResult> Add(int id)
    {
        var product = await db.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.IsActive);
        var quantity = (cart.Get().SingleOrDefault(x => x.ProductId == id)?.Quantity ?? 0) + 1;
        if (product == null || quantity > product.Stock || quantity > 999) TempData["Error"] = "Sản phẩm không đủ tồn kho.";
        else { cart.Set(id, quantity); TempData["Success"] = "Đã thêm sản phẩm vào giỏ."; }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Update(int id, int quantity)
    {
        if (quantity == 0) cart.Set(id, 0);
        else
        {
            var product = await db.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.IsActive);
            if (quantity is < 1 or > 999 || product == null || product.Stock < quantity) TempData["Error"] = "Số lượng phải từ 1–999 và không vượt tồn kho.";
            else cart.Set(id, quantity);
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost] public IActionResult Clear() { cart.Clear(); return RedirectToAction(nameof(Index)); }
}
