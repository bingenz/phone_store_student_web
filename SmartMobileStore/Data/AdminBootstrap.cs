using Microsoft.AspNetCore.Identity;
using SmartMobileStore.Models;
namespace SmartMobileStore.Data;

public static class AdminBootstrap
{
    public static async Task RunAsync(IServiceProvider services)
    {
        if (Console.IsInputRedirected) throw new InvalidOperationException("Chạy tương tác trong terminal để nhập mật khẩu an toàn.");
        Console.Write("Email admin mới: ");
        var email = Console.ReadLine()?.Trim() ?? "";
        if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email)) throw new InvalidOperationException("Email không hợp lệ.");
        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        if (await users.FindByEmailAsync(email) != null) throw new InvalidOperationException("Tài khoản đã tồn tại; không tự nâng quyền hoặc reset mật khẩu.");
        Console.Write("Mật khẩu (ít nhất 10 ký tự, hoa/thường, số, ký hiệu): ");
        var password = ReadPassword();
        Console.Write("Nhập lại mật khẩu: ");
        if (ReadPassword() != password) throw new InvalidOperationException("Mật khẩu xác nhận không khớp.");
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roles.RoleExistsAsync("Admin"))
        {
            var r = await roles.CreateAsync(new IdentityRole("Admin"));
            if (!r.Succeeded) throw new InvalidOperationException("Không tạo được vai trò Admin.");
        }
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await users.CreateAsync(user, password);
        if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
        var assigned = await users.AddToRoleAsync(user, "Admin");
        if (!assigned.Succeeded) throw new InvalidOperationException("Đã tạo tài khoản nhưng chưa gán được Admin. Cần kiểm tra lại.");
        Console.WriteLine("Đã tạo admin. Không ghi mật khẩu vào tài liệu hoặc Git.");
    }
    private static string ReadPassword()
    {
        var value = new System.Text.StringBuilder(); ConsoleKeyInfo key;
        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace) { if (value.Length > 0) value.Length--; }
            else if (!char.IsControl(key.KeyChar)) value.Append(key.KeyChar);
        }
        Console.WriteLine(); return value.ToString();
    }
}
