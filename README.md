# SmartMobile Store — ASP.NET Core MVC

**Người thực hiện duy nhất: Lê Ngọc Thuận.** Hình thức: dự án cá nhân. Thuận phụ trách giao diện, backend C#/ASP.NET, SQL Server, kiểm thử, báo cáo và thuyết trình; không còn phân công nhóm. MSSV **2500016989**, lớp học phần **25DTH1C**, năm học **2026**, giảng viên **Bùi Duy Tân**.

Bài tập cửa hàng điện thoại, chuyển từ website HTML/CSS/JavaScript sang C# và SQL Server. Website HTML cũ ở thư mục gốc được giữ làm tham khảo; `about.html` được cập nhật người thực hiện theo yêu cầu. Bản ASP.NET nằm trong `SmartMobileStore/`.

## Chạy ngay trên máy hiện tại

Đã khởi tạo `SmartMobileStore_Dev` trên `(localdb)\MSSQLLocalDB` theo phê duyệt ngày 20/10/2026, có 3 danh mục và 16 sản phẩm. Không cần chạy lại script khởi tạo. Chưa tạo tài khoản admin cho người sử dụng; không có mật khẩu mặc định.

1. Mở `SmartMobileStore.sln` bằng **Visual Studio 2026**, chọn **SmartMobileStore** làm Startup Project.
2. Chọn launch profile **SmartMobileStore**, bấm **F5**. Địa chỉ: `http://localhost:5180`.
3. Đăng ký một tài khoản để thử đặt hàng; đơn của mỗi người được bảo vệ riêng.
4. Tạo admin trong Terminal tương tác tại thư mục repo:

```powershell
dotnet run --project SmartMobileStore --no-launch-profile -- --create-admin
```

Nhập email mới và mật khẩu khi được hỏi. Mật khẩu nhập ẩn, ít nhất 10 ký tự gồm hoa/thường, số và ký hiệu. Lệnh không reset hoặc nâng quyền tài khoản đã tồn tại. Không gửi mật khẩu qua chat. Đăng nhập tài khoản này, bấm **Quản trị**.

Chạy bằng dòng lệnh:

```powershell
dotnet tool restore
dotnet restore --locked-mode
dotnet build
dotnet run --project SmartMobileStore
```

## Yêu cầu và cấu hình

- Windows, Visual Studio 2026 với workload **ASP.NET and web development**.
- SDK 10.0.401, ASP.NET Core 10; phiên bản SDK được ghi trong `global.json`.
- LocalDB và Windows Authentication; SSMS chỉ là công cụ quản lý, không thay thế Database Engine.
- EF Core, Identity, công cụ EF cục bộ và các thư viện Microsoft được khóa 10.0.13. Có `packages.lock.json` cho hai project.
- Bộ giải mã ảnh dùng `System.Drawing.Common` nên chức năng upload của bản này chỉ hỗ trợ Windows.

`appsettings.json` chứa connection string **không có mật khẩu** cho LocalDB. Có thể ghi đè bằng `ConnectionStrings__Store` trong môi trường. Không đặt thông tin xác thực thật trong Git. Profile chỉ nghe localhost HTTP; muốn HTTPS/trust certificate hoặc public deployment cần cấu hình và phê duyệt riêng.

## Khởi tạo trên máy khác / database mới

Chỉ thực hiện sau khi chủ máy đồng ý khởi động LocalDB và tạo database. Xem trước `docs/sql/InitialStore.sql`.

```powershell
dotnet restore --locked-mode
dotnet tool restore
./scripts/Initialize-Demo.ps1 -Approved
```

Script kiểm tra database bằng Windows Authentication, dừng nếu `SmartMobileStore_Dev` đã tồn tại, rồi áp dụng migration và seed. Không DROP/TRUNCATE, không ghi đè MDF. Không chạy lại với database hiện tại để “sửa nhanh”. Khi có lỗi, xem thông báo và kiểm tra dữ liệu trước.

Sinh lại script để xem xét, không thay đổi SQL:

```powershell
dotnet ef migrations script --idempotent --project SmartMobileStore --output docs/sql/InitialStore.sql
```

Chỉ sau khi đã duyệt schema/database, áp dụng migration bằng script khởi tạo. Website **không tự migrate hoặc seed khi khởi động**. Seed chỉ thêm sản phẩm chưa có, không đặt lại giá/tồn kho. Bản publish cũng mang theo JSON demo.

## Kiểm thử

```powershell
dotnet test
```

Mặc định chỉ chạy test không cần SQL; SQL tests được đánh dấu skip. Để chạy đầy đủ trên database mới đã được duyệt:

```powershell
./scripts/Test-Sql.ps1 -Approved
```

Trên **máy hiện tại**, database `SmartMobileStore_Test_20261020` đã được tạo bởi bộ test trong phiên này; cho phép dùng lại đúng database test đó bằng:

```powershell
./scripts/Test-Sql.ps1 -Approved -ReuseKnownTestDatabase
```

Không dùng cờ reuse nếu chưa xác minh đó là database test. Tests thêm dữ liệu riêng bằng ID ngẫu nhiên, có CRUD trên các bản ghi test; không xóa database, không xóa dữ liệu ngoài các bản ghi test do chính test tạo. Database được giữ lại để xem kết quả. Không trỏ integration tests vào database demo hoặc dữ liệu thật.

## Chức năng

- Trang chủ, danh mục, phụ kiện, chi tiết; tìm kiếm, lọc, sắp xếp, phân trang 8 sản phẩm.
- Đăng ký/đăng nhập/đăng xuất Identity, vai trò User/Admin, khóa tạm sau 5 lần sai mật khẩu.
- Giỏ session server; đăng nhập trước khi checkout; COD, phí vận chuyển demo 0đ.
- SQL lưu đơn và chi tiết; giá được tính server; transaction và rowversion bảo vệ tồn kho; chống submit trùng.
- Admin CRUD sản phẩm/danh mục, ảnh JPEG/PNG, trạng thái đơn và liên hệ.
- Giới thiệu cập nhật người thực hiện duy nhất; tin tức, khuyến mãi và đánh giá là minh họa.

## Lỗi thường gặp

| Triệu chứng | Cách kiểm tra |
|---|---|
| Không kết nối SQL | Kiểm tra `SqlLocalDB info MSSQLLocalDB`; nếu dừng, chủ máy có thể chạy `SqlLocalDB start MSSQLLocalDB`. Kiểm tra đúng Windows account và database |
| Trang lỗi sau khi chạy lần đầu trên máy mới | Database chưa khởi tạo; xem quy trình xin duyệt phía trên, không tự xóa DB |
| Cổng 5180 đã dùng | Dừng phiên chạy cũ hoặc chạy `dotnet run --project SmartMobileStore --no-launch-profile --urls http://localhost:5182` |
| Không đăng ký được | Xem validation; seed phải có vai trò User; email không được trùng |
| Không vào admin được | Tài khoản đăng ký thường không có quyền Admin; tạo admin mới bằng lệnh tương tác |
| Sửa sản phẩm bị báo dữ liệu thay đổi | Tồn kho hoặc sản phẩm vừa cập nhật; tải lại trang sửa, không ghi đè rowversion |
| Không xóa được danh mục | Chuyển sản phẩm sang danh mục khác trước |
| Upload bị từ chối | JPEG/PNG thật, tối đa 5 MB, 6000 px mỗi chiều và 16 triệu điểm ảnh; đổi đuôi file không đủ |

Giỏ session nằm trong bộ nhớ của tiến trình: mất giỏ khi ứng dụng khởi động lại/hết phiên là giới hạn đã chọn. Tài khoản, sản phẩm, đơn và liên hệ nằm trong SQL, không mất theo tiến trình. Ảnh upload không được Git theo dõi; khi sao lưu/chuyển máy phải sao lưu `wwwroot/uploads/` cùng database. Không tự dọn/xóa ảnh cũ.

## Tài liệu và trạng thái

Xem `docs/ARCHITECTURE.md`, `docs/FEATURES.md`, `docs/TEST_REPORT.md`, `docs/LEARNING_GUIDE.md`, `docs/CHANGELOG.md` và `docs/ASSETS.md`.

Đã build bằng MSBuild của Visual Studio, mở solution và chạy F5 trực tiếp trong IDE; localhost trả HTTP 200. Công nghệ Core/.NET 10 vẫn cần đối chiếu rubric giảng viên nếu nhận được. Bản ASP.NET dùng nhánh `feature/aspnet-smartmobile`; nhánh `master` trên GitHub có bản HTML khác, không tự ghi đè hoặc gộp hai bản. Chưa triển khai hosting công khai. Chưa xác minh giấy phép ảnh, chỉ tái sử dụng tài nguyên bài tập hiện có.

### Mở khi giảng viên yêu cầu

Mở `SmartMobileStore.sln` → chọn project/profile **SmartMobileStore** → **F5** → `http://localhost:5180`. Hoặc dùng bản publish: mở **Chay-Demo.cmd** → `http://localhost:5182`. Xem `docs/DEMO.md` để biết kịch bản 5 phút, tạo admin và chuyển máy. GitHub lưu source; database, tài khoản và đơn hàng nằm riêng trên máy.

Tạo bản publish vào thư mục mới bằng `./scripts/Publish-Demo.ps1 -OutputDirectory 'D:\SmartMobile-Demo-New'`. Bản publish cần .NET 10 Runtime và SQL trên Windows, có sẵn trên máy hiện tại.


### Tài liệu nộp bài

`tài liệu/bao_cao_smartmobile.docx` được tạo theo mẫu `Baocao_DA_LTWeb.docx`, cập nhật sinh viên, MSSV, lớp, năm học và giảng viên đã xác nhận. `tài liệu/smartmobile_thuyet_trinh.pptx` có 9 slide và ghi chú trình bày. Mẫu gốc không bị sửa. Mục lục báo cáo là field: cần cập nhật nếu chỉnh nội dung; xem trước khi in vì chưa kiểm chứng phân trang bằng Microsoft Word.

### Khi LocalDB có file nhưng không thấy database

Không chạy lại khởi tạo hoặc xóa MDF/LDF. Kiểm tra đúng Windows account, instance và catalog; sao lưu file trước khi cân nhắc attach. Phiên này đã gắn lại hai database hiện có sau khi sao lưu, rồi xác nhận F5 và test. Named pipe của LocalDB thay đổi khi restart; không chép pipe tạm thời vào cấu hình mặc định. Xem báo cáo kiểm thử để biết cách test được chạy trên đúng instance.
