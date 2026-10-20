# Chức năng và màn hình

| Đường dẫn | Người dùng | Hành vi |
|---|---|---|
| `/` | Mọi người | Trang chủ, điện thoại/đồng hồ và giá từ SQL |
| `/Products` | Mọi người | `q`, `category`, `sort`, `page`; phân trang 8 sản phẩm |
| `/Products/Accessories` | Mọi người | Chuyển sang danh mục phụ kiện |
| `/Products/Details/{slug}` | Mọi người | Giá, ảnh, thông số; 404 nếu sai ID/ngừng bán |
| `/Home/About`, `/Home/News` | Mọi người | Nội dung bài tập gốc; tin tức được ghi rõ minh họa |
| `/Contact` | Mọi người | GET form, POST validation và lưu liên hệ |
| `/Account/Register`, `/Account/Login` | Mọi người | Identity, validation, không tự cấp Admin |
| `/Account/Logout` | Đã đăng nhập | Chỉ POST; xóa session khi đăng xuất |
| `/Cart` | Mọi người | Giỏ server; POST thêm, cập nhật, xóa và xóa tất cả |
| `/Orders/Checkout` | Đã đăng nhập | Thông tin nhận hàng, COD, server kiểm tra giá/kho |
| `/Orders`, `/Orders/Details/{id}` | Đã đăng nhập | Chỉ đơn của tài khoản hiện tại |
| `/Admin/Products` | Admin | CRUD, upload, rowversion; sản phẩm có đơn chuyển ngừng bán khi xóa |
| `/Admin/Categories` | Admin | CRUD; không xóa danh mục còn sản phẩm |
| `/Admin/Orders` | Admin | Xem đơn/chi tiết, chuyển trạng thái hợp lệ |
| `/Admin/Contact` | Admin | Xem nội dung đã encode, đánh dấu xử lý |

Các thao tác ghi là POST có CSRF; API JSON công khai không được thêm. Giá VNĐ decimal; COD demo không thực hiện giao dịch tiền thật. Không có xóa tài khoản/đơn hàng hoặc reset mật khẩu tự động.

## Ánh xạ giao diện cũ

| Nguồn | Bản mới |
|---|---|
| `index.html` | Home/Index, `_ProductCard` và dữ liệu SQL |
| `product-detail.html` | Products/Details; không fallback khi ID sai |
| `accessories.html` | Products/Index với category `phu-kien` |
| `cart.html`, `cart.js` | Cart/Index, CartService, Orders/Checkout |
| `about.html`, `news.html`, `contact.html` | Home/About, Home/News, Contact/Index |
| Header/footer lặp | Views/Shared/_Layout |
| `style.css`, `images/` | Bản sao trong wwwroot; file gốc không đổi |

Phụ kiện dùng bố cục thẻ sản phẩm chung, có chi tiết hoạt động thay cho `href="#"`. Tài khoản/admin/checkout là màn hình mới bổ sung. Tìm kiếm/lọc nằm ở trang danh sách, không thêm mục Chi tiết vào header.
