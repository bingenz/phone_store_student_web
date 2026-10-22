using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartMobileStore.Data;
using SmartMobileStore.Models;
using SmartMobileStore.ViewModels;
namespace SmartMobileStore.Services;

public class StoreException(string message) : Exception(message);

public class OrderService(StoreDbContext db)
{
    public static bool IsDeadlock(Exception exception)
    {
        // EF may wrap SQL 1205 in InvalidOperationException; do not hide unrelated errors.
        for (Exception? cause = exception; cause != null; cause = cause.InnerException)
            if (cause is SqlException sql && sql.Errors.Cast<SqlError>().Any(error => error.Number == 1205)) return true;
        return false;
    }

    public static bool CanTransition(OrderStatus from, OrderStatus to) => (from, to) switch
    {
        (OrderStatus.Pending, OrderStatus.Confirmed or OrderStatus.Cancelled) => true,
        (OrderStatus.Confirmed, OrderStatus.Shipping or OrderStatus.Cancelled) => true,
        (OrderStatus.Shipping, OrderStatus.Completed) => true,
        _ => false
    };

    public async Task<Order> PlaceAsync(string userId, CheckoutInput input, List<CartLine> lines)
    {
        if (input.SubmissionKey == Guid.Empty) throw new StoreException("Phiên đặt hàng không hợp lệ.");
        // Serializable cùng rowversion bảo vệ tồn kho; SQL deadlock/concurrency được báo để khách thử lại.
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var existing = await db.Orders.SingleOrDefaultAsync(x => x.SubmissionKey == input.SubmissionKey);
        if (existing != null)
        {
            if (existing.UserId != userId) throw new StoreException("Phiên đặt hàng không hợp lệ.");
            return existing;
        }
        if (lines.Count == 0 || lines.Count > 100 || lines.Select(x => x.ProductId).Distinct().Count() != lines.Count || lines.Any(x => x.Quantity is < 1 or > 999))
            throw new StoreException("Giỏ hàng trống hoặc số lượng không hợp lệ (1–999).");
        var ids = lines.Select(x => x.ProductId).ToArray();
        var products = await db.Products.Where(x => ids.Contains(x.Id)).OrderBy(x => x.Id).ToListAsync();
        var order = new Order { UserId = userId, Recipient = input.Recipient.Trim(), Phone = input.Phone.Trim(), Address = input.Address.Trim(), SubmissionKey = input.SubmissionKey };
        foreach (var line in lines.OrderBy(x => x.ProductId))
        {
            var product = products.SingleOrDefault(x => x.Id == line.ProductId);
            if (product == null || !product.IsActive || product.Stock < line.Quantity)
                throw new StoreException("Sản phẩm ngừng bán hoặc không còn đủ hàng. Hãy cập nhật giỏ hàng.");
            product.Stock -= line.Quantity;
            order.Items.Add(new OrderItem { ProductId = product.Id, ProductName = product.Name, SKU = product.SKU, UnitPrice = product.Price, Quantity = line.Quantity });
        }
        order.Total = order.Items.Sum(x => x.Quantity * x.UnitPrice);
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
        return order;
    }

    public async Task TransitionAsync(int id, OrderStatus status)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var order = await db.Orders.Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == id) ?? throw new StoreException("Không tìm thấy đơn.");
        if (!CanTransition(order.Status, status)) throw new StoreException("Không thể chuyển trạng thái này hoặc đơn đã được xử lý.");
        if (status == OrderStatus.Cancelled)
        {
            var ids = order.Items.Select(x => x.ProductId).ToArray();
            var products = await db.Products.Where(x => ids.Contains(x.Id)).OrderBy(x => x.Id).ToListAsync();
            foreach (var item in order.Items) products.Single(x => x.Id == item.ProductId).Stock += item.Quantity;
        }
        order.Status = status;
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
