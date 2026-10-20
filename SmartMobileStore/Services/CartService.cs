using System.Text.Json;
using SmartMobileStore.ViewModels;
namespace SmartMobileStore.Services;

public class CartService(IHttpContextAccessor accessor)
{
    private ISession Session => accessor.HttpContext!.Session;
    public List<CartLine> Get() => JsonSerializer.Deserialize<List<CartLine>>(Session.GetString("cart") ?? "[]") ?? [];
    public void Set(int productId, int quantity)
    {
        var lines = Get();
        lines.RemoveAll(x => x.ProductId == productId);
        if (quantity > 0) lines.Add(new(productId, quantity));
        Session.SetString("cart", JsonSerializer.Serialize(lines));
    }
    public void Clear() => Session.Remove("cart");
    public Guid CheckoutKey()
    {
        if (Guid.TryParse(Session.GetString("checkout-key"), out var key)) return key;
        key = Guid.NewGuid();
        Session.SetString("checkout-key", key.ToString());
        return key;
    }
    public bool ValidKey(Guid key) => key != Guid.Empty && Session.GetString("checkout-key") == key.ToString();
    public void Finish() { Clear(); Session.Remove("checkout-key"); }
}
