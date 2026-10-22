# Nhật ký triển khai — 20/10/2026

- Từ HEAD `58b3fed` tạo nhánh `feature/aspnet-smartmobile`, giữ ba commit cục bộ. Không fetch/push/commit.
- Giữ HTML/CSS/JS và ảnh gốc làm tham khảo; cập nhật `about.html`, trang Razor giới thiệu và metadata thành dự án cá nhân theo yêu cầu mới. Tài liệu Word/PowerPoint được cập nhật; có bản sao trước chỉnh sửa trong scratch.
- Sửa `.gitignore`: bỏ quy tắc bỏ qua solution, thêm file sinh ra của .NET, DB, secrets, ảnh upload.
- Thêm solution web/test, global.json, local tool manifest, package lock, appsettings/launch profile localhost.
- Thêm Models/ViewModels, DbContext, initial migration/script, seed 16 sản phẩm/3 danh mục, lệnh tạo admin tương tác.
- Thêm Controllers/Services/Razor/Areas Admin cho tài khoản, sản phẩm, giỏ, đơn và liên hệ.
- Sao chép assets, chuyển menu sang URL MVC và thêm CSS màn hình nghiệp vụ; sửa menu mobile còn mở khi chuyển desktop.
- Thêm kiểm thử SQL/HTTP/nghiệp vụ, script khởi tạo có kiểm tra trùng database và script test có cờ duyệt.
- Được người dùng duyệt khởi động LocalDB, tạo/seed `SmartMobileStore_Dev` và tạo database test riêng `SmartMobileStore_Test_20261020`.
- NuGet audit ban đầu cảnh báo thư viện ảnh và dependency mật mã; đã bỏ thư viện ảnh đó, dùng giải mã Windows và bản dependency Microsoft đã cập nhật. Audit sau sửa không báo package dễ bị tổn thương từ nguồn hiện tại.
- .NET CLI lần đầu tự tạo certificate phát triển HTTPS; không chạy trust. Website dùng HTTP localhost.
- Không tạo admin/mật khẩu mặc định ở database demo; tài khoản test chỉ nằm trong database test.
- Chưa trust certificate, đổi hệ thống, cài SDK/workload, public deploy hoặc thay đổi dữ liệu ngoài hai database đã được duyệt.

Xem TEST_REPORT để biết bằng chứng, lệnh và giới hạn xác minh. Các phase được triển khai nối tiếp, kiểm thử được mở rộng theo chức năng và lỗi phát hiện; không xem việc đã viết code là bằng chứng đạt nghiệm thu.


- Cập nhật người thực hiện duy nhất Lê Ngọc Thuận, MSSV 2500016989, lớp 25DTH1C, năm học 2026, giảng viên Bùi Duy Tân theo xác nhận trực tiếp.
- Tạo lại báo cáo theo mẫu Baocao_DA_LTWeb.docx: giữ bìa/logo/bố cục bốn chương, thêm sơ đồ/ảnh thật và kết quả kiểm chứng; mẫu Downloads không sửa.
- Cập nhật slide 9 trang; tạo DOCX/PPTX bằng công cụ file, không thao tác Word theo yêu cầu mới.
- Sao lưu MDF/LDF trước khi gắn lại hai database hiện có do LocalDB thiếu đăng ký catalog; xác minh F5 HTTP 200 và chạy lại 37/37 test Release.
- Bổ sung biến SMARTMOBILE_TEST_SERVER tùy chọn cho test để dùng đúng server đang chạy; tên database test vẫn cố định, không cho trỏ test sang demo bằng biến này.

- Theo yêu cầu mới: kiểm thử lại 37/37, tạo bản publish Production chạy tại localhost:5182, thêm Run-PublishedDemo.ps1/Chay-Demo.cmd/Publish-Demo.ps1 và docs/DEMO.md hướng dẫn mở dự án trước giảng viên. Người dùng duyệt commit/push; giữ nhánh master remote không đổi, bàn giao ASP.NET trên feature/aspnet-smartmobile.

## 22/10/2026

- Theo yêu cầu: mở solution/F5 trực tiếp trong Visual Studio; thêm hiện/ẩn mật khẩu ở đăng ký, xác nhận và đăng nhập.
- Đặt Identity tối thiểu 6 ký tự, bỏ quy tắc chữ hoa/thường/số/ký hiệu; sửa hint web, lệnh tạo admin và README cho thống nhất; không reset tài khoản cũ.
- Thêm kiểm thử HTTP/SQL: mật khẩu 6 chữ thường, 6 số và ký tự lặp đăng ký/đăng nhập được, chỉ có vai trò User; mật khẩu dưới 6 hoặc xác nhận sai không tạo tài khoản.
- Bộ test phát hiện deadlock SQL 1205 được EF bọc trong InvalidOperationException. Bổ sung nhận diện qua chuỗi InnerException ở checkout/Admin; không bắt nhầm lỗi không liên quan, không đổi transaction hay tự retry.
- Kết quả cuối: 40/40 test đạt trên LocalDB test riêng. Cập nhật publish cục bộ theo lựa chọn trước đó; không hosting Internet.
