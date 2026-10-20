using System.ComponentModel.DataAnnotations;
using SmartMobileStore.Models;
namespace SmartMobileStore.ViewModels;

public class LoginInput
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    public string? ReturnUrl { get; set; }
}
public class RegisterInput : LoginInput
{
    [Required, Compare(nameof(Password)), DataType(DataType.Password)] public string ConfirmPassword { get; set; } = "";
}
public class CheckoutInput
{
    [Required, StringLength(100)] public string Recipient { get; set; } = "";
    [Required, RegularExpression(@"^[0-9+ ()-]{8,20}$")] public string Phone { get; set; } = "";
    [Required, StringLength(500)] public string Address { get; set; } = "";
    public Guid SubmissionKey { get; set; }
}
public record CartLine(int ProductId, int Quantity);
public record CartDisplay(Product Product, int Quantity);
public record CatalogView(List<Product> Products, List<Category> Categories, string? Query, string? Category, string Sort, int Page, int Pages);
public class ProductInput
{
    public int Id { get; set; }
    [Required, StringLength(80)] public string SKU { get; set; } = "";
    [Required, StringLength(100), RegularExpression("^[a-z0-9_-]+$")] public string Slug { get; set; } = "";
    [Required, StringLength(200)] public string Name { get; set; } = "";
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
    [Range(1, 9999999999d)] public decimal Price { get; set; }
    [Range(1, 9999999999d)] public decimal? OldPrice { get; set; }
    [Range(0, 1000000)] public int Stock { get; set; }
    [Required, StringLength(4000)] public string Description { get; set; } = "";
    [StringLength(5000)] public string? Specifications { get; set; }
    public bool IsActive { get; set; } = true;
    public IFormFile? Image { get; set; }
    public string? RowVersion { get; set; }
}
