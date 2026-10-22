using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using SmartMobileStore.Data;
using SmartMobileStore.Models;
using SmartMobileStore.Services;
namespace SmartMobileStore.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = "Admin")]
public class OrdersController(StoreDbContext db, OrderService orders) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Orders.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync());
    public async Task<IActionResult> Details(int id)
    {
        var order = await db.Orders.AsNoTracking().Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == id);
        return order == null ? NotFound() : View(order);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int id, OrderStatus status)
    {
        try { await orders.TransitionAsync(id, status); TempData["Success"] = "Đã cập nhật trạng thái."; }
        catch (StoreException ex) { TempData["Error"] = ex.Message; }
        catch (Exception ex) when (ex is DbUpdateException or SqlException || OrderService.IsDeadlock(ex)) { TempData["Error"] = "Đơn vừa thay đổi hoặc kết nối gián đoạn. Tải lại để kiểm tra trước khi thử lại."; }
        return RedirectToAction(nameof(Details), new { id });
    }
}
