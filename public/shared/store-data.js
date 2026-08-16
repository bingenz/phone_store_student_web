// store-data.js - Dữ liệu sản phẩm dùng chung cho trang user và admin.
// File này phải được nhúng trước admin.js, admin-products.js và cart.js.

var STORE_PRODUCTS_KEY = "smartmobile_admin_products";
var STORE_ORDERS_KEY = "smartmobile_admin_orders";
var STORE_DATA_VERSION_KEY = "smartmobile_store_data_version";
var STORE_DATA_VERSION = 2;

// 16 sản phẩm đang hiển thị trên trang bán hàng.
// Những sản phẩm trước đây chưa có dữ liệu tồn kho được khởi tạo là 10.
var STORE_DEFAULT_PRODUCTS = [
    { id: "iphone17pro256", name: "iPhone 17 Pro 256GB", category: "Điện thoại", price: 33990000, stock: 8, image: "../images/cropped/iphone17pro256.png" },
    { id: "iphone17pro512", name: "iPhone 17 Pro 512GB", category: "Điện thoại", price: 39990000, stock: 10, image: "../images/cropped/iphone17pro512.png" },
    { id: "s25ultra", name: "Samsung Galaxy S25 Ultra", category: "Điện thoại", price: 31090000, stock: 4, image: "../images/cropped/s25ultra.png" },
    { id: "xiaomi17ultra", name: "Xiaomi 17 Ultra 5G", category: "Điện thoại", price: 31190000, stock: 10, image: "../images/cropped/xiaomi17ultra.png" },
    { id: "galaxy_a07", name: "Samsung Galaxy A07", category: "Điện thoại", price: 3090000, stock: 10, image: "../images/cropped/galaxy_a07.png" },
    { id: "galaxy_a17", name: "Samsung Galaxy A17", category: "Điện thoại", price: 5890000, stock: 12, image: "../images/cropped/galaxy_a17.png" },
    { id: "watch_citizen1", name: "Citizen EG7073-16Y", category: "Đồng hồ", price: 10280000, stock: 3, image: "../images/cropped/watch_citizen1.png" },
    { id: "watch_citizen2", name: "Citizen EW2700-54L", category: "Đồng hồ", price: 10302000, stock: 10, image: "../images/cropped/watch_citizen2.png" },
    { id: "watch_citizen3", name: "Citizen CA0711-80H", category: "Đồng hồ", price: 10315000, stock: 10, image: "../images/cropped/watch_citizen3.png" },
    { id: "watch_orient1", name: "Orient Star RE-AT0004S00B", category: "Đồng hồ", price: 17864000, stock: 10, image: "../images/cropped/watch_orient1.png" },
    { id: "op_lung_1", name: "Ốp lưng chống sốc", category: "Phụ kiện", price: 150000, stock: 10, image: "../images/op_lung_samsung.png" },
    { id: "op_lung_2", name: "Ốp lưng trong suốt", category: "Phụ kiện", price: 80000, stock: 10, image: "../images/op_lung_trong_suot.png" },
    { id: "tai_nghe_1", name: "Tai nghe nhét tai có dây", category: "Phụ kiện", price: 120000, stock: 10, image: "../images/tai_nghe_samsung.png" },
    { id: "tai_nghe_2", name: "Tai nghe không dây Bluetooth", category: "Phụ kiện", price: 350000, stock: 0, image: "../images/tai_nghe_jbl_den2.png" },
    { id: "cuc_sac_1", name: "Cục sạc nhanh 20W", category: "Phụ kiện", price: 200000, stock: 10, image: "../images/cuc_sac_anker.png" },
    { id: "cuc_sac_2", name: "Cục sạc không dây 15W", category: "Phụ kiện", price: 320000, stock: 10, image: "../images/cuc_sac_anker_zolo.png" }
];

function readStoreArray(key) {
    try {
        var data = JSON.parse(localStorage.getItem(key));
        return Array.isArray(data) ? data : [];
    } catch (error) {
        return [];
    }
}

function getStoreProducts() {
    return readStoreArray(STORE_PRODUCTS_KEY);
}

function saveStoreProducts(products) {
    localStorage.setItem(STORE_PRODUCTS_KEY, JSON.stringify(products));
}

function getStoreOrders() {
    return readStoreArray(STORE_ORDERS_KEY);
}

function saveStoreOrders(orders) {
    localStorage.setItem(STORE_ORDERS_KEY, JSON.stringify(orders));
}

function getStoreProductById(productId) {
    return getStoreProducts().find(function (product) {
        return String(product.id) === String(productId);
    }) || null;
}

function formatStorePrice(value) {
    return Number(value).toLocaleString("vi-VN") + "đ";
}

function copyDefaultProduct(product) {
    return {
        id: product.id,
        name: product.name,
        category: product.category,
        price: product.price,
        stock: product.stock,
        image: product.image
    };
}

function ensureStoreData() {
    var rawProducts = localStorage.getItem(STORE_PRODUCTS_KEY);
    var currentVersion = Number(localStorage.getItem(STORE_DATA_VERSION_KEY)) || 0;

    // Trình duyệt mới: tạo đầy đủ danh mục ngay cả khi chưa từng mở trang admin.
    if (rawProducts === null) {
        saveStoreProducts(STORE_DEFAULT_PRODUCTS.map(copyDefaultProduct));
        localStorage.setItem(STORE_DATA_VERSION_KEY, String(STORE_DATA_VERSION));
        return;
    }

    // Migration một lần: chỉ thêm ID còn thiếu, không ghi đè dữ liệu admin đã sửa.
    if (currentVersion < STORE_DATA_VERSION) {
        var products = getStoreProducts();
        var existingIds = products.map(function (product) {
            return String(product.id);
        });

        STORE_DEFAULT_PRODUCTS.forEach(function (defaultProduct) {
            if (!existingIds.includes(defaultProduct.id)) {
                products.push(copyDefaultProduct(defaultProduct));
            }
        });

        saveStoreProducts(products);
        localStorage.setItem(STORE_DATA_VERSION_KEY, String(STORE_DATA_VERSION));
    }
}

ensureStoreData();
