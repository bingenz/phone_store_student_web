// admin.js - Xử lý dữ liệu quản trị mô phỏng bằng localStorage.
var ADMIN_SESSION_KEY = "smartmobile_admin_session";
var ADMIN_PRODUCTS_KEY = STORE_PRODUCTS_KEY;
var ADMIN_ORDERS_KEY = STORE_ORDERS_KEY;

var sampleOrders = [
    { id: "DH1005", customer: "Khách hàng mẫu 01", phone: "Không lưu số mẫu", date: "2026-08-06", total: 33990000, status: "Chờ xử lý", items: [{ name: "iPhone 17 Pro 256GB", quantity: 1 }] },
    { id: "DH1004", customer: "Khách hàng mẫu 02", phone: "Không lưu số mẫu", date: "2026-08-05", total: 5640000, status: "Đang giao", items: [{ name: "Samsung Galaxy A17", quantity: 1 }, { name: "Tai nghe Bluetooth JBL", quantity: 1 }] },
    { id: "DH1003", customer: "Khách hàng mẫu 03", phone: "Không lưu số mẫu", date: "2026-08-04", total: 13780000, status: "Hoàn thành", items: [{ name: "Citizen Eco-Drive", quantity: 2 }] },
    { id: "DH1002", customer: "Khách hàng mẫu 04", phone: "Không lưu số mẫu", date: "2026-08-03", total: 31090000, status: "Hoàn thành", items: [{ name: "Samsung Galaxy S25 Ultra", quantity: 1 }] },
    { id: "DH1001", customer: "Khách hàng mẫu 05", phone: "Không lưu số mẫu", date: "2026-08-02", total: 350000, status: "Đã hủy", items: [{ name: "Tai nghe Bluetooth JBL", quantity: 1 }] }
];

document.addEventListener("DOMContentLoaded", function () {
    seedAdminData();

    var page = document.body.dataset.page;
    if (page === "login") {
        initLoginPage();
        return;
    }

    if (!getAdminSession()) {
        window.location.replace("login.html");
        return;
    }

    showAdminName();
    bindLogout();

    if (page === "dashboard") renderDashboard();
    if (page === "orders") initOrdersPage();
    if (page === "products") initProductsPage();
});

function seedAdminData() {
    // Dữ liệu sản phẩm và migration được xử lý trong shared/store-data.js.
    ensureStoreData();
    if (localStorage.getItem(ADMIN_ORDERS_KEY) === null) {
        saveData(ADMIN_ORDERS_KEY, sampleOrders);
    }
}

function getData(key) {
    try {
        var data = JSON.parse(localStorage.getItem(key));
        return Array.isArray(data) ? data : [];
    } catch (error) {
        return [];
    }
}

function saveData(key, value) {
    localStorage.setItem(key, JSON.stringify(value));
}

function getAdminSession() {
    try {
        return JSON.parse(localStorage.getItem(ADMIN_SESSION_KEY));
    } catch (error) {
        return null;
    }
}

function initLoginPage() {
    if (getAdminSession()) {
        window.location.replace("index.html");
        return;
    }

    var form = document.getElementById("adminLoginForm");
    form.addEventListener("submit", function (event) {
        event.preventDefault();
        var username = document.getElementById("adminUsername").value.trim();
        var password = document.getElementById("adminPassword").value.trim();
        var errorBox = document.getElementById("loginError");

        if (!username || !password) {
            errorBox.textContent = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
            return;
        }

        localStorage.setItem(ADMIN_SESSION_KEY, JSON.stringify({ username: username }));
        window.location.href = "index.html";
    });
}

function showAdminName() {
    var session = getAdminSession();
    var element = document.getElementById("adminUserName");
    if (element && session) element.textContent = session.username;
}

function bindLogout() {
    var button = document.getElementById("logoutButton");
    if (!button) return;
    button.addEventListener("click", function () {
        localStorage.removeItem(ADMIN_SESSION_KEY);
        window.location.replace("login.html");
    });
}

function formatCurrency(value) {
    return Number(value).toLocaleString("vi-VN") + "đ";
}

function formatDate(value) {
    var parts = value.split("-");
    return parts.length === 3 ? parts[2] + "/" + parts[1] + "/" + parts[0] : value;
}

function escapeHtml(value) {
    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function getStatusClass(status) {
    if (status === "Chờ xử lý") return "status-pending";
    if (status === "Đang giao") return "status-shipping";
    if (status === "Hoàn thành") return "status-completed";
    return "status-cancelled";
}

function renderDashboard() {
    var products = getData(ADMIN_PRODUCTS_KEY);
    var orders = getData(ADMIN_ORDERS_KEY);
    var revenue = orders.reduce(function (sum, order) {
        return order.status === "Hoàn thành" ? sum + Number(order.total) : sum;
    }, 0);
    var pendingCount = orders.filter(function (order) { return order.status === "Chờ xử lý"; }).length;

    document.getElementById("totalRevenue").textContent = formatCurrency(revenue);
    document.getElementById("totalOrders").textContent = orders.length;
    document.getElementById("totalProducts").textContent = products.length;
    document.getElementById("pendingOrders").textContent = pendingCount;

    var recentOrders = orders.slice().sort(function (a, b) {
        return b.date.localeCompare(a.date);
    }).slice(0, 5);
    var recentBody = document.getElementById("recentOrdersBody");
    recentBody.innerHTML = recentOrders.length ? recentOrders.map(function (order) {
        return "<tr>" +
            "<td><strong>" + escapeHtml(order.id) + "</strong></td>" +
            "<td>" + escapeHtml(order.customer) + "</td>" +
            "<td>" + formatDate(order.date) + "</td>" +
            "<td>" + formatCurrency(order.total) + "</td>" +
            "<td><span class=\"status-badge " + getStatusClass(order.status) + "\">" + escapeHtml(order.status) + "</span></td>" +
            "</tr>";
    }).join("") : "<tr><td colspan=\"5\" class=\"empty-state\">Chưa có đơn hàng.</td></tr>";

    var lowStock = products.filter(function (product) { return Number(product.stock) <= 5; });
    var lowStockBox = document.getElementById("lowStockList");
    lowStockBox.innerHTML = lowStock.length ? lowStock.map(function (product) {
        return "<div class=\"low-stock-item\"><div><p>" + escapeHtml(product.name) + "</p><small>" + escapeHtml(product.category) + "</small></div><strong>" + Number(product.stock) + " sản phẩm</strong></div>";
    }).join("") : "<p class=\"empty-state\">Không có sản phẩm sắp hết.</p>";
}

function initOrdersPage() {
    var searchInput = document.getElementById("orderSearch");
    var statusFilter = document.getElementById("orderStatusFilter");
    searchInput.addEventListener("input", renderOrders);
    statusFilter.addEventListener("change", renderOrders);
    document.getElementById("orderDetailClose").addEventListener("click", function () {
        document.getElementById("orderDetailDialog").close();
    });
    renderOrders();
}

function renderOrders() {
    var keyword = document.getElementById("orderSearch").value.trim().toLowerCase();
    var selectedStatus = document.getElementById("orderStatusFilter").value;
    var orders = getData(ADMIN_ORDERS_KEY).filter(function (order) {
        var matchesKeyword = order.id.toLowerCase().includes(keyword) || order.customer.toLowerCase().includes(keyword);
        var matchesStatus = !selectedStatus || order.status === selectedStatus;
        return matchesKeyword && matchesStatus;
    });

    var tbody = document.getElementById("ordersTableBody");
    tbody.innerHTML = orders.length ? orders.map(function (order) {
        var statuses = ["Chờ xử lý", "Đang giao", "Hoàn thành", "Đã hủy"];
        var options = statuses.map(function (status) {
            return "<option value=\"" + status + "\"" + (status === order.status ? " selected" : "") + ">" + status + "</option>";
        }).join("");
        return "<tr>" +
            "<td><strong>" + escapeHtml(order.id) + "</strong></td>" +
            "<td>" + escapeHtml(order.customer) + "<br><small>" + escapeHtml(order.phone) + "</small></td>" +
            "<td>" + formatDate(order.date) + "</td>" +
            "<td>" + formatCurrency(order.total) + "</td>" +
            "<td><select class=\"status-select\" data-order-id=\"" + escapeHtml(order.id) + "\">" + options + "</select></td>" +
            "<td><button class=\"btn btn-secondary btn-small view-order-btn\" data-order-id=\"" + escapeHtml(order.id) + "\">Xem</button></td>" +
            "</tr>";
    }).join("") : "<tr><td colspan=\"6\" class=\"empty-state\">Không tìm thấy đơn hàng phù hợp.</td></tr>";

    tbody.querySelectorAll(".status-select").forEach(function (select) {
        select.addEventListener("change", function () {
            updateOrderStatus(select.dataset.orderId, select.value);
        });
    });
    tbody.querySelectorAll(".view-order-btn").forEach(function (button) {
        button.addEventListener("click", function () { openOrderDetail(button.dataset.orderId); });
    });
}

function updateOrderStatus(orderId, newStatus) {
    var orders = getData(ADMIN_ORDERS_KEY);
    var order = orders.find(function (item) { return item.id === orderId; });
    if (!order) return;
    order.status = newStatus;
    saveData(ADMIN_ORDERS_KEY, orders);
    renderOrders();
}

function openOrderDetail(orderId) {
    var order = getData(ADMIN_ORDERS_KEY).find(function (item) { return item.id === orderId; });
    if (!order) return;
    var items = order.items.map(function (item) {
        return escapeHtml(item.name) + " × " + Number(item.quantity);
    }).join("<br>");
    document.getElementById("orderDetailContent").innerHTML =
        "<div class=\"order-detail-grid\">" +
        "<p><strong>Mã đơn</strong><br>" + escapeHtml(order.id) + "</p>" +
        "<p><strong>Ngày đặt</strong><br>" + formatDate(order.date) + "</p>" +
        "<p><strong>Khách hàng</strong><br>" + escapeHtml(order.customer) + "</p>" +
        "<p><strong>Điện thoại</strong><br>" + escapeHtml(order.phone) + "</p>" +
        "<p class=\"full\"><strong>Sản phẩm</strong><br>" + items + "</p>" +
        "<p><strong>Tổng tiền</strong><br>" + formatCurrency(order.total) + "</p>" +
        "<p><strong>Trạng thái</strong><br><span class=\"status-badge " + getStatusClass(order.status) + "\">" + escapeHtml(order.status) + "</span></p>" +
        "</div>";
    document.getElementById("orderDetailDialog").showModal();
}

function initProductsPage() {
    document.getElementById("productSearch").addEventListener("input", renderProducts);
    document.getElementById("productCategoryFilter").addEventListener("change", renderProducts);
    document.getElementById("addProductButton").addEventListener("click", function () { openProductDialog(); });
    document.getElementById("productDialogClose").addEventListener("click", closeProductDialog);
    document.getElementById("cancelProductButton").addEventListener("click", closeProductDialog);
    document.getElementById("productForm").addEventListener("submit", saveProductFromForm);
    renderProducts();
}

function renderProducts() {
    var keyword = document.getElementById("productSearch").value.trim().toLowerCase();
    var category = document.getElementById("productCategoryFilter").value;
    var products = getData(ADMIN_PRODUCTS_KEY).filter(function (product) {
        return product.name.toLowerCase().includes(keyword) && (!category || product.category === category);
    });
    var tbody = document.getElementById("productsTableBody");
    tbody.innerHTML = products.length ? products.map(function (product) {
        var inStock = Number(product.stock) > 0;
        return "<tr>" +
            "<td><div class=\"product-cell\"><img class=\"product-thumb\" src=\"" + escapeHtml(product.image) + "\" alt=\"\"><strong>" + escapeHtml(product.name) + "</strong></div></td>" +
            "<td>" + escapeHtml(product.category) + "</td>" +
            "<td>" + formatCurrency(product.price) + "</td>" +
            "<td>" + Number(product.stock) + "</td>" +
            "<td><span class=\"status-badge " + (inStock ? "status-stock" : "status-out") + "\">" + (inStock ? "Còn hàng" : "Hết hàng") + "</span></td>" +
            "<td><div class=\"action-group\"><button class=\"btn btn-secondary btn-small edit-product-btn\" data-product-id=\"" + escapeHtml(product.id) + "\">Sửa</button><button class=\"btn btn-danger btn-small delete-product-btn\" data-product-id=\"" + escapeHtml(product.id) + "\">Xóa</button></div></td>" +
            "</tr>";
    }).join("") : "<tr><td colspan=\"6\" class=\"empty-state\">Không tìm thấy sản phẩm phù hợp.</td></tr>";

    tbody.querySelectorAll(".edit-product-btn").forEach(function (button) {
        button.addEventListener("click", function () { openProductDialog(button.dataset.productId); });
    });
    tbody.querySelectorAll(".delete-product-btn").forEach(function (button) {
        button.addEventListener("click", function () { deleteProduct(button.dataset.productId); });
    });
}

function openProductDialog(productId) {
    var form = document.getElementById("productForm");
    form.reset();
    document.getElementById("productFormError").textContent = "";
    document.getElementById("productId").value = "";
    document.getElementById("productDialogTitle").textContent = productId ? "Sửa sản phẩm" : "Thêm sản phẩm";

    if (productId) {
        var product = getData(ADMIN_PRODUCTS_KEY).find(function (item) { return item.id === productId; });
        if (!product) return;
        document.getElementById("productId").value = product.id;
        document.getElementById("productName").value = product.name;
        document.getElementById("productCategory").value = product.category;
        document.getElementById("productPrice").value = product.price;
        document.getElementById("productStock").value = product.stock;
        document.getElementById("productImage").value = product.image;
    }
    document.getElementById("productDialog").showModal();
}

function closeProductDialog() {
    document.getElementById("productDialog").close();
}

function saveProductFromForm(event) {
    event.preventDefault();
    var currentId = document.getElementById("productId").value;
    var name = document.getElementById("productName").value.trim();
    var category = document.getElementById("productCategory").value;
    var price = Number(document.getElementById("productPrice").value);
    var stock = Number(document.getElementById("productStock").value);
    var image = document.getElementById("productImage").value.trim();
    var errorBox = document.getElementById("productFormError");

    if (!name || !category || !Number.isFinite(price) || price <= 0 || !Number.isInteger(stock) || stock < 0 || !image) {
        errorBox.textContent = "Vui lòng nhập đủ thông tin; giá phải lớn hơn 0 và tồn kho là số nguyên không âm.";
        return;
    }

    var products = getData(ADMIN_PRODUCTS_KEY);
    if (currentId) {
        var current = products.find(function (item) { return item.id === currentId; });
        if (!current) return;
        current.name = name;
        current.category = category;
        current.price = price;
        current.stock = stock;
        current.image = image;
    } else {
        products.push({
            id: "SP" + Date.now(),
            name: name,
            category: category,
            price: price,
            stock: stock,
            image: image
        });
    }
    saveData(ADMIN_PRODUCTS_KEY, products);
    closeProductDialog();
    renderProducts();
}

function deleteProduct(productId) {
    var products = getData(ADMIN_PRODUCTS_KEY);
    var product = products.find(function (item) { return item.id === productId; });
    if (!product || !window.confirm("Bạn có chắc muốn xóa sản phẩm \"" + product.name + "\"?")) return;
    products = products.filter(function (item) { return item.id !== productId; });
    saveData(ADMIN_PRODUCTS_KEY, products);
    renderProducts();
}
