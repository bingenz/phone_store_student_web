# Mở dự án khi giảng viên yêu cầu

Người thực hiện: **Lê Ngọc Thuận**, MSSV **2500016989**, lớp **25DTH1C**, năm **2026**. Giảng viên **Bùi Duy Tân**.

## Trình bày mã nguồn bằng Visual Studio

1. Trên máy hiện tại, mở `C:\Users\Acer\Documents\phone_store_student_web\SmartMobileStore.sln`.
2. Chọn **SmartMobileStore** làm Startup Project, chọn profile **SmartMobileStore**, bấm **F5**.
3. Mở `http://localhost:5180`. Bấm **Shift+F5** để dừng debug.
4. Khi thầy hỏi code: mở `Controllers`, `Views`, `Models`, `Services`, `Data/StoreDbContext.cs` và `tests/SmartMobileStore.Tests`.

Luồng dễ giải thích: Controller nhận form → kiểm tra ViewModel → Service xử lý → DbContext đọc/ghi SQL → Razor View hiển thị. Đọc `docs/LEARNING_GUIDE.md` trước khi trình bày.

## Chạy bản publish không cần mở Visual Studio

Giải nén ZIP bản Demo vào một thư mục cố định trên máy này, rồi bấm **Chay-Demo.cmd**. Khi console báo đang lắng nghe, mở `http://localhost:5182`. Giữ cửa sổ console mở; Ctrl+C để dừng. Nếu chính sách máy chặn script, không thay đổi thiết lập bảo mật; dùng cách F5 phía trên.

Bản này dùng .NET 10 Runtime và LocalDB đã cài trên máy hiện tại; không phải ứng dụng tự mang database. Script chỉ đọc kiểm tra database `SmartMobileStore_Dev`, có thể dò named pipe hiện tại nếu alias LocalDB không phân giải; không tạo/xóa database và không ghi pipe tạm vào cấu hình. Chạy bằng cùng tài khoản Windows đã tạo dữ liệu. Mở F5 và publish ở hai cổng khác nhau vẫn cùng dùng database demo.

Muốn tạo admin mới từ bản publish, mở PowerShell trong thư mục đã giải nén:

```powershell
./Run-PublishedDemo.ps1 -CreateAdmin
```

Nhập email và mật khẩu riêng trực tiếp trong terminal; không có mật khẩu mặc định. Tài khoản đăng ký trên website chỉ là User.

## Kịch bản trình diễn khoảng 5 phút

1. Trang chủ → tìm/lọc sản phẩm → xem chi tiết → thêm giỏ.
2. Đăng ký/đăng nhập User → checkout COD với thông tin giả → xem đơn của mình.
3. Đăng nhập Admin → xem đơn → xác nhận/đang giao/hoàn tất; thử hủy một đơn khác trước khi giao.
4. Tạo/sửa sản phẩm, gửi liên hệ rồi đánh dấu xử lý trong Admin.
5. Giải thích server tự lấy giá SQL; transaction lưu đơn và trừ kho cùng lúc; khóa submit ngăn đơn trùng.

Hành động demo ghi dữ liệu thật vào database demo. Không chạy test vào database này. Test dùng `SmartMobileStore_Test_20261020` riêng.

## Chuẩn bị trước buổi học

Chạy thử một lần trước buổi học; chuẩn bị tài khoản User/Admin, báo cáo và slide; cắm sạc. Bản publish không cần Internet để chạy nghiệp vụ cục bộ, nhưng bản đồ/nguồn tài nguyên bên ngoài có thể không tải khi offline. `localhost` chỉ mở trên chính máy đang chạy; GitHub lưu source, không tự chạy backend hay SQL.

Nếu đem sang máy khác, phải chuẩn bị .NET/LocalDB, khởi tạo database mới theo README sau khi chủ máy cho phép, hoặc chuyển database bằng quy trình sao lưu/khôi phục. Tài khoản/đơn/ảnh upload không đi theo Git push hay ZIP source. Không xóa MDF/LDF hoặc chạy lại khởi tạo trên dữ liệu hiện có khi gặp lỗi.

Tạo lại bản publish từ source trong terminal tại repo, chọn thư mục đích mới:

```powershell
./scripts/Publish-Demo.ps1 -OutputDirectory 'D:\SmartMobile-Demo-New'
```
