using Microsoft.AspNetCore.Identity;

namespace SmartMobileStore.Services;

public class AccountErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError PasswordTooShort(int length) => new()
    {
        Code = nameof(PasswordTooShort),
        Description = $"Mật khẩu cần ít nhất {length} ký tự."
    };
}
