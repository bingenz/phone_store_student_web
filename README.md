# SmartMobile Store - Dự Án Web Bán Hàng Cơ Bản

Đây là dự án thiết kế giao diện website bán điện thoại di động và phụ kiện, được xây dựng theo tiêu chuẩn HTML/CSS/JS thuần, tối ưu cho sinh viên IT năm nhất làm đồ án/bài tập. 

Dự án chú trọng vào tính dễ đọc, code cấu trúc tốt, giao diện responsive hiện đại và hỗ trợ đầy đủ các trang cần thiết của một trang thương mại điện tử cơ bản.

## 🧑‍💻 Thành viên nhóm (Thực hiện dự án)
- **Lê Ngọc Thuận**
- **Phạm Nguyễn Hoàng Tiến**
- **Nguyễn Minh Tân**

---

## 🛠 Cấu trúc dự án & Công nghệ

Dự án được viết bằng **HTML5, CSS3, Vanilla JavaScript**. Code không sử dụng thư viện UI nặng nề để đảm bảo tính gọn nhẹ và dễ hiểu để sinh viên có thể giải thích trực tiếp.

### Danh sách các trang chính (9 trang):

- 🏠 `index.html`: Trang chủ - Tổng hợp banner, các mẫu điện thoại nổi bật, đồng hồ giảm giá và ưu đãi.
- 📱 `product-detail.html`: Trang chi tiết sản phẩm - Hiển thị ảnh tĩnh, thông số, mô tả chi tiết, chức năng chọn màu sắc và thêm vào giỏ hàng.
- 🛒 `cart.html`: Trang giỏ hàng - Mô phỏng giỏ hàng với thông tin chi tiết và tính tổng tiền mua sắm (lưu qua LocalStorage).
- 🎁 `accessories.html`: Trang phụ kiện/Ưu đãi - Trưng bày đầy đủ các dòng phụ kiện như Tai nghe, Ốp lưng, Cục sạc.
- 📰 `news.html`: Trang tin tức công nghệ - Cập nhật thông tin và xu hướng công nghệ mới.
- 👥 `about.html`: Trang giới thiệu - Thông tin về lịch sử hình thành, triết lý kinh doanh và thông tin nhóm.
- 📞 `contact.html`: Trang liên hệ - Gồm form gửi tin nhắn chuyên nghiệp và bản đồ đường đi.
- 🔑 `login.html`: Trang đăng nhập - Form đăng nhập giả lập, lưu trạng thái qua localStorage.
- 📝 `register.html`: Trang đăng ký - Form đăng ký giả lập với xác nhận mật khẩu.

### Danh sách các tệp hệ thống:
- 🎨 `style.css`: File định dạng giao diện cho toàn bộ 9 trang HTML (Có Responsive Design: Desktop, Tablet, Mobile).
- ⚙️ `cart.js`: Tập lệnh JavaScript đơn giản xử lý Logic Thêm/Xóa/Sửa giỏ hàng sử dụng `localStorage`.
- 🔐 `auth.js`: Script quản lý trạng thái đăng nhập/đăng xuất giả lập (dùng localStorage).
- 🐍 `make_pages.py`: Script Python sinh tự động các trang phụ (about, news, contact, accessories, login, register).

---

## ✨ Tính năng nổi bật

1. **Giao diện hiện đại & Đẹp mắt:** Giao diện tối giản, gam màu sáng hiện đại theo xu hướng e-commerce thực tế.
2. **Responsive Hoàn hảo:** Tương thích trên cả PC và Thiết bị di động (Mobile Menu Hamburger).
3. **Giỏ hàng (Cart Logic):** Logic thêm sản phẩm, cập nhật số lượng và tính tổng tiền hiển thị hoạt động ngay lập tức thông qua LocalStorage.
4. **Đăng nhập / Đăng ký giả lập:** Hệ thống xác thực giả lập sử dụng localStorage — khi đăng nhập, nút trên header tự đổi thành "Đăng xuất (tên)" trên mọi trang.
5. **Code tiêu chuẩn & sạch:** Tuân thủ đúng các tag `<header>`, `<nav>`, `<main>`, `<section>`, `<footer>` thuận tiện cho giải thích đồ án.

## 🚀 Hướng dẫn cài đặt và chạy thử

Dự án có thể chạy trực tiếp bằng trình duyệt mà không cần cài đặt Web Server, nhưng để trải nghiệm tốt nhất (đặc biệt là tính năng localStorage), bạn nên dùng Extension **Live Server**.

1. Cài đặt VS Code.
2. Tải extension `Live Server` (bởi Ritwick Dey).
3. Click chuột phải vào file `index.html` chọn **"Open with Live Server"**.
4. Website sẽ tự mở ở `http://127.0.0.1:5500/index.html`.

---
*Dự án phục vụ mục đích học tập môn Thiết kế Web.*
