# AGENTS.md - Lưu ý làm việc cho dự án sinh viên CNTT

## Thông tin người thực hiện

- Người thực hiện hiện là sinh viên năm nhất ngành Công nghệ thông tin.
- Khi hỗ trợ làm bài hoặc chỉnh sửa dự án, cần ưu tiên cách làm dễ hiểu, dễ giải thích và phù hợp trình độ hiện tại.
- Không nên tự ý viết code quá phức tạp nếu bài làm chỉ yêu cầu HTML/CSS/JavaScript cơ bản.

## Nguyên tắc trước khi làm

- Nếu yêu cầu chưa rõ, cần hỏi lại hoặc xác nhận trước khi sửa.
- Nếu có nhiều phương án, nên gợi ý ngắn gọn ưu/nhược điểm để người thực hiện chọn.
- Nếu người dùng đã nói rõ yêu cầu, thực hiện trực tiếp theo yêu cầu đó, không tự ý đổi hướng.
- Cần phân biệt rõ file chính và file tham khảo. Ví dụ: `phone_store...` là dự án cần sửa, còn `c.zip` chỉ dùng để tham khảo giao diện hoặc cách làm.

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
