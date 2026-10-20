using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SmartMobileStore.Models;

public class ApplicationUser : IdentityUser { }

public class Category
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [Required, StringLength(100), RegularExpression("^[a-z0-9-]+$")] public string Slug { get; set; } = "";
}

public class Product
{
    public int Id { get; set; }
    [Required, StringLength(80)] public string SKU { get; set; } = "";
    [Required, StringLength(100), RegularExpression("^[a-z0-9_-]+$")] public string Slug { get; set; } = "";
    [Required, StringLength(200)] public string Name { get; set; } = "";
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    [Range(1, 9999999999d)] public decimal Price { get; set; }
    [Range(1, 9999999999d)] public decimal? OldPrice { get; set; }
    [Range(0, 1000000)] public int Stock { get; set; }
    [Required, StringLength(4000)] public string Description { get; set; } = "";
    public string ImagePath { get; set; } = "/images/logo-phone.svg";
    public bool IsActive { get; set; } = true;
    [Timestamp] public byte[] RowVersion { get; set; } = [];
    public List<ProductSpecification> Specifications { get; set; } = [];
}

public class ProductSpecification
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    [Required, StringLength(300)] public string Text { get; set; } = "";
    public int Position { get; set; }
}

public enum OrderStatus { Pending, Confirmed, Shipping, Completed, Cancelled }

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    [Required, StringLength(100)] public string Recipient { get; set; } = "";
    [Required, StringLength(20)] public string Phone { get; set; } = "";
    [Required, StringLength(500)] public string Address { get; set; } = "";
    public decimal Total { get; set; }
    public Guid SubmissionKey { get; set; }
    [Timestamp] public byte[] RowVersion { get; set; } = [];
    public List<OrderItem> Items { get; set; } = [];
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string SKU { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class ContactMessage
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [Required, EmailAddress, StringLength(200)] public string Email { get; set; } = "";
    [StringLength(20), Phone] public string? Phone { get; set; }
    [Required, StringLength(4000)] public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsHandled { get; set; }
}

public static class DisplayFormat
{
    public static string Money(decimal amount) => amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + "đ";
    public static string Status(OrderStatus status) => status switch
    {
        OrderStatus.Pending => "Chờ xác nhận",
        OrderStatus.Confirmed => "Đã xác nhận",
        OrderStatus.Shipping => "Đang giao",
        OrderStatus.Completed => "Hoàn tất",
        _ => "Đã hủy"
    };
}
