using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
using SmartMobileStore.Services;
using SmartMobileStore.ViewModels;
namespace SmartMobileStore.Controllers;

[Authorize]
public class OrdersController(StoreDbContext db, CartService cart, OrderService orders) : Controller
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public async Task<IActionResult> Index() => View(await db.Orders.AsNoTracking().Where(x => x.UserId == UserId).OrderByDescending(x => x.CreatedAt).ToListAsync());
    public async Task<IActionResult> Details(int id)
    {
        var order = await db.Orders.AsNoTracking().Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
        return order == null ? NotFound() : View(order);
    }
    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        if (cart.Get().Count == 0) return RedirectToAction("Index", "Cart");
        ViewBag.Total = await TotalAsync(); return View(new CheckoutInput { SubmissionKey = cart.CheckoutKey() });
    }
    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutInput input)
    {
        var existing = await db.Orders.AsNoTracking().SingleOrDefaultAsync(x => x.SubmissionKey == input.SubmissionKey && x.UserId == UserId);
        if (existing != null) return RedirectToAction(nameof(Details), new { id = existing.Id });
        if (!cart.ValidKey(input.SubmissionKey)) ModelState.AddModelError("", "Phiên đặt hàng đã hết hạn. Mở lại trang thanh toán.");
        if (ModelState.IsValid)
        {
            try
            {
                var order = await orders.PlaceAsync(UserId, input, cart.Get()); cart.Finish();
                return RedirectToAction(nameof(Details), new { id = order.Id });
            }
            catch (StoreException ex) { ModelState.AddModelError("", ex.Message); }
            catch (Exception ex) when (ex is DbUpdateException or SqlException || OrderService.IsDeadlock(ex)) { ModelState.AddModelError("", "Dữ liệu vừa thay đổi hoặc kết nối bị gián đoạn. Kiểm tra Đơn hàng của tôi trước khi thử lại."); }
        }
        ViewBag.Total = await TotalAsync(); return View(input);
    }
    private async Task<decimal> TotalAsync()
    {
        var lines = cart.Get(); var ids = lines.Select(x => x.ProductId).ToArray();
        var products = await db.Products.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
        return products.Sum(p => p.Price * lines.Single(x => x.ProductId == p.Id).Quantity);
    }
}
