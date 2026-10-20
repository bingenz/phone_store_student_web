using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
using SmartMobileStore.Models;
using SmartMobileStore.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(o => o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddDbContext<StoreDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Store")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.User.RequireUniqueEmail = true;
    o.Password.RequiredLength = 10;
    o.Lockout.MaxFailedAccessAttempts = 5;
    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
}).AddEntityFrameworkStores<StoreDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/Account/Login"; o.AccessDeniedPath = "/Account/Denied";
    o.Cookie.HttpOnly = true; o.Cookie.SameSite = SameSiteMode.Lax;
    o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromHours(2); o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true; o.Cookie.SameSite = SameSiteMode.Lax;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ImageService>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o => o.MultipartBodyLengthLimit = 6 * 1024 * 1024);
var app = builder.Build();
// Không tự migrate/seed khi mở web. Chỉ chạy các lệnh này sau khi được duyệt SQL.
if (args.Contains("--seed-demo") || args.Contains("--create-admin"))
{
    using var scope = app.Services.CreateScope();
    if (args.Contains("--seed-demo")) await DemoSeeder.SeedAsync(scope.ServiceProvider);
    if (args.Contains("--create-admin")) await AdminBootstrap.RunAsync(scope.ServiceProvider);
    return;
}
app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Status", "?code={0}");
app.Use(async (context, next) =>
{
    context.Response.Headers.XContentTypeOptions = "nosniff";
    context.Response.Headers.XFrameOptions = "SAMEORIGIN";
    await next();
});
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("areas", "{area:exists}/{controller=Products}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
public partial class Program { }
