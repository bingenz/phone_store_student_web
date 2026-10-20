# Hướng dẫn đọc và trình bày bài

## 1. MVC là gì?

**Model** mô tả dữ liệu, ví dụ Product có tên, giá, tồn kho. **View** là HTML có Razor để hiển thị dữ liệu. **Controller** nhận URL/request rồi gọi xử lý và chọn View.

Ví dụ mở `/Products/Details/iphone17pro256`: ProductsController nhận slug → EF truy vấn sản phẩm và thông số → trả Model cho Details.cshtml → Razor viết tên/giá vào HTML → trình duyệt tải CSS và ảnh. Không tìm thấy thì HTTP 404, không tự thay bằng iPhone mặc định như bản JS cũ.

Hãy đặt breakpoint ở `ProductsController.Details`, bấm F5 rồi mở trang chi tiết. Xem `id`, `product` và danh sách Specifications trong Locals của Visual Studio.

## 2. DbContext và EF Core

`StoreDbContext` là cầu nối giữa C# và SQL Server. `db.Products.Where(...)` được EF dịch thành truy vấn tham số hóa. `AsNoTracking` dùng cho dữ liệu chỉ đọc, giảm việc theo dõi thay đổi. `await` chờ thao tác I/O mà không giữ thread trong lúc SQL đang xử lý.

Migration là mã mô tả phiên bản schema; script SQL là bản có thể xem xét trước khi áp dụng. Tạo migration không đồng nghĩa đã sửa database. Seed bổ sung dữ liệu demo, không reset tồn kho. DbContext được DI cấp theo từng HTTP request, không dùng một instance chung giữa nhiều người.

## 3. CRUD và ViewModel

Admin GET Edit đọc sản phẩm và chuyển sang ProductInput. POST Edit kiểm tra ModelState, category/SKU/slug, giá và ảnh rồi mới cập nhật entity. `SaveChangesAsync` ghi thay đổi xuống SQL. ViewModel không có trường UserId/Role cho client tự gán.

Thử nhập giá âm, slug trùng hoặc xóa danh mục còn sản phẩm. Giải thích rằng kiểm tra ở trình duyệt chỉ giúp trải nghiệm; server vẫn phải kiểm tra vì request có thể bị sửa.

## 4. Đăng nhập và quyền

UserManager tạo tài khoản và hash mật khẩu; SignInManager kiểm tra mật khẩu rồi cấp cookie xác thực. `[Authorize]` yêu cầu đăng nhập; `Roles="Admin"` yêu cầu vai trò. Ẩn nút quản trị chỉ là giao diện, không phải biện pháp bảo vệ chính.

Tạo hai User khác nhau, đăng nhập lần lượt và thử URL đơn của người kia: phải không xem được. Không đưa mật khẩu vào slide, source hoặc ảnh chụp màn hình.

## 5. Giỏ hàng và transaction

Session giỏ chỉ giữ ID/số lượng. Khi mua, server lấy giá và tồn kho từ SQL. Transaction đảm bảo việc tạo đơn và trừ kho cùng thành công hoặc cùng rollback. Rowversion giống “phiên bản bản ghi”: nếu một người đã cập nhật sau lúc admin mở form, form cũ không được ghi đè.

Hai người tranh sản phẩm cuối: chỉ một đơn được tạo, tồn không âm. Cùng khóa submit: trả đơn cũ, không trừ kho lần hai. Hủy đơn hợp lệ: hoàn kho một lần. Đơn dùng giá chụp tại lúc mua, nên sửa giá sản phẩm sau đó không làm thay đổi lịch sử.

## 6. Thứ tự demo với thầy

1. Giới thiệu cấu trúc solution và so sánh trang HTML gốc với Razor.
2. Tìm/lọc sản phẩm, xem chi tiết một phụ kiện.
3. Đăng ký User, thêm giỏ, đặt COD, xem đơn.
4. Đăng nhập Admin, thêm/sửa sản phẩm và thử validation.
5. Xử lý đơn hoặc hủy trước khi giao, quan sát kho.
6. Gửi liên hệ và đánh dấu xử lý trong admin.
7. Mở SSMS với Windows Authentication, xem database demo và bảng Orders/OrderItems.
8. Chạy test và giải thích một test thiếu hàng, một test phân quyền.

Đọc báo cáo kiểm thử để phân biệt đã kiểm chứng tự động, đã quan sát trình duyệt và phần còn cần kiểm tra trong IDE/rubric.
