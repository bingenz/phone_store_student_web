using System.Drawing;
using System.Drawing.Imaging;
namespace SmartMobileStore.Services;

public class ImageService(IWebHostEnvironment environment)
{
    public Task<string> SaveAsync(IFormFile file)
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(6, 1)) throw new StoreException("Bản upload này dùng bộ giải mã ảnh của Windows.");
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (file.Length is <= 0 or > 5 * 1024 * 1024 || extension is not (".jpg" or ".jpeg" or ".png"))
            throw new StoreException("Ảnh phải là JPEG/PNG và không quá 5 MB.");
        using var stream = file.OpenReadStream();
        try
        {
            using var image = Image.FromStream(stream, useEmbeddedColorManagement: false, validateImageData: true);
            if (image.Width > 6000 || image.Height > 6000 || (long)image.Width * image.Height > 16000000)
                throw new StoreException("Ảnh quá lớn; tối đa 6000 px mỗi chiều và 16 triệu điểm ảnh.");
            var isPng = image.RawFormat.Guid == ImageFormat.Png.Guid;
            if (!isPng && image.RawFormat.Guid != ImageFormat.Jpeg.Guid) throw new StoreException("Nội dung file không phải JPEG/PNG.");
            var folder = Path.Combine(environment.WebRootPath, "uploads");
            Directory.CreateDirectory(folder);
            var name = Guid.NewGuid().ToString("N") + (isPng ? ".png" : ".jpg");
            var path = Path.Combine(folder, name);
            // Vẽ vào bitmap mới để loại metadata và nội dung thừa; chỉ lưu ảnh đã giải mã.
            using var clean = new Bitmap(image.Width, image.Height);
            using (var graphics = Graphics.FromImage(clean)) graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            clean.Save(path, isPng ? ImageFormat.Png : ImageFormat.Jpeg);
            return Task.FromResult("/uploads/" + name);
        }
        catch (ArgumentException) { throw new StoreException("Ảnh bị hỏng hoặc không hợp lệ."); }
        catch (OutOfMemoryException) { throw new StoreException("Không giải mã được ảnh hoặc ảnh quá lớn."); }
    }
}
