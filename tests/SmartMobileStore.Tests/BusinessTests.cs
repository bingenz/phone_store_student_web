using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration;
using SmartMobileStore.Models;
using SmartMobileStore.Services;
using SmartMobileStore.ViewModels;

namespace SmartMobileStore.Tests;

public class BusinessTests
{
    [Fact]
    public void UnrelatedFailuresAreNotTreatedAsSqlDeadlocks()
        => Assert.False(OrderService.IsDeadlock(new InvalidOperationException("Unexpected application error", new Exception("Not SQL"))));

    [Fact]
    public void OrderStatusOnlyAllowsApprovedTransitions()
    {
        var allowed = new[] { (OrderStatus.Pending, OrderStatus.Confirmed), (OrderStatus.Pending, OrderStatus.Cancelled), (OrderStatus.Confirmed, OrderStatus.Shipping), (OrderStatus.Confirmed, OrderStatus.Cancelled), (OrderStatus.Shipping, OrderStatus.Completed) };
        foreach (var from in Enum.GetValues<OrderStatus>())
            foreach (var to in Enum.GetValues<OrderStatus>()) Assert.Equal(allowed.Contains((from, to)), OrderService.CanTransition(from, to));
    }
    [Theory]
    [InlineData("", "123", "")]
    [InlineData("Khách", "abc12345", "Địa chỉ")]
    public void CheckoutRejectsInvalidRecipient(string name, string phone, string address)
    {
        var input = new CheckoutInput { Recipient = name, Phone = phone, Address = address };
        Assert.False(Validator.TryValidateObject(input, new ValidationContext(input), [], true));
    }
    [Fact]
    public void CartUpdatesRemovesAndRotatesCheckoutKey()
    {
        var context = new DefaultHttpContext { Session = new MemorySession() };
        var cart = new CartService(new HttpContextAccessor { HttpContext = context });
        cart.Set(1, 2); cart.Set(1, 3); cart.Set(2, 1);
        Assert.Equal(4, cart.Get().Sum(x => x.Quantity)); Assert.Equal(2, cart.Get().Count);
        cart.Set(2, 0); Assert.Single(cart.Get());
        var key = cart.CheckoutKey(); Assert.Equal(key, cart.CheckoutKey()); Assert.True(cart.ValidKey(key));
        cart.Finish(); Assert.Empty(cart.Get()); Assert.False(cart.ValidKey(key)); Assert.NotEqual(key, cart.CheckoutKey());
    }
    [Theory]
    [InlineData("attack.svg", "<svg></svg>")]
    [InlineData("attack.png", "not an image")]
    [InlineData("attack.jpg", "<script>alert(1)</script>")]
    public async Task UploadRejectsSpoofedOrUnsupportedImages(string name, string body)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(body));
        var file = new FormFile(stream, 0, stream.Length, "Image", name);
        await Assert.ThrowsAsync<StoreException>(() => new ImageService(new TestEnvironment()).SaveAsync(file));
    }
    [Fact]
    public async Task ValidImageIsReencodedWithServerGeneratedName()
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(6, 1)) return;
        var folder = Path.Combine(Path.GetTempPath(), "SmartMobileImageTests", Guid.NewGuid().ToString("N"));
        var environment = new TestEnvironment { WebRootPath = folder };
        using var stream = new MemoryStream();
        using (var bitmap = new System.Drawing.Bitmap(2, 2)) bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Position = 0;
        var file = new FormFile(stream, 0, stream.Length, "Image", "../../untrusted.png");
        var result = await new ImageService(environment).SaveAsync(file);
        Assert.Matches("^/uploads/[a-f0-9]{32}\\.png$", result);
        Assert.True(File.Exists(Path.Combine(folder, result.TrimStart('/').Replace('/', Path.DirectorySeparatorChar))));
        // Chỉ xóa file test vừa tạo trong thư mục tạm do test sở hữu.
        File.Delete(Path.Combine(folder, result.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)));
    }
}

public class PublicHttpTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;
    public PublicHttpTests(WebApplicationFactory<Program> factory) => client = factory.CreateClient(new() { AllowAutoRedirect = false });
    [Theory]
    [InlineData("/Home/About")]
    [InlineData("/Home/News")]
    [InlineData("/Contact")]
    [InlineData("/Account/Login")]
    [InlineData("/Account/Register")]
    public async Task PublicPagesRenderWithoutSql(string url)
    {
        var response = await client.GetAsync(url); Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("SmartMobile", await response.Content.ReadAsStringAsync());
    }
    [Theory]
    [InlineData("/Admin/Products")]
    [InlineData("/Admin/Categories")]
    [InlineData("/Admin/Orders")]
    [InlineData("/Admin/Contact")]
    [InlineData("/Orders")]
    public async Task AnonymousCannotAccessProtectedPages(string url)
    {
        var response = await client.GetAsync(url); Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("http://localhost/Account/Login", response.Headers.Location!.ToString());
    }
    [Fact]
    public async Task PostWithoutCsrfIsRejected()
    {
        var response = await client.PostAsync("/Cart/Add", new FormUrlEncodedContent(new Dictionary<string, string> { ["id"] = "1" }));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task UnknownPageReturns404()
        => Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/does-not-exist")).StatusCode);
    [Fact]
    public async Task InvalidDatabaseConfigurationShowsSafeErrorPage()
    {
        await using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(b => b.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:Store"] = "invalid connection string" })));
        using var failedClient = factory.CreateClient();
        var response = await failedClient.GetAsync("/"); Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync(); Assert.Contains("SmartMobile", html); Assert.DoesNotContain("invalid connection string", html); Assert.DoesNotContain("StackTrace", html);
    }
}

internal class MemorySession : ISession
{
    private readonly Dictionary<string, byte[]> values = [];
    public bool IsAvailable => true; public string Id => "test"; public IEnumerable<string> Keys => values.Keys;
    public void Clear() => values.Clear(); public Task CommitAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken token = default) => Task.CompletedTask;
    public void Remove(string key) => values.Remove(key); public void Set(string key, byte[] value) => values[key] = value;
    public bool TryGetValue(string key, out byte[] value) => values.TryGetValue(key, out value!);
}
internal class TestEnvironment : IWebHostEnvironment
{
    public string ApplicationName { get; set; } = "SmartMobileStore";
    public string EnvironmentName { get; set; } = "Testing";
    public string ContentRootPath { get; set; } = "";
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    public string WebRootPath { get; set; } = Path.GetTempPath();
    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
}
