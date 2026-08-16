// cart.js - Quản lý giỏ hàng và kiểm tra tồn kho bằng localStorage.
var CART_KEY = "smartmobile_cart";

function getCart() {
    try {
        var data = JSON.parse(localStorage.getItem(CART_KEY));
        return Array.isArray(data) ? data : [];
    } catch (error) {
        return [];
    }
}

function saveCart(cart) {
    localStorage.setItem(CART_KEY, JSON.stringify(cart));
}

function getInventoryProduct(productId) {
    if (typeof getStoreProductById === "function") {
        return getStoreProductById(productId);
    }

    // Fallback cho các trang phụ chỉ nhúng cart.js để hiện badge giỏ hàng.
    try {
        var products = JSON.parse(localStorage.getItem("smartmobile_admin_products"));
        if (!Array.isArray(products)) return null;
        return products.find(function (product) {
            return String(product.id) === String(productId);
        }) || null;
    } catch (error) {
        return null;
    }
}

function parsePrice(priceValue) {
    if (typeof priceValue === "number") return priceValue;
    return parseInt(String(priceValue).replace(/\./g, "").replace("đ", ""), 10) || 0;
}

function formatPrice(value) {
    return Number(value).toLocaleString("vi-VN") + "đ";
}

function escapeCartText(value) {
    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function getCartItemQuantity(cart, productId) {
    var item = cart.find(function (cartItem) {
        return String(cartItem.id) === String(productId);
    });
    return item ? Number(item.quantity) : 0;
}

function addToCart(id) {
    var product = getInventoryProduct(id);
    if (!product) {
        return { ok: false, message: "Sản phẩm không còn kinh doanh." };
    }

    var stock = Number(product.stock);
    if (stock <= 0) {
        return { ok: false, message: "Sản phẩm đã hết hàng." };
    }

    var cart = getCart();
    var currentQuantity = getCartItemQuantity(cart, id);
    if (currentQuantity >= stock) {
        return { ok: false, message: "Bạn đã chọn tối đa " + stock + " sản phẩm đang có trong kho." };
    }

    var item = cart.find(function (cartItem) {
        return String(cartItem.id) === String(id);
    });
    if (item) {
        item.quantity += 1;
        item.name = product.name;
        item.price = formatPrice(product.price);
        item.image = product.image;
    } else {
        cart.push({
            id: String(product.id),
            name: product.name,
            price: formatPrice(product.price),
            image: product.image,
            quantity: 1
        });
    }

    saveCart(cart);
    updateCartBadge();
    return { ok: true, message: "Đã thêm sản phẩm vào giỏ." };
}

function removeFromCart(id) {
    var cart = getCart().filter(function (item) {
        return String(item.id) !== String(id);
    });
    saveCart(cart);
    updateCartBadge();
}

function clearCart() {
    saveCart([]);
    updateCartBadge();
}

function setCartQuantity(id, requestedQuantity) {
    var product = getInventoryProduct(id);
    if (!product || Number(product.stock) <= 0) {
        removeFromCart(id);
        return { ok: false, removed: true, message: "Sản phẩm đã hết hàng và được xóa khỏi giỏ." };
    }

    var quantity = parseInt(requestedQuantity, 10);
    if (!Number.isInteger(quantity) || quantity < 1) quantity = 1;

    var stock = Number(product.stock);
    var limited = quantity > stock;
    if (limited) quantity = stock;

    var cart = getCart();
    var item = cart.find(function (cartItem) {
        return String(cartItem.id) === String(id);
    });
    if (!item) return { ok: false, message: "Không tìm thấy sản phẩm trong giỏ." };

    item.quantity = quantity;
    item.name = product.name;
    item.price = formatPrice(product.price);
    item.image = product.image;
    saveCart(cart);
    updateCartBadge();

    return {
        ok: true,
        limited: limited,
        quantity: quantity,
        message: limited ? "Số lượng đã được giới hạn theo tồn kho: " + stock + "." : ""
    };
}

function updateQuantity(id, delta) {
    var cart = getCart();
    var item = cart.find(function (cartItem) {
        return String(cartItem.id) === String(id);
    });
    if (!item) return { ok: false, message: "Không tìm thấy sản phẩm trong giỏ." };

    var nextQuantity = Number(item.quantity) + Number(delta);
    if (nextQuantity <= 0) {
        removeFromCart(id);
        return { ok: true, removed: true, message: "" };
    }
    return setCartQuantity(id, nextQuantity);
}

// Cập nhật giá, tên, ảnh và số lượng trong giỏ theo dữ liệu kho mới nhất.
function syncCartWithInventory() {
    var cart = getCart();
    var syncedCart = [];
    var messages = [];

    cart.forEach(function (item) {
        var product = getInventoryProduct(item.id);
        if (!product) {
            messages.push(item.name + " không còn kinh doanh và đã được xóa khỏi giỏ.");
            return;
        }

        var stock = Number(product.stock);
        if (stock <= 0) {
            messages.push(product.name + " đã hết hàng và được xóa khỏi giỏ.");
            return;
        }

        var quantity = Math.max(1, parseInt(item.quantity, 10) || 1);
        if (quantity > stock) {
            quantity = stock;
            messages.push(product.name + " chỉ còn " + stock + " sản phẩm; số lượng trong giỏ đã được điều chỉnh.");
        }

        syncedCart.push({
            id: String(product.id),
            name: product.name,
            price: formatPrice(product.price),
            image: product.image,
            quantity: quantity
        });
    });

    if (JSON.stringify(cart) !== JSON.stringify(syncedCart)) saveCart(syncedCart);
    updateCartBadge();
    return { cart: syncedCart, messages: messages };
}

function getCartCount() {
    return getCart().reduce(function (total, item) {
        return total + (parseInt(item.quantity, 10) || 0);
    }, 0);
}

function updateCartBadge() {
    var badge = document.getElementById("cartBadge");
    if (!badge) return;

    var count = getCartCount();
    badge.textContent = count;
    badge.style.display = count > 0 ? "flex" : "none";
}

updateCartBadge();

document.addEventListener("click", function (event) {
    var button = event.target.closest(".add-cart-btn");
    if (!button || !button.getAttribute("data-id") || button.disabled) return;

    var result = addToCart(button.getAttribute("data-id"));
    if (!result.ok) {
        window.alert(result.message);
        return;
    }

    var originalText = button.textContent;
    button.textContent = "✓ Đã thêm!";
    button.disabled = true;

    setTimeout(function () {
        var product = getInventoryProduct(button.getAttribute("data-id"));
        var selectedQuantity = getCartItemQuantity(getCart(), button.getAttribute("data-id"));
        if (!product || Number(product.stock) <= 0) {
            button.textContent = "Hết hàng";
            button.disabled = true;
        } else if (selectedQuantity >= Number(product.stock)) {
            button.textContent = "Đã chọn tối đa";
            button.disabled = true;
        } else {
            button.textContent = originalText;
            button.disabled = false;
        }
    }, 1200);
});

window.addEventListener("storage", function (event) {
    if (event.key === CART_KEY || event.key === "smartmobile_admin_products") {
        updateCartBadge();
    }
});
