using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
namespace SmartMobileStore.Controllers;

public class HomeController(StoreDbContext db, ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Products.AsNoTracking().Include(x => x.Category).Where(x => x.IsActive).OrderBy(x => x.Id).ToListAsync());
    public IActionResult About() => View();
    public IActionResult News() => View();
    [AllowAnonymous, IgnoreAntiforgeryToken, ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Status(int code) { Response.StatusCode = code; return View("Status", code); }
    [AllowAnonymous, IgnoreAntiforgeryToken, ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Error()
    {
        var error = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null) logger.LogError("Request failed: {ErrorType}; Trace {TraceId}", error.Error.GetType().Name, HttpContext.TraceIdentifier);
        Response.StatusCode = 500; return View();
    }
}
