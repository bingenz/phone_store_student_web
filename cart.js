// ============================================================
// cart.js - Quản lý giỏ hàng bằng localStorage
// Tác dụng: Được nhúng vào index.html, product-detail.html, cart.html
// Sinh viên đọc từng phần theo ghi chú bên dưới.
// ============================================================

// Tên key để nhận diện dữ liệu trong localStorage
// (giống như đặt tên cho "ngăn kéo" lưu trữ)
var CART_KEY = 'smartmobile_cart';


// ────────────────────────────────────────────────────────────
// PHẦN 1: LẤY VÀ LƯU GIỎ HÀNG VÀO localStorage
// ────────────────────────────────────────────────────────────

// Lấy dữ liệu giỏ hàng từ localStorage
// - Nếu đã có dữ liệu → chuyển chuỗi JSON thành mảng rồi trả về
// - Nếu chưa có gì → trả về mảng rỗng []
function getCart() {
    var data = localStorage.getItem(CART_KEY);
    if (data) {
        // JSON.parse: chuyển chuỗi '["a","b"]' → mảng ["a","b"]
        return JSON.parse(data);
    }
    return [];
}

// Lưu mảng giỏ hàng vào localStorage
// localStorage chỉ lưu chuỗi, nên phải dùng JSON.stringify để chuyển mảng → chuỗi
function saveCart(cart) {
    localStorage.setItem(CART_KEY, JSON.stringify(cart));
}


// ────────────────────────────────────────────────────────────
// PHẦN 2: THÊM / XÓA / THAY ĐỔI SỐ LƯỢNG SẢN PHẨM
// ────────────────────────────────────────────────────────────

// Thêm sản phẩm vào giỏ hàng
// - Nếu sản phẩm (cùng id) đã có trong giỏ → tăng số lượng lên 1
// - Nếu chưa có → thêm mới với quantity = 1
function addToCart(id, name, price, image) {
    var cart  = getCart();
    var found = false;

    // Duyệt qua từng sản phẩm trong giỏ để kiểm tra đã tồn tại chưa
    for (var i = 0; i < cart.length; i++) {
        if (cart[i].id === id) {
            cart[i].quantity += 1;  // Đã có → tăng số lượng
            found = true;
            break;
        }
    }

    // Nếu chưa tìm thấy → thêm sản phẩm mới vào cuối mảng
    if (!found) {
        cart.push({
            id:       id,
            name:     name,
            price:    price,    // Lưu dạng chuỗi, ví dụ: "33.990.000đ"
            image:    image,
            quantity: 1
        });
    }

    saveCart(cart);     // Lưu lại vào localStorage
    updateCartBadge();  // Cập nhật số hiển thị trên nút giỏ hàng
}

// Xóa một sản phẩm khỏi giỏ hàng theo id
function removeFromCart(id) {
    var cart = getCart();

    // filter() giữ lại tất cả phần tử KHÔNG có id trùng → loại bỏ sản phẩm cần xóa
    cart = cart.filter(function(item) {
        return item.id !== id;
    });

    saveCart(cart);
    updateCartBadge();
}

// Xóa TOÀN BỘ giỏ hàng (lưu mảng rỗng vào localStorage)
// Dùng cho nút "Xóa tất cả" trên trang giỏ hàng
function clearCart() {
    saveCart([]);       // Ghi đè bằng mảng rỗng []
    updateCartBadge();  // Badge về 0 → tự ẩn
}


// Thay đổi số lượng một sản phẩm trong giỏ
// delta = +1 (tăng) hoặc -1 (giảm)
// Nếu số lượng giảm xuống 0 hoặc âm → tự động xóa sản phẩm khỏi giỏ
function updateQuantity(id, delta) {
    var cart = getCart();

    for (var i = 0; i < cart.length; i++) {
        if (cart[i].id === id) {
            cart[i].quantity += delta;  // Cộng hoặc trừ 1

            // Số lượng về 0 hoặc âm → xóa khỏi mảng
            // splice(vị_trí_bắt_đầu, số_phần_tử_cần_xóa)
            if (cart[i].quantity <= 0) {
                cart.splice(i, 1);
            }
            break;
        }
    }

    saveCart(cart);
    updateCartBadge();
}


// ────────────────────────────────────────────────────────────
// PHẦN 3: TÍNH TOÁN VÀ ĐỊNH DẠNG GIÁ TIỀN
// ────────────────────────────────────────────────────────────

// Chuyển chuỗi giá "33.990.000đ" thành số nguyên 33990000
// Cần thiết để cộng/nhân tính tổng tiền
function parsePrice(priceStr) {
    // Bước 1: replace(/\./g, '') → xóa tất cả dấu chấm (33.990.000 → 33990000)
    // Bước 2: replace('đ', '') → xóa chữ "đ"
    // Bước 3: parseInt(..., 10) → chuyển chuỗi sang số nguyên
    return parseInt(priceStr.replace(/\./g, '').replace('đ', ''), 10);
}

// Chuyển số 33990000 thành chuỗi "33.990.000đ"
// toLocaleString('vi-VN') tự thêm dấu chấm ngăn cách hàng nghìn theo chuẩn Việt Nam
function formatPrice(n) {
    return n.toLocaleString('vi-VN') + 'đ';
}


// ────────────────────────────────────────────────────────────
// PHẦN 4: BADGE SỐ LƯỢNG TRÊN NÚT GIỎ HÀNG NỔI
// ────────────────────────────────────────────────────────────

// Đếm tổng số lượng sản phẩm trong giỏ (cộng hết tất cả quantity)
// Ví dụ: 2 iPhone + 1 Samsung = tổng 3
function getCartCount() {
    var cart  = getCart();
    var total = 0;
    for (var i = 0; i < cart.length; i++) {
        total += cart[i].quantity;
    }
    return total;
}

// Cập nhật số hiển thị trên badge của nút giỏ hàng nổi (góc phải màn hình)
// - Hiện badge nếu có sản phẩm trong giỏ
// - Ẩn badge nếu giỏ trống
function updateCartBadge() {
    var badge = document.getElementById('cartBadge');
    if (!badge) return;  // Không tìm thấy phần tử thì bỏ qua (tránh lỗi)

    var count = getCartCount();

    if (count > 0) {
        badge.textContent   = count;   // Hiển thị số lượng
        badge.style.display = 'flex';  // Hiện badge
    } else {
        badge.style.display = 'none';  // Ẩn badge
    }
}


// ────────────────────────────────────────────────────────────
// PHẦN 5: KHỞI ĐỘNG - Chạy ngay khi file được tải
// Script được đặt cuối <body> nên DOM đã sẵn sàng khi code này chạy
// ────────────────────────────────────────────────────────────

// Hiển thị số lượng trên badge ngay khi trang mở (dữ liệu lấy từ localStorage)
updateCartBadge();

// Lắng nghe sự kiện click trên toàn bộ trang (kỹ thuật: event delegation)
// Thay vì gắn riêng cho từng nút, ta bắt ở cấp document rồi kiểm tra bên trong
// → Hoạt động được kể cả khi nút được tạo động bằng JavaScript (product-detail.html)
document.addEventListener('click', function(e) {
    // closest() tìm phần tử gần nhất khớp selector (kể cả chính e.target)
    // Nếu người dùng click vào chữ bên trong nút, closest vẫn tìm được nút cha
    var btn = e.target.closest('.add-cart-btn');

    // Bỏ qua nếu không phải nút "Thêm vào giỏ" hoặc không có data-id
    if (!btn || !btn.getAttribute('data-id')) return;

    // Lấy thông tin sản phẩm từ các thuộc tính data-* của nút
    var id    = btn.getAttribute('data-id');
    var name  = btn.getAttribute('data-name');
    var price = btn.getAttribute('data-price');
    var image = btn.getAttribute('data-image');

    addToCart(id, name, price, image);

    // Hiệu ứng phản hồi: đổi text nút thành "✓ Đã thêm!" trong 1.2 giây
    var original    = btn.textContent;
    btn.textContent = '✓ Đã thêm!';
    btn.disabled    = true;

    setTimeout(function() {
        btn.textContent = original;
        btn.disabled    = false;
    }, 1200);
});
