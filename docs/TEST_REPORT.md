# Báo cáo kiểm chứng — 20/10/2026

## Môi trường

Windows 11 x64, VS Community 2026 18.10.3, SDK 10.0.401, runtime 10.0.12, EF/Identity 10.0.13. LocalDB MSSQLLocalDB 17.0.4025.3, Windows Authentication. Nhánh `feature/aspnet-smartmobile`, bắt đầu từ HEAD `58b3fed`.

## Kết quả đã thực hiện

| Kiểm tra | Kết quả |
|---|---|
| Restore package và local EF tool | Thành công, phiên bản khóa cụ thể, có lock file |
| `dotnet build -c Release --no-restore` | 0 errors, 0 warnings |
| MSBuild.exe của VS 2026, solution /restore Debug | Thành công |
| `dotnet publish SmartMobileStore -c Release` | Thành công vào thư mục scratch; không deploy |
| Sinh script migration idempotent | Thành công, `docs/sql/InitialStore.sql` |
| `dotnet ef migrations has-pending-model-changes` | Không có thay đổi model chưa được migration ghi nhận |
| Khởi tạo demo sau duyệt SQL | Thành công; 3 danh mục, 16 sản phẩm; không có User/Order test trong demo |
| SQL integration + HTTP + business tests | **37 passed, 0 failed, 0 skipped**, database test riêng |
| NuGet audit cả hai project, gồm transitive | Không phát hiện package dễ bị tổn thương theo nguồn audit tại thời điểm chạy |
| Chạy ứng dụng và mở trình duyệt localhost | Trang chủ/danh sách hiển thị dữ liệu từ SQL |
| Trang chủ 375, 768, 1440 px | Đã quan sát; chiều rộng nội dung không vượt viewport |
| Menu mobile → desktop | Đã phát hiện/sửa lỗi menu còn mở; kiểm chứng aria-expanded=false và menu display=none sau chuyển desktop |
| Mọi ảnh và chi tiết của 16 sản phẩm seed | HTTP 200 trong integration test |
| Website HTML gốc | Giữ làm tham khảo; `about.html` cập nhật người thực hiện duy nhất theo yêu cầu |
| F5 trong Visual Studio 2026 | Đã mở solution, chạy debug; sau gắn lại database hiện có, localhost:5180 trả HTTP 200 |
| Báo cáo và slide | Báo cáo theo mẫu Baocao_DA_LTWeb.docx, đủ 4 chương; slide 9 trang; cập nhật thông tin sinh viên và xem ảnh render |

Lệnh test đầy đủ đã chạy với `SMARTMOBILE_SQL_APPROVED=1` và `SMARTMOBILE_REUSE_APPROVED_TEST=1` sau khi database test được tạo trong phiên được duyệt. Có thể tái hiện bằng `scripts/Test-Sql.ps1 -Approved -ReuseKnownTestDatabase` trên đúng database test đó.

## Nội dung 37 test

- Ma trận chuyển trạng thái, validation nhận hàng, giỏ session và xoay khóa checkout.
- Upload giả đuôi, SVG/file hỏng bị từ chối; JPEG/PNG thật được giải mã và ghi tên ngẫu nhiên.
- Năm trang công khai render; năm URL bảo vệ yêu cầu đăng nhập; POST thiếu CSRF trả 400; trang không tồn tại trả 404.
- Cấu hình SQL không hợp lệ trả trang lỗi 500 không lộ connection string/stack trace.
- Giá server và chi tiết đơn lưu đúng SQL; submit lại không tạo đơn/trừ kho thêm.
- Thiếu kho rollback; hai giao dịch mua sản phẩm cuối không bán vượt kho.
- Hủy đơn hoàn kho đúng một lần; admin sửa dữ liệu cũ không ghi đè kho sau checkout.
- SQL từ chối tồn âm và xóa danh mục đang dùng.
- Catalog/ID sai, seed lặp không trùng/không reset kho, sản phẩm ngừng bán/giỏ trống.
- User đăng ký thật không xem được đơn người khác hoặc vào admin; đăng nhập ReturnUrl bên ngoài bị đưa về trang chủ.
- Luồng HTTP đăng nhập → thêm giỏ → checkout với giá giả mạo → submit lặp → đăng xuất.
- CRUD admin sản phẩm/danh mục, chặn xóa danh mục còn hàng, liên hệ encode XSS và đánh dấu xử lý.
- Admin xử lý đơn qua các trạng thái và ngừng bán sản phẩm có lịch sử thay vì xóa cứng.
- Sai mật khẩu và form liên hệ không hợp lệ; mọi ảnh/trang chi tiết seed và thứ tự lọc ổn định.

Một lần mở rộng test bị lỗi do truy vấn test dùng so sánh hoa/thường trên SQL collation không phân biệt case, vô tình chọn sản phẩm test đã ngừng bán. Đã sửa truy vấn giới hạn đúng ba danh mục seed và xác nhận đủ 16 sản phẩm; lần chạy cuối đạt 37/37. Không sửa hành vi 404 đúng của ứng dụng để làm test qua.

## Giới hạn và phần cần người dùng kiểm tra

- Đã chạy F5 trực tiếp trong Visual Studio. Website dùng profile HTTP localhost; người dùng đã chọn No ở hộp thoại trust chứng chỉ IIS Express. Không coi đây là xác minh HTTPS.
- Chưa có rubric giảng viên; cần đối chiếu Core/.NET 10 với yêu cầu học phần.
- Database demo chưa có admin mật khẩu mặc định. Người dùng tạo admin bằng lệnh tương tác trong README; luồng quyền admin đã được test bằng tài khoản riêng trong database test.
- Responsive đã quan sát trang chủ và danh sách; chưa kiểm tra thủ công từng màn hình admin trên mọi thiết bị hoặc audit WCAG đầy đủ.
- Giỏ session không tồn tại sau restart; kiểm chứng dữ liệu sản phẩm/đơn qua DbContext/request mới dùng SQL thật. Không coi session là nguồn lưu đơn hàng.
- Chưa xác minh giấy phép ảnh, chưa kiểm thử public hosting, HTTPS trust, thanh toán/email thật.
- File mẫu dư của template vẫn còn vì bước dọn đệ quy bị hệ thống tự động từ chối `blocked by policy`; giao diện mới không tham chiếu Bootstrap/jQuery của template.

Không có DROP/TRUNCATE, reset dữ liệu thật, push, commit hoặc deploy. Giữ cả hai database đã được duyệt để người dùng kiểm tra.


## Lần xác minh cuối và sự cố LocalDB

Lần chạy cuối `dotnet test -c Release` đạt **37 passed, 0 failed, 0 skipped**. File kết quả: `work/test-results/smartmobile-final.trx` trong thư mục công việc Codex. Hai cờ duyệt SQL và reuse đã bật; `SMARTMOBILE_TEST_SERVER` được đặt bằng named pipe của instance đang chạy do alias LocalDB trong tiến trình công cụ không đồng nhất với phiên Visual Studio. Cấu hình mặc định vẫn là `(localdb)\MSSQLLocalDB`; không ghi pipe tạm thời vào ứng dụng.

Khi F5 không tìm thấy database, đã xác nhận MDF/LDF còn tồn tại nhưng catalog instance đang chạy chưa đăng ký hai DB. Đã sao lưu cả bốn file vào `work/sql-before-attach` trước khi dùng FOR ATTACH gắn lại database hiện có; không DROP/TRUNCATE, không rebuild log và không seed đè. Đọc lại demo: 3 danh mục, 16 sản phẩm, 0 tài khoản, 0 đơn ở thời điểm xác minh. Sau đó F5 và test đều chạy được.

Báo cáo DOCX được tạo bằng công cụ file từ bản sao mẫu người dùng cung cấp; mẫu gốc giữ nguyên SHA-256. Không dùng Microsoft Word sau yêu cầu của người dùng. Bộ render LibreOffice không có sẵn; đã dùng engine tài liệu đi kèm để kiểm tra 17 trang, giữ nguyên control TOC, PAGE/PAGEREF, logo, styles, numbering và hai section. Cache mục lục điền theo phân trang đã kiểm tra; bật updateFields để cập nhật khi tài liệu thay đổi. Chưa xác nhận lại phân trang bằng Microsoft Word; khi in/nộp cần xem trước và cập nhật mục lục nếu có thay đổi.

Kiểm tra bàn giao cuối: đã dừng/chạy lại F5 sau cập nhật Razor About; trang chủ và About trả HTTP 200, About có 'Người thực hiện duy nhất', không còn hai tên thành viên cũ. File nguồn và bản đang chạy đã thống nhất.

## Kiểm thử trước push và publish cục bộ theo yêu cầu mới

- Chạy lại dotnet test -c Release: 37 passed, 0 failed, 0 skipped; file smartmobile-prepush.trx trong scratch.
- NuGet audit web project và dependency gián tiếp: không báo package dễ bị tổn thương theo nguồn hiện tại.
- Publish Release thành công. Bản Production chạy tại localhost:5182; chín trang công khai trả HTTP 200, slug sản phẩm sai trả 404; About có tác giả duy nhất và không có hai tên cũ.
- Thử bản bàn giao thực tế bằng Windows PowerShell 5.1 giống launcher Chay-Demo.cmd tại cổng 5183: About HTTP 200. Launcher chỉ đọc kiểm tra SQL và dò pipe khi alias lỗi, không tạo/xóa DB hoặc thay đổi thiết lập hệ thống.
- Người dùng chọn publish trên máy để trình diễn; không yêu cầu hosting công khai. Cấp quyền commit/push riêng trong yêu cầu mới. Fetch cho thấy master trên remote có thay đổi HTML khác; chỉ push nhánh feature/aspnet-smartmobile, không ghi đè master.
- Quét mẫu private key/token/AWS key trên file văn bản không phát hiện đường dẫn nghi vấn; không xem đây là kiểm toán đầy đủ lịch sử Git.

## Xác minh cập nhật tài khoản 22/10/2026

- dotnet test -c Release: 40 passed, 0 failed, 0 skipped; kết quả password-update-final.trx trong scratch.
- HTTP/SQL: chấp nhận mật khẩu 6 chữ thường, 6 chữ số, 6 ký tự lặp; đăng xuất/đăng nhập lại được, mật khẩu lưu dạng hash, tài khoản chỉ có User và không vào Admin.
- Password dưới 6 và xác nhận sai: từ chối bằng thông báo tiếng Việt, không tạo user trong SQL.
- Browser thật tại localhost:5180: từng nút Hiện/Ẩn đổi type password/text đúng ở đăng ký/đăng nhập; ô xác nhận độc lập; phím Space toggle được và không submit form. Chỉ nhập chuỗi minh họa, không submit tài khoản demo qua UI.
- Lần đầu kiểm thử đầy đủ phát hiện EF bọc deadlock SQL 1205 bằng InvalidOperationException; đã sửa nhận diện lỗi ở checkout/Admin và test concurrency. Bộ test cuối 40/40, ràng buộc không bán vượt kho vẫn kiểm tra số đơn/kho. Không coi test thất bại ban đầu là đạt.
- Không đổi schema hoặc reset dữ liệu demo; chỉ thêm bản ghi vào database test đã duyệt.
- Kiểm tra breakpoint 375 px qua trình duyệt bị timeout; chưa kết luận breakpoint mới từ lần thử đó. Ảnh form ở viewport mặc định đã được kiểm tra, không thấy chồng lấn.

- Bản publish Production mới chạy qua Windows PowerShell launcher tại localhost:5182: Home/Register/Login/JS HTTP 200; Register có hai nút hiện/ẩn, Login có một nút; hint 6 ký tự đã có trong HTML publish.
