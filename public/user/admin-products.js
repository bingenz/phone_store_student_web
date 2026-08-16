// Đồng bộ dữ liệu sản phẩm từ localStorage lên trang bán hàng.

function escapeProductText(value) {
    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function getProductGrid(category) {
    var sectionId = "offers";
    if (category === "Điện thoại") sectionId = "phones";
    if (category === "Đồng hồ") sectionId = "watches";
    return document.querySelector("#" + sectionId + " .product-grid");
}

function setProductButtonState(button, product) {
    var isInStock = Number(product.stock) > 0;
    button.disabled = !isInStock;
    button.setAttribute("aria-disabled", String(!isInStock));
    button.textContent = isInStock ? "Thêm vào giỏ" : "Hết hàng";
}

function updateStaticProductCard(card, product) {
    var image = card.querySelector("img");
    var title = card.querySelector("h3");
    var price = card.querySelector(".new-price");
    var oldPrice = card.querySelector(".old-price");
    var button = card.querySelector(".add-cart-btn[data-id]");
    var detailLink = card.querySelector(".detail-link");
    var stockLine = card.querySelector(".stock-status");
    var currentPrice = formatStorePrice(product.price);

    if (!card.dataset.originalPrice && button) {
        card.dataset.originalPrice = button.dataset.price;
    }

    card.hidden = false;
    if (image) {
        image.src = product.image;
        image.alt = product.name;
    }
    if (title) title.textContent = product.name;
    if (price) price.textContent = currentPrice;
    if (oldPrice) oldPrice.hidden = card.dataset.originalPrice !== currentPrice;
    if (detailLink) detailLink.href = "product-detail.html?id=" + encodeURIComponent(product.id);

    if (!stockLine) {
        stockLine = document.createElement("p");
        stockLine.className = "stock-status";
        var priceBox = card.querySelector(".price-box");
        if (priceBox) card.insertBefore(stockLine, priceBox);
    }
    stockLine.textContent = Number(product.stock) > 0 ? "Còn " + Number(product.stock) + " sản phẩm" : "Hết hàng";
    stockLine.classList.toggle("out-of-stock", Number(product.stock) <= 0);

    if (button) {
        button.dataset.id = product.id;
        button.dataset.name = product.name;
        button.dataset.price = currentPrice;
        button.dataset.image = product.image;
        setProductButtonState(button, product);
    }

    var targetGrid = getProductGrid(product.category);
    if (targetGrid && card.parentElement !== targetGrid) targetGrid.appendChild(card);
}

function createAdminProductCard(product) {
    var isInStock = Number(product.stock) > 0;
    var safeId = escapeProductText(product.id);
    var safeName = escapeProductText(product.name);
    var safeImage = escapeProductText(product.image);
    var price = formatStorePrice(product.price);
    var card = document.createElement("article");

    card.className = "product-card admin-added-product";
    card.innerHTML =
        '<img src="' + safeImage + '" alt="' + safeName + '">' +
        '<h3>' + safeName + '</h3>' +
        '<p class="spec">' + escapeProductText(product.category) + '</p>' +
        '<p class="desc">Sản phẩm mới được cập nhật từ trang quản trị.</p>' +
        '<p class="stock-status' + (isInStock ? "" : " out-of-stock") + '">' + (isInStock ? "Còn " + Number(product.stock) + " sản phẩm" : "Hết hàng") + '</p>' +
        '<div class="price-box"><span class="new-price">' + price + '</span></div>' +
        '<div class="product-actions">' +
            '<a href="product-detail.html?id=' + encodeURIComponent(product.id) + '" target="_self" class="detail-link">Xem chi tiết</a>' +
            '<button class="add-cart-btn" data-id="' + safeId + '" data-name="' + safeName + '" data-price="' + price + '" data-image="' + safeImage + '"' + (isInStock ? "" : " disabled aria-disabled=\"true\"") + '>' + (isInStock ? "Thêm vào giỏ" : "Hết hàng") + '</button>' +
        '</div>';

    return card;
}

function updateHeroProduct(products) {
    var heroLink = document.querySelector(".hero-product-link");
    if (!heroLink) return;

    var featured = products.find(function (product) {
        return String(product.id) === "iphone17pro256" && Number(product.stock) > 0;
    }) || products.find(function (product) {
        return product.category === "Điện thoại" && Number(product.stock) > 0;
    });

    if (!featured) {
        heroLink.hidden = true;
        return;
    }

    heroLink.hidden = false;
    heroLink.href = "product-detail.html?id=" + encodeURIComponent(featured.id);
    heroLink.setAttribute("aria-label", "Xem chi tiết " + featured.name);
    heroLink.querySelector("img").src = featured.image;
    heroLink.querySelector("img").alt = featured.name;
    heroLink.querySelector("h3").textContent = featured.name;
    heroLink.querySelector("p").textContent = "Sản phẩm nổi bật - còn " + Number(featured.stock) + " sản phẩm";
    heroLink.querySelector("strong").textContent = formatStorePrice(featured.price);
}

function renderStoreProducts() {
    document.querySelectorAll(".admin-added-product").forEach(function (card) {
        card.remove();
    });

    var products = getStoreProducts();
    var productMap = {};
    products.forEach(function (product) {
        productMap[String(product.id)] = product;
    });

    updateHeroProduct(products);

    var staticIds = [];
    document.querySelectorAll(".product-card").forEach(function (card) {
        var button = card.querySelector(".add-cart-btn[data-id]");
        if (!button || card.classList.contains("admin-added-product")) return;

        var productId = String(button.dataset.id);
        staticIds.push(productId);
        if (productMap[productId]) {
            updateStaticProductCard(card, productMap[productId]);
        } else {
            // Admin đã xóa sản phẩm: không tiếp tục bán thẻ sản phẩm tĩnh đó.
            card.hidden = true;
        }
    });

    products.forEach(function (product) {
        if (staticIds.includes(String(product.id))) return;
        var grid = getProductGrid(product.category);
        if (grid) grid.appendChild(createAdminProductCard(product));
    });
}

renderStoreProducts();

window.addEventListener("storage", function (event) {
    if (event.key === STORE_PRODUCTS_KEY) renderStoreProducts();
});
