# AGENTS.md - Lưu ý làm việc cho dự án sinh viên CNTT

## Thông tin người thực hiện

- Người thực hiện hiện là sinh viên năm nhất ngành Công nghệ thông tin.
- Khi hỗ trợ làm bài hoặc chỉnh sửa dự án, cần ưu tiên cách làm dễ hiểu, dễ giải thích và phù hợp trình độ hiện tại.
- Không nên tự ý viết code quá phức tạp nếu bài làm chỉ yêu cầu HTML/CSS/JavaScript cơ bản.

## Nguyên tắc trước khi làm

- Nếu yêu cầu chưa rõ, cần hỏi lại hoặc xác nhận trước khi sửa.
- Nếu có nhiều phương án, nên gợi ý ngắn gọn ưu/nhược điểm để người thực hiện chọn.
- Nếu người dùng đã nói rõ yêu cầu, thực hiện trực tiếp theo yêu cầu đó, không tự ý đổi hướng.

## Cấu trúc dự án bắt buộc

- Toàn bộ file chạy website nằm trong `public/`; không tạo lại `index.html`, `user/`, `admin/`, `images/` hoặc `shared/` ở thư mục gốc.
- `public/index.html` là entry point khi chạy thử. Trang khách hàng nằm trong `public/user/`, quản trị trong `public/admin/`, ảnh trong `public/images/` và dữ liệu dùng chung trong `public/shared/`.
- Thư mục gốc chỉ giữ file cấu hình và tài liệu Markdown như `.gitignore`, `AGENTS.md`, `README.md`. Không lưu báo cáo Word/PowerPoint cũ hoặc bản nháp trong repository; nếu sau này cần đưa bản chính thức lên GitHub thì tạo lại thư mục `docs/` theo yêu cầu cụ thể.
- Hiện tại chưa triển khai lại hosting. Khi người dùng yêu cầu deploy sau này, ưu tiên cấu hình build output là `public/`; không sao chép website ra gốc và không tạo thêm bản lồng.

## Phạm vi chỉnh sửa

- Chỉ chỉnh sửa trong đúng phạm vi yêu cầu hiện tại.
- Không tự ý thêm nút, menu, section, hiệu ứng hoặc chức năng mới ngoài yêu cầu.
- Không thay đổi nội dung/trang đang đúng nếu không liên quan trực tiếp đến yêu cầu.
- Nếu phát hiện phần code cũ đang sai với yêu cầu, cần xóa hoặc sửa sạch phần sai trước khi thêm code mới.
- Trước khi thay đổi lớn, cần xác nhận lại nếu yêu cầu chưa đủ rõ.

## Nguyên tắc khi chỉnh code

- Không xóa hoặc thay đổi chức năng đang đúng nếu không cần thiết.
- Không thêm nút, menu hoặc section ngoài yêu cầu.
- Nếu đã làm sai, nên quay lại từ file gốc hoặc xóa sạch phần sai trước khi làm lại.
- Code cần có cấu trúc rõ ràng, tên class dễ hiểu, dễ tìm trong HTML/CSS.
- Với trang mới, nên tách thành file riêng nếu yêu cầu là “trang riêng”.
- Link chuyển trang nên dùng cùng tab (`target="_self"`) nếu cần người dùng có thể quay lại bằng Back.

## Nguyên tắc dữ liệu sản phẩm và tồn kho

- Mỗi `id` sản phẩm chỉ dùng một nguồn dữ liệu chính cho tên, giá, ảnh và tồn kho: `smartmobile_admin_products` trong `localStorage`.
- Trang chủ, trang chi tiết, giỏ hàng và trang admin phải cùng đọc nguồn dữ liệu sản phẩm dùng chung trong `public/shared/store-data.js`.
- Sản phẩm có tồn kho bằng `0`, đã bị xóa hoặc không còn trong dữ liệu admin thì không được thêm vào giỏ hay đặt hàng.
- Khi tăng hoặc nhập số lượng trong giỏ, luôn giới hạn theo tồn kho hiện tại.
- Trước khi tạo đơn phải kiểm tra lại tồn kho, sau đó mới lưu đơn, trừ kho và xóa giỏ.
- Khi thay đổi cấu trúc dữ liệu `localStorage`, cần có migration một lần; không ghi đè sản phẩm admin đã sửa hoặc tự thêm.

## Nguyên tắc về tài liệu và ghi chú

- Mỗi lần chỉnh sửa nên có ghi chú ngắn về các file đã sửa và lý do sửa.
- Nếu có thêm code mới, nên có tài liệu nhỏ hoặc phần giải thích để sinh viên có thể trình bày lại.
- Khi tạo tính năng, nên mô tả ngắn: tính năng nằm ở file nào, class nào, cách hoạt động ra sao.

## Nguyên tắc về giao diện web bán hàng

- Header chỉ nên chứa các mục điều hướng chính như Trang chủ, Điện thoại, Đồng hồ, Liên hệ.
- Không thêm mục `Chi tiết` vào header nếu chi tiết sản phẩm là trang con mở từ thẻ sản phẩm.
- Trang chi tiết sản phẩm nên có: hình ảnh, tên sản phẩm, giá, mô tả, thông số, đánh giá giả lập và nút thao tác chính.
- Nút `Xem chi tiết` nên nằm trong thẻ sản phẩm hoặc khi bấm vào sản phẩm, không đặt trong header.
- Nếu người dùng muốn quay lại bằng Back của trình duyệt, không cần thêm nút `Quay lại` trong giao diện.
- Thông số sản phẩm có thể dùng `ul/li` hoặc `ol/li` để dễ đọc, tránh lạm dụng nhiều box dạng lưới khi không cần thiết.

## Khi bàn giao

- Nêu rõ file nào đã thay đổi.
- Nêu rõ cách kiểm tra nhanh.
- Nếu tạo bản zip, đặt tên file dễ hiểu.
- Không nói là đã hoàn thành 100% nếu chưa kiểm tra cơ bản đường dẫn, file HTML và CSS.
- Trước khi bàn giao, kiểm tra dự án không còn thư mục bản sao lồng cùng tên bên trong workspace.
