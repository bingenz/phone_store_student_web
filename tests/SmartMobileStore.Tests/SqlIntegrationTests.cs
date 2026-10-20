using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMobileStore.Data;
using SmartMobileStore.Models;
using SmartMobileStore.Services;
using SmartMobileStore.ViewModels;

namespace SmartMobileStore.Tests;

public class SqlFactAttribute : FactAttribute
{
    public SqlFactAttribute() { if (Environment.GetEnvironmentVariable("SMARTMOBILE_SQL_APPROVED") != "1") Skip = "Cần duyệt database test riêng; xem README."; }
}
[CollectionDefinition("SQL", DisableParallelization = true)]
public class SqlCollection : ICollectionFixture<SqlFixture> { }
public class SqlFixture : IAsyncLifetime
{
    public const string DatabaseName = "SmartMobileStore_Test_20261020";
    public string Connection => new SqlConnectionStringBuilder {
        DataSource = Environment.GetEnvironmentVariable("SMARTMOBILE_TEST_SERVER") ?? @"(localdb)\MSSQLLocalDB",
        InitialCatalog = DatabaseName, IntegratedSecurity = true, TrustServerCertificate = true, ConnectTimeout = 5
    }.ConnectionString;
    public StoreDbContext NewDb() => new(new DbContextOptionsBuilder<StoreDbContext>().UseSqlServer(Connection).Options);
    public async Task InitializeAsync()
    {
        if (Environment.GetEnvironmentVariable("SMARTMOBILE_SQL_APPROVED") != "1") return;
        var masterConnection = new SqlConnectionStringBuilder(Connection) { InitialCatalog = "master" };
        await using var master = new SqlConnection(masterConnection.ConnectionString);
        await master.OpenAsync();
        await using var command = new SqlCommand("SELECT DB_ID(@name)", master); command.Parameters.AddWithValue("@name", DatabaseName);
        var id = await command.ExecuteScalarAsync();
        if (id != DBNull.Value && id != null && Environment.GetEnvironmentVariable("SMARTMOBILE_REUSE_APPROVED_TEST") != "1")
            throw new InvalidOperationException("Database test đã tồn tại. Không tự ghi đè; cần duyệt dùng lại database test.");
        await using var db = NewDb(); await db.Database.MigrateAsync();
    }
    public Task DisposeAsync() => Task.CompletedTask; // Giữ database lại; tuyệt đối không tự DROP.
    public async Task<(string UserId, int ProductId)> ArrangeAsync(int stock = 5)
    {
        await using var db = NewDb(); var key = Guid.NewGuid().ToString("N");
        var user = new ApplicationUser { Id = key, UserName = key + "@example.test", NormalizedUserName = key.ToUpperInvariant() + "@EXAMPLE.TEST", Email = key + "@example.test", SecurityStamp = Guid.NewGuid().ToString() };
        db.Users.Add(user);
        var category = new Category { Name = "Test", Slug = key }; db.Categories.Add(category); await db.SaveChangesAsync();
        var product = new Product { SKU = key, Slug = key, Name = "Test product", CategoryId = category.Id, Price = 100000, Stock = stock, Description = "Integration test" };
        db.Products.Add(product); await db.SaveChangesAsync(); return (user.Id, product.Id);
    }
    public WebApplicationFactory<Program> Factory() => new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:Store"] = Connection })));
    public static CheckoutInput Input(Guid? key = null) => new() { Recipient = "Khách thử nghiệm", Phone = "0901234567", Address = "Địa chỉ demo", SubmissionKey = key ?? Guid.NewGuid() };
}

[Collection("SQL")]
public class SqlIntegrationTests(SqlFixture fixture)
{
    [SqlFact]
    public async Task CheckoutPersistsServerPricesAndDeductsStock()
    {
        var setup = await fixture.ArrangeAsync();
        await using var db = fixture.NewDb();
        var order = await new OrderService(db).PlaceAsync(setup.UserId, SqlFixture.Input(), [new(setup.ProductId, 2)]);
        Assert.Equal(200000, order.Total);
        await using var verify = fixture.NewDb();
        Assert.Equal(3, (await verify.Products.FindAsync(setup.ProductId))!.Stock);
        Assert.Single(await verify.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync());
    }
    [SqlFact]
    public async Task DuplicateSubmitReturnsSameOrderAndDoesNotDeductTwice()
    {
        var setup = await fixture.ArrangeAsync(); var input = SqlFixture.Input();
        await using var db = fixture.NewDb();
        var first = await new OrderService(db).PlaceAsync(setup.UserId, input, [new(setup.ProductId, 1)]);
        await using var db2 = fixture.NewDb();
        var repeat = await new OrderService(db2).PlaceAsync(setup.UserId, input, [new(setup.ProductId, 1)]);
        Assert.Equal(first.Id, repeat.Id);
        Assert.Equal(4, (await db2.Products.FindAsync(setup.ProductId))!.Stock);
    }
    [SqlFact]
    public async Task InsufficientStockRollsBackWholeOrder()
    {
        var setup = await fixture.ArrangeAsync(1);
        await using var db = fixture.NewDb(); var input = SqlFixture.Input();
        await Assert.ThrowsAsync<StoreException>(() => new OrderService(db).PlaceAsync(setup.UserId, input, [new(setup.ProductId, 2)]));
        await using var verify = fixture.NewDb();
        Assert.Equal(1, (await verify.Products.FindAsync(setup.ProductId))!.Stock);
        Assert.False(await verify.Orders.AnyAsync(x => x.SubmissionKey == input.SubmissionKey));
    }
    [SqlFact]
    public async Task CancellationRestocksOnceOnly()
    {
        var setup = await fixture.ArrangeAsync();
        await using var db = fixture.NewDb();
        var order = await new OrderService(db).PlaceAsync(setup.UserId, SqlFixture.Input(), [new(setup.ProductId, 2)]);
        await using var db2 = fixture.NewDb(); await new OrderService(db2).TransitionAsync(order.Id, OrderStatus.Cancelled);
        await using var db3 = fixture.NewDb();
        await Assert.ThrowsAsync<StoreException>(() => new OrderService(db3).TransitionAsync(order.Id, OrderStatus.Cancelled));
        await using var verify = fixture.NewDb(); Assert.Equal(5, (await verify.Products.FindAsync(setup.ProductId))!.Stock);
    }
    [SqlFact]
    public async Task ConcurrentBuyersCannotOversell()
    {
        var setup = await fixture.ArrangeAsync(1);
        async Task<bool> Buy()
        {
            await using var db = fixture.NewDb();
            try { await new OrderService(db).PlaceAsync(setup.UserId, SqlFixture.Input(), [new(setup.ProductId, 1)]); return true; }
            catch (Exception ex) when (ex is StoreException or SqlException or DbUpdateException) { return false; }
        }
        var results = await Task.WhenAll(Buy(), Buy()); Assert.Single(results, x => x);
        await using var verify = fixture.NewDb(); Assert.Equal(0, (await verify.Products.FindAsync(setup.ProductId))!.Stock);
    }
    [SqlFact]
    public async Task ConcurrentAdminEditCannotOverwriteCheckoutStock()
    {
        var setup = await fixture.ArrangeAsync();
        await using var adminDb = fixture.NewDb(); var old = (await adminDb.Products.FindAsync(setup.ProductId))!;
        await using var buyDb = fixture.NewDb(); await new OrderService(buyDb).PlaceAsync(setup.UserId, SqlFixture.Input(), [new(setup.ProductId, 1)]);
        old.Stock = 50;
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => adminDb.SaveChangesAsync());
    }
    [SqlFact]
    public async Task SqlConstraintsRejectNegativeStockAndDeletingUsedCategory()
    {
        var setup = await fixture.ArrangeAsync();
        await using var db = fixture.NewDb(); var p = (await db.Products.FindAsync(setup.ProductId))!;
        p.Stock = -1; await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        await using var db2 = fixture.NewDb(); var category = await db2.Categories.FindAsync(p.CategoryId); db2.Categories.Remove(category!);
        await Assert.ThrowsAsync<DbUpdateException>(() => db2.SaveChangesAsync());
    }
    [SqlFact]
    public async Task CatalogAndInvalidDetailUseRealSql()
    {
        await using var factory = fixture.Factory(); using var client = factory.CreateClient(new() { AllowAutoRedirect = false });
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Products?q=Test&sort=price-asc&page=999")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/Products/Details/no-such-product")).StatusCode);
    }
    [SqlFact]
    public async Task UserCannotReadAnotherUsersOrderOrEnterAdmin()
    {
        var setup = await fixture.ArrangeAsync();
        await using var db = fixture.NewDb(); var order = await new OrderService(db).PlaceAsync(setup.UserId, SqlFixture.Input(), [new(setup.ProductId, 1)]);
        await using var factory = fixture.Factory();
        using (var scope = factory.Services.CreateScope())
        {
            var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await roles.RoleExistsAsync("User")) Assert.True((await roles.CreateAsync(new IdentityRole("User"))).Succeeded);
        }
        using var client = factory.CreateClient(new() { AllowAutoRedirect = false });
        var register = await client.GetStringAsync("/Account/Register");
        var token = Regex.Match(register, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value;
        Assert.NotEmpty(token);
        var password = "Demo!" + Guid.NewGuid().ToString("N");
        var response = await client.PostAsync("/Account/Register", new FormUrlEncodedContent(new Dictionary<string, string> { ["Email"] = Guid.NewGuid().ToString("N") + "@example.test", ["Password"] = password, ["ConfirmPassword"] = password, ["__RequestVerificationToken"] = WebUtility.HtmlDecode(token) }));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/Orders/Details/" + order.Id)).StatusCode);
        var admin = await client.GetAsync("/Admin/Products"); Assert.Equal(HttpStatusCode.Redirect, admin.StatusCode);
        Assert.Contains("/Account/Denied", admin.Headers.Location!.ToString());
    }

    private static string Hidden(string html, string name)
        => WebUtility.HtmlDecode(Regex.Match(html, "name=\"" + name + "\"[^>]*value=\"([^\"]+)\"").Groups[1].Value);
    private static async Task<HttpResponseMessage> Post(HttpClient client, string url, string page, Dictionary<string, string> fields)
    {
        fields["__RequestVerificationToken"] = Hidden(await client.GetStringAsync(page), "__RequestVerificationToken");
        return await client.PostAsync(url, new FormUrlEncodedContent(fields));
    }
    private static async Task<HttpClient> SignedClient(WebApplicationFactory<Program> factory, bool admin)
    {
        var password = "Demo!" + Guid.NewGuid().ToString("N"); var email = Guid.NewGuid().ToString("N") + "@example.test";
        using (var scope = factory.Services.CreateScope())
        {
            var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var role = admin ? "Admin" : "User";
            if (!await roles.RoleExistsAsync(role)) Assert.True((await roles.CreateAsync(new IdentityRole(role))).Succeeded);
            var user = new ApplicationUser { UserName = email, Email = email };
            Assert.True((await users.CreateAsync(user, password)).Succeeded);
            Assert.True((await users.AddToRoleAsync(user, role)).Succeeded);
        }
        var client = factory.CreateClient(new() { AllowAutoRedirect = false });
        var response = await Post(client, "/Account/Login", "/Account/Login", new() { ["Email"] = email, ["Password"] = password, ["ReturnUrl"] = "https://evil.example.test" });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode); Assert.Equal("/", response.Headers.Location!.ToString());
        return client;
    }
    [SqlFact]
    public async Task FullHttpCheckoutIgnoresClientPriceAndRepeatedSubmit()
    {
        var setup = await fixture.ArrangeAsync();
        await using var factory = fixture.Factory(); using var client = await SignedClient(factory, false);
        await using var lookup = fixture.NewDb();
        var product = (await lookup.Products.FindAsync(setup.ProductId))!;
        Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/Cart/Add", "/Products/Details/" + product.Slug, new() { ["id"] = setup.ProductId.ToString(), ["price"] = "1" })).StatusCode);
        var page = await client.GetStringAsync("/Orders/Checkout"); var key = Hidden(page, "SubmissionKey"); Assert.NotEmpty(key);
        var fields = new Dictionary<string, string> { ["Recipient"] = "Khách demo", ["Phone"] = "0901234567", ["Address"] = "Địa chỉ test", ["SubmissionKey"] = key, ["Total"] = "1", ["__RequestVerificationToken"] = Hidden(page, "__RequestVerificationToken") };
        var response = await client.PostAsync("/Orders/Checkout", new FormUrlEncodedContent(fields)); Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var repeat = await client.PostAsync("/Orders/Checkout", new FormUrlEncodedContent(fields)); Assert.Equal(response.Headers.Location, repeat.Headers.Location);
        await using var verify = fixture.NewDb(); var order = await verify.Orders.SingleAsync(x => x.SubmissionKey == Guid.Parse(key));
        Assert.Equal(100000, order.Total); Assert.Equal(4, (await verify.Products.FindAsync(setup.ProductId))!.Stock);
        Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/Account/Logout", "/Orders", new())).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Orders")).StatusCode);
    }
    [SqlFact]
    public async Task AdminCrudAndContactWorkflowWorksOverHttp()
    {
        await using var factory = fixture.Factory(); using var client = await SignedClient(factory, true);
        var slug = "test-" + Guid.NewGuid().ToString("N");
        Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/Admin/Categories/Edit", "/Admin/Categories/Edit", new() { ["Name"] = "Test category", ["Slug"] = slug })).StatusCode);
        await using var verify = fixture.NewDb(); var category = await verify.Categories.SingleAsync(x => x.Slug == slug);
        var fields = new Dictionary<string, string> { ["Name"] = "Admin test product", ["SKU"] = slug, ["Slug"] = slug, ["CategoryId"] = category.Id.ToString(), ["Price"] = "200000", ["Stock"] = "2", ["Description"] = "Mô tả test", ["IsActive"] = "true", ["Specifications"] = "Thông số 1\nThông số 2" };
        var saved = await Post(client, "/Admin/Products/Edit", "/Admin/Products/Edit", fields); Assert.Equal(HttpStatusCode.Redirect, saved.StatusCode);
        var p = await verify.Products.AsNoTracking().Include(x => x.Specifications).SingleAsync(x => x.Slug == slug); Assert.Equal(2, p.Specifications.Count);
        // Danh mục có sản phẩm phải bị giữ lại.
        await Post(client, "/Admin/Categories/Delete", "/Admin/Categories", new() { ["id"] = category.Id.ToString() });
        Assert.True(await verify.Categories.AnyAsync(x => x.Id == category.Id));
        fields["Id"] = p.Id.ToString(); fields["RowVersion"] = Convert.ToBase64String(p.RowVersion); fields["Name"] = "Updated product";
        Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/Admin/Products/Edit", "/Admin/Products/Edit/" + p.Id, fields)).StatusCode);
        var updated = await verify.Products.AsNoTracking().SingleAsync(x => x.Id == p.Id); Assert.Equal("Updated product", updated.Name);
        var deletion = await Post(client, "/Admin/Products/Delete", "/Admin/Products", new() { ["id"] = p.Id.ToString(), ["rowVersion"] = Convert.ToBase64String(updated.RowVersion) }); Assert.Equal(HttpStatusCode.Redirect, deletion.StatusCode);
        Assert.False(await verify.Products.AnyAsync(x => x.Id == p.Id));
        await Post(client, "/Admin/Categories/Delete", "/Admin/Categories", new() { ["id"] = category.Id.ToString() }); Assert.False(await verify.Categories.AnyAsync(x => x.Id == category.Id));
        var text = "<script>alert('test')</script>" + slug;
        Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/Contact", "/Contact", new() { ["Name"] = "Khách thử", ["Email"] = "demo@example.test", ["Message"] = text })).StatusCode);
        var contact = await verify.ContactMessages.AsNoTracking().SingleAsync(x => x.Message == text);
        var messages = await client.GetStringAsync("/Admin/Contact"); Assert.DoesNotContain(text, messages); Assert.Contains("&lt;script&gt;", messages);
        Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/Admin/Contact/Handle", "/Admin/Contact", new() { ["id"] = contact.Id.ToString() })).StatusCode);
        Assert.True((await verify.ContactMessages.AsNoTracking().SingleAsync(x => x.Id == contact.Id)).IsHandled);
    }
    [SqlFact]
    public async Task SeedIsRepeatableWithoutResettingStock()
    {
        await using var factory = fixture.Factory(); using var scope = factory.Services.CreateScope();
        await DemoSeeder.SeedAsync(scope.ServiceProvider);
        await using var db = fixture.NewDb(); var p = await db.Products.SingleAsync(x => x.Slug == "iphone17pro256"); p.Stock = 17; await db.SaveChangesAsync();
        using var scope2 = factory.Services.CreateScope(); await DemoSeeder.SeedAsync(scope2.ServiceProvider);
        await using var verify = fixture.NewDb(); Assert.Equal(17, (await verify.Products.SingleAsync(x => x.Slug == "iphone17pro256")).Stock);
        Assert.Equal(1, await verify.Products.CountAsync(x => x.Slug == "iphone17pro256"));
    }
    [SqlFact]
    public async Task InactiveProductAndEmptyCartCannotCreateOrder()
    {
        var setup = await fixture.ArrangeAsync();
        await using (var db = fixture.NewDb()) { var p = (await db.Products.FindAsync(setup.ProductId))!; p.IsActive = false; await db.SaveChangesAsync(); }
        await using var inactiveDb = fixture.NewDb();
        await Assert.ThrowsAsync<StoreException>(() => new OrderService(inactiveDb).PlaceAsync(setup.UserId, SqlFixture.Input(), [new(setup.ProductId, 1)]));
        await using var emptyDb = fixture.NewDb();
        await Assert.ThrowsAsync<StoreException>(() => new OrderService(emptyDb).PlaceAsync(setup.UserId, SqlFixture.Input(), []));
    }
    [SqlFact]
    public async Task WrongPasswordAndInvalidContactAreRejected()
    {
        await using var factory = fixture.Factory(); using var client = factory.CreateClient(new() { AllowAutoRedirect = false });
        var login = await Post(client, "/Account/Login", "/Account/Login", new() { ["Email"] = "missing@example.test", ["Password"] = "InvalidTestOnly!123" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        Assert.Contains("validation-summary-errors", await login.Content.ReadAsStringAsync());
        var contact = await Post(client, "/Contact", "/Contact", new() { ["Name"] = "", ["Email"] = "invalid", ["Message"] = "" });
        Assert.Equal(HttpStatusCode.OK, contact.StatusCode);
        Assert.Contains("validation-summary-errors", await contact.Content.ReadAsStringAsync());
    }
    [SqlFact]
    public async Task AllSeedAssetsAndDetailsRenderAndFiltersStayStable()
    {
        await using var factory = fixture.Factory(); using var client = factory.CreateClient(new() { AllowAutoRedirect = false });
        using (var scope = factory.Services.CreateScope()) await DemoSeeder.SeedAsync(scope.ServiceProvider);
        await using var db = fixture.NewDb();
        var seedCategories = new[] { "dien-thoai", "dong-ho", "phu-kien" };
        var paths = await db.Products.AsNoTracking().Where(x => seedCategories.Contains(x.Category!.Slug)).Select(x => new { x.ImagePath, x.Slug }).ToListAsync();
        Assert.Equal(16, paths.Count);
        foreach (var p in paths)
        {
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync(p.ImagePath)).StatusCode);
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Products/Details/" + p.Slug)).StatusCode);
        }
        var page = await client.GetStringAsync("/Products?category=phu-kien&sort=price-asc");
        Assert.DoesNotContain("iPhone 17 Pro 256GB", page);
        var names = Regex.Matches(page, "<h3>([^<]+)</h3>").Select(x => x.Groups[1].Value).ToArray();
        Assert.Equal(6, names.Length);
        var repeat = await client.GetStringAsync("/Products?category=phu-kien&sort=price-asc");
        Assert.Equal(names, Regex.Matches(repeat, "<h3>([^<]+)</h3>").Select(x => x.Groups[1].Value).ToArray());
    }
    [SqlFact]
    public async Task AdminOrderWorkflowPreservesOrderHistory()
    {
        var setup = await fixture.ArrangeAsync();
        await using var db = fixture.NewDb(); var order = await new OrderService(db).PlaceAsync(setup.UserId, SqlFixture.Input(), [new(setup.ProductId, 1)]);
        await using var factory = fixture.Factory(); using var client = await SignedClient(factory, true);
        foreach (var status in new[] { OrderStatus.Confirmed, OrderStatus.Shipping, OrderStatus.Completed })
        {
            Assert.Equal(HttpStatusCode.Redirect, (await Post(client, "/Admin/Orders/Update", "/Admin/Orders/Details/" + order.Id, new() { ["id"] = order.Id.ToString(), ["status"] = status.ToString() })).StatusCode);
            await using var verify = fixture.NewDb(); Assert.Equal(status, (await verify.Orders.FindAsync(order.Id))!.Status);
        }
        await using var lookup = fixture.NewDb(); var p = (await lookup.Products.FindAsync(setup.ProductId))!;
        await Post(client, "/Admin/Products/Delete", "/Admin/Products", new() { ["id"] = p.Id.ToString(), ["rowVersion"] = Convert.ToBase64String(p.RowVersion) });
        await using var final = fixture.NewDb(); Assert.False((await final.Products.FindAsync(p.Id))!.IsActive);
        Assert.Single(await final.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync());
    }
}
