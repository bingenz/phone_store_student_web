using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Models;
namespace SmartMobileStore.Data;

public static class DemoSeeder
{
    private record SeedProduct(string SKU, string Slug, string Name, decimal Price, decimal? OldPrice, int Stock, string Description, string ImagePath, string CategorySlug, string[] Specifications);
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<StoreDbContext>();
        // Schema phải được áp dụng riêng; seed không migrate hoặc ghi đè dữ liệu có sẵn.
        if ((await db.Database.GetPendingMigrationsAsync()).Any()) throw new InvalidOperationException("Còn migration chưa áp dụng. Dừng seed.");
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var name in new[] { "User", "Admin" })
            if (!await roles.RoleExistsAsync(name))
            {
                var result = await roles.CreateAsync(new IdentityRole(name));
                if (!result.Succeeded) throw new InvalidOperationException("Không tạo được vai trò.");
            }
        await using var tx = await db.Database.BeginTransactionAsync();
        foreach (var item in new[] { ("dien-thoai", "Điện thoại"), ("dong-ho", "Đồng hồ"), ("phu-kien", "Phụ kiện") })
            if (!await db.Categories.AnyAsync(x => x.Slug == item.Item1)) db.Categories.Add(new Category { Slug = item.Item1, Name = item.Item2 });
        await db.SaveChangesAsync();
        var env = services.GetRequiredService<IWebHostEnvironment>();
        var json = await File.ReadAllTextAsync(Path.Combine(env.ContentRootPath, "Data", "demo-products.json"));
        var seeds = JsonSerializer.Deserialize<List<SeedProduct>>(json)!;
        foreach (var p in seeds)
        {
            if (await db.Products.AnyAsync(x => x.Slug == p.Slug || x.SKU == p.SKU)) continue;
            db.Products.Add(new Product { SKU = p.SKU, Slug = p.Slug, Name = p.Name, Price = p.Price, OldPrice = p.OldPrice, Stock = p.Stock, Description = p.Description, ImagePath = p.ImagePath, CategoryId = (await db.Categories.SingleAsync(x => x.Slug == p.CategorySlug)).Id, Specifications = p.Specifications.Select((text, i) => new ProductSpecification { Text = text, Position = i }).ToList() });
        }
        await db.SaveChangesAsync(); await tx.CommitAsync();
        Console.WriteLine("Seed hoàn tất; không ghi đè sản phẩm hoặc tồn kho cũ.");
    }
}
