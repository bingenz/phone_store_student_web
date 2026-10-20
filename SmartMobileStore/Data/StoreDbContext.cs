using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Models;

namespace SmartMobileStore.Data;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductSpecification> ProductSpecifications => Set<ProductSpecification>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<Category>().HasIndex(x => x.Slug).IsUnique();
        b.Entity<Product>().HasIndex(x => x.SKU).IsUnique();
        b.Entity<Product>().HasIndex(x => x.Slug).IsUnique();
        b.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);
        b.Entity<Product>().Property(x => x.OldPrice).HasPrecision(18, 2);
        b.Entity<Product>().HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Product>().HasMany(x => x.Specifications).WithOne().HasForeignKey(x => x.ProductId);
        b.Entity<Product>().ToTable(t => { t.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0"); t.HasCheckConstraint("CK_Product_Price", "[Price] > 0 AND ([OldPrice] IS NULL OR [OldPrice] >= [Price])"); });
        b.Entity<Order>().HasIndex(x => x.SubmissionKey).IsUnique();
        b.Entity<Order>().HasIndex(x => new { x.UserId, x.CreatedAt });
        b.Entity<Order>().Property(x => x.Total).HasPrecision(18, 2);
        b.Entity<Order>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        b.Entity<OrderItem>().HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<OrderItem>().ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0 AND [UnitPrice] > 0"));
    }
}
