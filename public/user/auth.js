// Chạy khi trang web vừa tải xong
document.addEventListener("DOMContentLoaded", function() {
    checkLoginState();
});

// Hàm kiểm tra trạng thái đăng nhập
function checkLoginState() {
    // Lấy trạng thái đăng nhập từ localStorage
    var isLoggedIn = localStorage.getItem("isLoggedIn");
    var username = localStorage.getItem("username");
    
    // Tìm tất cả các nút có class "btn-login-header" (trên desktop và mobile)
    var authBtns = document.querySelectorAll(".btn-login-header");
    
    if (isLoggedIn === "true") {
        // Nếu đã đăng nhập
        authBtns.forEach(function(btn) {
            // Thay đổi text và link thành Đăng xuất
            btn.innerHTML = "Đăng xuất (" + username + ")";
            btn.href = "#";
            
            // Xử lý sự kiện click để đăng xuất
            btn.onclick = function(e) {
                e.preventDefault(); // Ngăn hành vi link mặc định
                
                // Xóa trạng thái trong localStorage
                localStorage.removeItem("isLoggedIn");
                localStorage.removeItem("username");
                
                // Thông báo và tải lại trang để cập nhật giao diện
                alert("Bạn đã đăng xuất thành công!");
                window.location.reload();
            };
        });
    } else {
        // Nếu chưa đăng nhập
        authBtns.forEach(function(btn) {
            btn.innerHTML = "Đăng nhập";
            btn.href = "login.html";
            btn.onclick = null;
        });
    }
}
