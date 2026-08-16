# SmartMobile Store - Dự Án Web Bán Hàng Cơ Bản

Đây là dự án thiết kế giao diện website bán điện thoại di động và phụ kiện, được xây dựng theo tiêu chuẩn HTML/CSS/JS thuần, tối ưu cho sinh viên IT năm nhất làm đồ án/bài tập. 

Dự án chú trọng vào tính dễ đọc, code có cấu trúc rõ ràng, giao diện responsive và các chức năng mô phỏng cơ bản của một website bán hàng.

## 🧑‍💻 Thành viên nhóm (Thực hiện dự án)
- **Lê Ngọc Thuận**
- **Phạm Nguyễn Hoàng Tiến**
- **Nguyễn Minh Tân**

---

## 🛠 Cấu trúc dự án & Công nghệ

Dự án được viết bằng **HTML5, CSS3, Vanilla JavaScript**. Code không sử dụng thư viện UI nặng nề để đảm bảo tính gọn nhẹ và dễ hiểu để sinh viên có thể giải thích trực tiếp.

### Danh sách các trang khách hàng (trong thư mục `public/user/`):

- 🏠 `public/user/index.html`: Trang chủ - Tổng hợp banner, điện thoại, đồng hồ và sản phẩm ưu đãi.
- 📲 `app-download.html`: Trang tải ứng dụng - Hiển thị ảnh giới thiệu và liên kết tải SmartMobile trên App Store, Google Play.
- 📱 `product-detail.html`: Trang chi tiết sản phẩm - Hiển thị ảnh, thông số, mô tả, tồn kho và chức năng thêm vào giỏ hàng.
- 🛒 `cart.html`: Trang giỏ hàng - Giới hạn số lượng theo tồn kho, tạo đơn mô phỏng và tính tổng tiền bằng JavaScript.
- 📰 `news.html`: Trang tin tức công nghệ - Cập nhật thông tin và xu hướng công nghệ mới.
- 👥 `about.html`: Trang giới thiệu - Thông tin về lịch sử hình thành, triết lý kinh doanh và thông tin nhóm.
- 📞 `contact.html`: Trang liên hệ - Gồm form gửi tin nhắn chuyên nghiệp và bản đồ đường đi.
- 🔑 `login.html`: Trang đăng nhập - Form đăng nhập giả lập, lưu trạng thái qua localStorage.
- 📝 `register.html`: Trang đăng ký - Form đăng ký giả lập với xác nhận mật khẩu.
- ⭐ `client-partner.html`: Trang đánh giá - Hiển thị các đánh giá giả lập của khách hàng theo dạng lưới.

### Cấu trúc dự án

- `public/index.html`: Entry point chạy thử, chuyển tới `public/user/index.html`.
- `public/user/`: Toàn bộ trang, CSS và JavaScript phía khách hàng.
- `public/admin/`: Dashboard, quản lý đơn hàng, sản phẩm và đăng nhập quản trị.
- `public/images/`: Hình ảnh dùng chung của website.
- `public/shared/store-data.js`: Nguồn dữ liệu sản phẩm, migration và hàm đọc/ghi tồn kho, đơn hàng.
- `.gitignore`, `AGENTS.md`, `README.md`: Cấu hình và hướng dẫn ở thư mục gốc.

> Báo cáo Word và PowerPoint cũ không được lưu trong repository để tránh nhầm lẫn với bản nộp chính thức.

> **Lưu ý cấu trúc:** không tạo lại `index.html`, `user/`, `admin/`, `images/` hoặc `shared/` ở thư mục gốc. Không đặt thêm một thư mục dự án cùng tên bên trong workspace.

### Danh sách các tệp hệ thống phía khách hàng:
- 🎨 `public/user/style.css`: File định dạng giao diện cho các trang khách hàng.
- ⚙️ `public/user/cart.js`: Xử lý giỏ hàng bằng `localStorage`.
- 🔄 `public/user/admin-products.js`: Đồng bộ tên, giá, ảnh và tồn kho từ admin lên trang chủ.
- 🔐 `public/user/auth.js`: Quản lý đăng nhập/đăng xuất giả lập của khách hàng.

---

## ✨ Tính năng nổi bật

1. **Giao diện hiện đại & Đẹp mắt:** Giao diện tối giản, gam màu sáng hiện đại theo xu hướng e-commerce thực tế.
2. **Responsive:** Có bố cục cho máy tính, máy tính bảng và điện thoại, kèm menu mobile.
3. **Giỏ hàng và tồn kho:** Không cho mua hàng hết kho hoặc chọn vượt số lượng hiện có; giá và tồn kho lấy từ dữ liệu admin.
4. **Đăng nhập / Đăng ký giả lập:** Hệ thống xác thực giả lập sử dụng localStorage — khi đăng nhập, nút trên header tự đổi thành "Đăng xuất (tên)" trên mọi trang.
5. **Đặt hàng mô phỏng:** Khách nhập họ tên và số điện thoại; đơn được lưu cho admin, tồn kho bị trừ và giỏ được xóa sau khi đặt thành công.

## 🚀 Hướng dẫn cài đặt và chạy thử

Để admin, trang user, giỏ hàng và tồn kho dùng chung `localStorage`, dự án cần được chạy bằng **Live Server** hoặc một web server cục bộ. Không nên mở từng file bằng `file://` hoặc chạy admin và user ở hai cổng khác nhau.

1. Cài đặt VS Code.
2. Tải extension `Live Server` (bởi Ritwick Dey).
3. Click chuột phải vào file `public/index.html` và chọn **"Open with Live Server"**.
4. Website tự chuyển tới `http://127.0.0.1:5500/public/user/index.html`.
5. Trang quản trị mở tại `http://127.0.0.1:5500/public/admin/login.html`.

Admin và user phải giữ nguyên cùng giao thức, tên miền và cổng, ví dụ đều dùng `http://127.0.0.1:5500`.

## Dữ liệu và giới hạn mô phỏng

- Sản phẩm dùng key `smartmobile_admin_products`, đơn hàng dùng `smartmobile_admin_orders`, giỏ hàng dùng `smartmobile_cart` trong `localStorage`.
- `public/shared/store-data.js` khởi tạo đủ 16 sản phẩm. Migration chỉ bổ sung ID thiếu và giữ dữ liệu admin đã sửa hoặc tự thêm.
- Đây là dự án học tập không có backend, cơ sở dữ liệu máy chủ, thanh toán trực tuyến hoặc đồng bộ giữa nhiều trình duyệt/thiết bị.
- Đăng ký, đăng nhập user/admin và form liên hệ hiện chỉ là mô phỏng; không dùng cho dữ liệu hay mật khẩu thật.
- Nút App Store/Google Play hiện dẫn đến trang kho ứng dụng chung, chưa phải ứng dụng SmartMobile thật.

## Đề xuất cải thiện tiếp theo

1. Xây dựng xác thực tài khoản thật thay cho việc chấp nhận dữ liệu đăng nhập giả lập.
2. Lưu hoặc gửi nội dung form liên hệ thay vì chỉ hiển thị thông báo.
3. Thay liên kết kho ứng dụng, email, số điện thoại và mạng xã hội mẫu bằng thông tin thật.
4. Cập nhật nội dung khuyến mãi đã hết hạn và tách trạng thái “Hết hàng” khỏi nhóm “Sắp hết”.
5. Điều chỉnh vị trí nút Zalo/Facebook trên mobile để không che nút thao tác ở cạnh phải màn hình.

---
*Dự án phục vụ mục đích học tập môn Thiết kế Web.*

## Deploy sau này

Hiện tại cấu trúc mới chưa được cấu hình deploy lại và không còn `index.html` ở thư mục gốc. Khi cần triển khai Cloudflare Pages hoặc nền tảng tương tự:

- Framework preset: `None`.
- Build command: để trống.
- Build output directory: `public`.
- Không sao chép nội dung `public/` ra thư mục gốc và không tạo thêm một bản dự án lồng.

## Cập nhật giao diện mới

- Header chỉ giữ các điều hướng chính: Điện thoại, Đồng hồ và Đăng nhập. Logo được dùng để quay về trang chủ.
- Liên kết Giới thiệu, Tin tức và Liên hệ được chuyển xuống footer.
- Các sản phẩm ưu đãi được hiển thị trực tiếp bên dưới mục Đồng hồ trong `public/user/index.html`.
- Ảnh sản phẩm dùng đường dẫn tương đối đến các file thật trong `public/images`.
- Khi kiểm tra, mở `public/index.html` bằng Live Server rồi kiểm tra ảnh, tồn kho ở trang chủ, chi tiết sản phẩm, giỏ hàng và trang admin trên cùng địa chỉ/cổng.
- Khu vực tải ứng dụng SmartMobile được tách sang trang `app-download.html` và mở bằng liên kết `Tải app` trên header. Khu vực này dùng các class `app-download-section`, `app-download-grid` và `store-badge`, hiển thị hai cột trên máy tính và một cột trên điện thoại.
- Hai badge tải ứng dụng dùng ảnh tại `public/images/app-store-badge.svg` và `public/images/google-play-badge.png`, mở liên kết trong tab mới. Ảnh điện thoại nền trong suốt được lưu tại `public/images/smartmobile-app-mockup-floating.png`.
- Trang đánh giá khách hàng dùng các class `review-summary-grid`, `client-review-grid` và `client-review-card`. Lưới có 3 cột trên máy tính, 2 cột trên máy tính bảng và 1 cột trên điện thoại.
- Cụm nút liên hệ dùng class `social-buttons` và `social-button`, được đặt cố định ở góc phải dưới của tất cả các trang.
