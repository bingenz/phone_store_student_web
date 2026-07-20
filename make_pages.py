# encoding: utf-8

# ===== SHARED PARTS =====
def header():
    return """    <header class="header">
        <div class="container nav-bar">
            <a href="index.html#home" target="_self" class="logo-box">
                <img src="images/logo-phone.svg" alt="Logo SmartMobile">
                <div>
                    <h1>SmartMobile</h1>
                    <p>\u0110i\u1ec7n tho\u1ea1i ch\u00ednh h\u00e3ng</p>
                </div>
            </a>
            <nav class="menu">
                <a href="index.html#home" target="_self">Trang ch\u1ee7</a>
                <a href="index.html#phones" target="_self">\u0110i\u1ec7n tho\u1ea1i</a>
                <a href="index.html#watches" target="_self">\u0110\u1ed3ng h\u1ed3</a>
                <a href="about.html" target="_self">Gi\u1edbi thi\u1ec7u</a>
                <a href="news.html" target="_self">Tin t\u1ee9c</a>
                <a href="contact.html" target="_self">Li\u00ean h\u1ec7</a>
                <a href="login.html" class="btn-login-header" target="_self">Đăng nhập</a>
            </nav>
            <button class="hamburger-btn" id="hamburgerBtn" onclick="toggleMenu()">&#9776;</button>
        </div>
        <nav class="menu-mobile" id="menuMobile">
            <a href="index.html#home" target="_self">Trang ch\u1ee7</a>
            <a href="index.html#phones" target="_self">\u0110i\u1ec7n tho\u1ea1i</a>
            <a href="index.html#watches" target="_self">\u0110\u1ed3ng h\u1ed3</a>
            <a href="about.html" target="_self">Gi\u1edbi thi\u1ec7u</a>
            <a href="news.html" target="_self">Tin t\u1ee9c</a>
            <a href="contact.html" target="_self">Li\u00ean h\u1ec7</a>
            <a href="login.html" class="btn-login-header" target="_self">Đăng nhập</a>
        </nav>
    </header>"""

def footer():
    return """    <footer id="contact-footer" class="footer">
        <div class="container footer-grid">
            <div class="footer-info">
                <h2>SmartMobile Store</h2>
                <p><strong>\u0110\u1ecba ch\u1ec9:</strong> 300A Nguy\u1ec5n T\u1ea5t Th\u00e0nh, Qu\u1eadn 4, TP. H\u1ed3 Ch\u00ed Minh</p>
                <p><strong>Email:</strong> 1235@gmail.com</p>
                <p><strong>\u0110i\u1ec7n tho\u1ea1i:</strong> 1234 567 891</p>
                <p><strong>Gi\u1edd m\u1edf c\u1eeda:</strong> 8:00 - 21:00 m\u1ed7i ng\u00e0y</p>
            </div>
            <div class="footer-map">
                <iframe src="https://www.google.com/maps?q=300A%20Nguyen%20Tat%20Thanh%2C%20Quan%204%2C%20Ho%20Chi%20Minh%20City&amp;output=embed" loading="lazy" referrerpolicy="no-referrer-when-downgrade" title="B\u1ea3n \u0111\u1ed3 SmartMobile"></iframe>
            </div>
        </div>
    </footer>"""

def scripts(extra=""):
    return """    <a href="cart.html" class="cart-floating-btn" id="cartFloatingBtn">
        &#128722;
        <span class="cart-badge" id="cartBadge"></span>
    </a>
    <script src="cart.js"></script>
    <script src="auth.js"></script>
    <script>
        function toggleMenu() {
            document.getElementById('menuMobile').classList.toggle('open');
        }
    </script>""" + extra

# ===== ABOUT.HTML =====
about = """<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Gi\u1edbi thi\u1ec7u v\u1ec1 SmartMobile - c\u1eeda h\u00e0ng \u0111i\u1ec7n tho\u1ea1i ch\u00ednh h\u00e3ng, uy t\u00edn t\u1ea1i TP. H\u1ed3 Ch\u00ed Minh.">
    <title>Gi\u1edbi thi\u1ec7u - SmartMobile</title>
    <link rel="stylesheet" href="style.css">
    <link rel="shortcut icon" href="images/logo-phone.svg">
</head>
<body>

""" + header() + """

    <!-- N\u1ed9i dung ch\u00ednh trang Gi\u1edbi thi\u1ec7u -->
    <main class="about-section">
        <div class="container">

            <div class="section-title">
                <h2>Gi\u1edbi thi\u1ec7u SmartMobile</h2>
                <p>\u0110\u1ed1i t\u00e1c tin c\u1eady c\u1ee7a b\u1ea1n trong th\u1ebf gi\u1edbi c\u00f4ng ngh\u1ec7</p>
            </div>

            <!-- \u0110o\u1ea1n gi\u1edbi thi\u1ec7u ch\u00ednh: d\u00f9ng class .about-intro \u0111\u1ecbnh ngh\u0129a trong style.css -->
            <div class="about-intro">
                <h3>Ch\u00fang t\u00f4i l\u00e0 ai?</h3>
                <p>
                    SmartMobile l\u00e0 c\u1eeda h\u00e0ng \u0111i\u1ec7n tho\u1ea1i v\u00e0 \u0111\u1ed3ng h\u1ed3 ch\u00ednh h\u00e3ng \u0111\u01b0\u1ee3c th\u00e0nh l\u1eadp
                    v\u1edbi m\u1ee5c ti\u00eau mang \u0111\u1ebfn cho kh\u00e1ch h\u00e0ng nh\u1eefng s\u1ea3n ph\u1ea9m c\u00f4ng ngh\u1ec7 ch\u1ea5t l\u01b0\u1ee3ng cao
                    v\u1edbi m\u1ee9c gi\u00e1 h\u1ee3p l\u00fd v\u00e0 d\u1ecbch v\u1ee5 h\u1eadu m\u00e3i t\u1ed1t nh\u1ea5t.
                </p>
                <p>
                    Ch\u00fang t\u00f4i hi\u1ec3u r\u1eb1ng m\u1ed7i ng\u01b0\u1eddi c\u00f3 nhu c\u1ea7u v\u00e0 ng\u00e2n s\u00e1ch kh\u00e1c nhau,
                    v\u00ec v\u1eady SmartMobile lu\u00f4n c\u1ed1 g\u1eafng \u0111a d\u1ea1ng h\u00f3a danh m\u1ee5c s\u1ea3n ph\u1ea9m \u2014
                    t\u1eeb \u0111i\u1ec7n tho\u1ea1i cao c\u1ea5p nh\u01b0 iPhone, Samsung Galaxy Ultra
                    cho \u0111\u1ebfn c\u00e1c d\u00f2ng t\u1ea7m trung ph\u00f9 h\u1ee3p v\u1edbi h\u1ecdc sinh, sinh vi\u00ean.
                </p>
            </div>

            <!-- \u0110i\u1ec3m m\u1ea1nh c\u1ee7a c\u1eeda h\u00e0ng: d\u00f9ng ul/li \u0111\u1ec3 d\u1ec5 \u0111\u1ecdc theo AGENTS.md -->
            <div class="about-intro">
                <h3>T\u1ea1i sao ch\u1ecdn SmartMobile?</h3>
                <ul class="about-highlight-list">
                    <li>S\u1ea3n ph\u1ea9m ch\u00ednh h\u00e3ng 100% \u2014 c\u00f3 h\u00f3a \u0111\u01a1n, tem b\u1ea3o h\u00e0nh r\u00f5 r\u00e0ng</li>
                    <li>B\u1ea3o h\u00e0nh 12 th\u00e1ng t\u1ea1i c\u1eeda h\u00e0ng, h\u1ed7 tr\u1ee3 \u0111\u1ed5i tr\u1ea3 trong 7 ng\u00e0y</li>
                    <li>Gi\u00e1 ni\u00eam y\u1ebft minh b\u1ea1ch, kh\u00f4ng ph\u1ee5 thu ph\u00ed \u1ea9n</li>
                    <li>H\u1ed7 tr\u1ee3 tr\u1ea3 g\u00f3p 0% l\u00e3i su\u1ea5t cho nhi\u1ec1u d\u00f2ng s\u1ea3n ph\u1ea9m</li>
                    <li>\u0110\u1ed9i ng\u0169 t\u01b0 v\u1ea5n nhi\u1ec7t t\u00ecnh, gi\u1ea3i \u0111\u00e1p tr\u01b0\u1edbc v\u00e0 sau khi mua</li>
                </ul>
            </div>

            <!-- \u0110\u1ed9i ng\u0169 -->
            <div class="section-title" style="margin-top: 36px;">
                <h2>\u0110\u1ed9i ng\u0169 c\u1ee7a ch\u00fang t\u00f4i</h2>
                <p>Nh\u1eefng ng\u01b0\u1eddi lu\u00f4n s\u1eb5n s\u00e0ng h\u1ed7 tr\u1ee3 b\u1ea1n</p>
            </div>

            <!-- 3 th\u1ebb th\u00e0nh vi\u00ean: d\u00f9ng emoji \u0111\u1ea1i di\u1ec7n \u0111\u01a1n gi\u1ea3n -->
            <div class="team-list">
                <div class="team-card">
                    <div class="team-avatar">&#128188;</div>
                    <h4>L\u00ea Ng\u1ecdc Thu\u1eadn</h4>
                    <p>Qu\u1ea3n l\u00fd c\u1eeda h\u00e0ng</p>
                    <p>5 n\u0103m kinh nghi\u1ec7m trong ng\u00e0nh b\u00e1n l\u1ebb \u0111i\u1ec7n tho\u1ea1i</p>
                </div>
                <div class="team-card">
                    <div class="team-avatar">&#128101;</div>
                    <h4>Ph\u1ea1m Nguy\u1ec5n Ho\u00e0ng Ti\u1ebfn</h4>
                    <p>T\u01b0 v\u1ea5n vi\u00ean</p>
                    <p>Chuy\u00ean v\u1ec1 \u0111i\u1ec7n tho\u1ea1i cao c\u1ea5p v\u00e0 \u0111\u1ed3ng h\u1ed3 th\u1eddi trang</p>
                </div>
                <div class="team-card">
                    <div class="team-avatar">&#128295;</div>
                    <h4>Nguy\u1ec5n Minh T\u00e2n</h4>
                    <p>K\u1ef9 thu\u1eadt vi\u00ean</p>
                    <p>Ph\u1ee5 tr\u00e1ch ki\u1ec3m tra v\u00e0 b\u1ea3o h\u00e0nh s\u1ea3n ph\u1ea9m</p>
                </div>
            </div>

        </div>
    </main>

""" + footer() + "\n\n" + scripts() + """
</body>
</html>"""

# ===== NEWS.HTML =====
news = """<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Tin t\u1ee9c v\u00e0 khuy\u1ebfn m\u00e3i m\u1edbi nh\u1ea5t t\u1eeb SmartMobile.">
    <title>Tin t\u1ee9c - SmartMobile</title>
    <link rel="stylesheet" href="style.css">
    <link rel="shortcut icon" href="images/logo-phone.svg">
</head>
<body>

""" + header() + """

    <!-- N\u1ed9i dung ch\u00ednh trang Tin t\u1ee9c -->
    <main class="news-section">
        <div class="container">

            <div class="section-title">
                <h2>Tin t\u1ee9c &amp; Khuy\u1ebfn m\u00e3i</h2>
                <p>C\u1eadp nh\u1eadt th\u00f4ng tin m\u1edbi nh\u1ea5t t\u1eeb SmartMobile</p>
            </div>

            <!-- L\u01b0\u1edbi b\u00e0i vi\u1ebft: 2 c\u1ed9t tr\u00ean desktop, 1 c\u1ed9t tr\u00ean mobile (CSS x\u1eed l\u00fd) -->
            <div class="news-grid">

                <!-- B\u00e0i 1: Khuy\u1ebfn m\u00e3i -->
                <article class="news-card">
                    <span class="news-tag">Khuy\u1ebfn m\u00e3i</span>
                    <p class="news-date">15/07/2026</p>
                    <h3>Gi\u1ea3m \u0111\u1ebfn 20% cho iPhone 17 Pro trong th\u00e1ng 7</h3>
                    <p class="news-excerpt">
                        Nh\u00e2n d\u1ecbp k\u1ef7 ni\u1ec7m ng\u00e0y th\u00e0nh l\u1eadp, SmartMobile gi\u1ea3m gi\u00e1 \u0111\u1ebfn 20%
                        cho to\u00e0n b\u1ed9 d\u00f2ng iPhone 17 Pro. \u0110\u00e2y l\u00e0 c\u01a1 h\u1ed9i tuy\u1ec7t v\u1eddi \u0111\u1ec3 s\u1edf h\u1eefu
                        chi\u1ebfc \u0111i\u1ec7n tho\u1ea1i cao c\u1ea5p v\u1edbi gi\u00e1 t\u1ed1t nh\u1ea5t trong n\u0103m.
                        Ch\u01b0\u01a1ng tr\u00ecnh \u00e1p d\u1ee5ng \u0111\u1ebfn h\u1ebft ng\u00e0y 31/07/2026.
                    </p>
                </article>

                <!-- B\u00e0i 2: S\u1ef1 ki\u1ec7n -->
                <article class="news-card">
                    <span class="news-tag">S\u1ef1 ki\u1ec7n</span>
                    <p class="news-date">10/07/2026</p>
                    <h3>SmartMobile m\u1edf r\u1ed9ng showroom t\u1ea1i Qu\u1eadn 7</h3>
                    <p class="news-excerpt">
                        Ch\u00fang t\u00f4i vui m\u1eebng th\u00f4ng b\u00e1o khai tr\u01b0\u01a1ng chi nh\u00e1nh m\u1edbi
                        t\u1ea1i Qu\u1eadn 7, TP. H\u1ed3 Ch\u00ed Minh. Chi nh\u00e1nh m\u1edbi s\u1ebd ph\u1ee5c v\u1ee5 kh\u00e1ch h\u00e0ng
                        t\u1eeb 8:00 \u2013 21:00 h\u00e0ng ng\u00e0y v\u1edbi \u0111\u1ea7y \u0111\u1ee7 c\u00e1c d\u00f2ng s\u1ea3n ph\u1ea9m
                        \u0111i\u1ec7n tho\u1ea1i v\u00e0 \u0111\u1ed3ng h\u1ed3 ch\u00ednh h\u00e3ng.
                    </p>
                </article>

                <!-- B\u00e0i 3: C\u00f4ng ngh\u1ec7 -->
                <article class="news-card">
                    <span class="news-tag">C\u00f4ng ngh\u1ec7</span>
                    <p class="news-date">05/07/2026</p>
                    <h3>Samsung Galaxy A17 \u2014 L\u1ef1a ch\u1ecdn t\u1ea7m trung \u0111\u00e1ng mua nh\u1ea5t 2026</h3>
                    <p class="news-excerpt">
                        Samsung Galaxy A17 v\u1eeba ra m\u1eaft v\u1edbi nhi\u1ec1u n\u00e2ng c\u1ea5p \u1ea5n t\u01b0\u1ee3ng:
                        chip m\u1ea1nh h\u01a1n, pin 5000mAh v\u00e0 m\u00e0n h\u00ecnh 90Hz.
                        \u0110\u00e2y l\u00e0 l\u1ef1a ch\u1ecdn l\u00fd t\u01b0\u1edfng cho h\u1ecdc sinh, sinh vi\u00ean mu\u1ed1n
                        m\u1ed9t chi\u1ebfc \u0111i\u1ec7n tho\u1ea1i b\u1ec1n, l\u00e2u d\u00e0i v\u1edbi ng\u00e2n s\u00e1ch v\u1eeba ph\u1ea3i.
                    </p>
                </article>

                <!-- B\u00e0i 4: M\u1eb9o d\u00f9ng -->
                <article class="news-card">
                    <span class="news-tag">M\u1eb9o d\u00f9ng</span>
                    <p class="news-date">01/07/2026</p>
                    <h3>5 c\u00e1ch b\u1ea3o v\u1ec7 \u0111i\u1ec7n tho\u1ea1i gi\u00fap t\u0103ng tu\u1ed5i th\u1ecd pin</h3>
                    <p class="news-excerpt">
                        Pin l\u00e0 b\u1ed9 ph\u1eadn quan tr\u1ecdng nh\u1ea5t nh\u01b0ng l\u1ea1i d\u1ec5 h\u1ecfng nh\u1ea5t n\u1ebfu s\u1eed d\u1ee5ng sai c\u00e1ch.
                        B\u00e0i vi\u1ebft n\u00e0y t\u1ed5ng h\u1ee3p 5 th\u00f3i quen \u0111\u01a1n gi\u1ea3n gi\u00fap b\u1ea1n b\u1ea3o v\u1ec7 pin \u0111i\u1ec7n tho\u1ea1i,
                        gi\u1eef cho m\u00e1y lu\u00f4n ho\u1ea1t \u0111\u1ed9ng t\u1ed1t trong nhi\u1ec1u n\u0103m.
                    </p>
                </article>

            </div>
        </div>
    </main>

""" + footer() + "\n\n" + scripts() + """
</body>
</html>"""

# ===== CONTACT.HTML =====
contact = """<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Li\u00ean h\u1ec7 v\u1edbi SmartMobile \u2014 \u0111\u1ecba ch\u1ec9, email, s\u1ed1 \u0111i\u1ec7n tho\u1ea1i v\u00e0 form g\u1eedi tin nh\u1eafn.">
    <title>Li\u00ean h\u1ec7 - SmartMobile</title>
    <link rel="stylesheet" href="style.css">
    <link rel="shortcut icon" href="images/logo-phone.svg">
</head>
<body>

""" + header() + """

    <!-- N\u1ed9i dung ch\u00ednh trang Li\u00ean h\u1ec7 -->
    <main class="contact-page-section">
        <div class="container">

            <div class="section-title">
                <h2>Li\u00ean h\u1ec7 v\u1edbi ch\u00fang t\u00f4i</h2>
                <p>Ch\u00fang t\u00f4i lu\u00f4n s\u1eb5n s\u00e0ng l\u1eafng nghe v\u00e0 h\u1ed7 tr\u1ee3 b\u1ea1n</p>
            </div>

            <!-- Layout 2 c\u1ed9t: th\u00f4ng tin (tr\u00e1i) | form (ph\u1ea3i) -->
            <div class="contact-grid">

                <!-- C\u1ed8T TR\u00c1I: Th\u00f4ng tin li\u00ean h\u1ec7 -->
                <div class="contact-info-box">
                    <h3>Th\u00f4ng tin c\u1eeda h\u00e0ng</h3>

                    <!-- M\u1ed7i d\u00f2ng d\u00f9ng class .contact-info-item g\u1ed3m icon + n\u1ed9i dung -->
                    <div class="contact-info-item">
                        <span class="icon">&#128205;</span>
                        <span>300A Nguy\u1ec5n T\u1ea5t Th\u00e0nh, Qu\u1eadn 4, TP. H\u1ed3 Ch\u00ed Minh</span>
                    </div>
                    <div class="contact-info-item">
                        <span class="icon">&#128231;</span>
                        <span>1235@gmail.com</span>
                    </div>
                    <div class="contact-info-item">
                        <span class="icon">&#128222;</span>
                        <span>1234 567 891</span>
                    </div>
                    <div class="contact-info-item">
                        <span class="icon">&#128336;</span>
                        <span>M\u1edf c\u1eeda: 8:00 \u2013 21:00, t\u1ea5t c\u1ea3 c\u00e1c ng\u00e0y trong tu\u1ea7n</span>
                    </div>

                    <!-- B\u1ea3n \u0111\u1ed3 nh\u1ecf (iframe copy t\u1eeb footer index.html) -->
                    <div class="contact-map">
                        <iframe
                            src="https://www.google.com/maps?q=300A%20Nguyen%20Tat%20Thanh%2C%20Quan%204%2C%20Ho%20Chi%20Minh%20City&amp;output=embed"
                            loading="lazy"
                            referrerpolicy="no-referrer-when-downgrade"
                            title="B\u1ea3n \u0111\u1ed3 SmartMobile">
                        </iframe>
                    </div>
                </div>

                <!-- C\u1ed8T PH\u1ea2I: Form li\u00ean h\u1ec7 -->
                <div class="contact-form-box">
                    <h3>G\u1edfi tin nh\u1eafn cho ch\u00fang t\u00f4i</h3>

                    <!--
                        Form HTML c\u01a1 b\u1ea3n \u2014 d\u00f9ng \u0111\u1ec3 demo, ch\u01b0a g\u1eedi th\u1eadt.
                        action="#": form kh\u00f4ng g\u1eedi \u0111i \u0111\u00e2u.
                        onsubmit: hi\u1ec7n th\u00f4ng b\u00e1o v\u00e0 ng\u0103n submit m\u1eb7c \u0111\u1ecbnh.
                    -->
                    <form action="#" id="contactForm" onsubmit="handleSubmit(event)">

                        <!-- M\u1ed7i nh\u00f3m: label + input trong .form-group -->
                        <div class="form-group">
                            <label for="contactName">H\u1ecd v\u00e0 t\u00ean</label>
                            <input type="text" id="contactName" class="form-input"
                                   placeholder="Nh\u1eadp h\u1ecd v\u00e0 t\u00ean c\u1ee7a b\u1ea1n" required>
                        </div>

                        <div class="form-group">
                            <label for="contactEmail">Email</label>
                            <input type="email" id="contactEmail" class="form-input"
                                   placeholder="Nh\u1eadp email c\u1ee7a b\u1ea1n" required>
                        </div>

                        <div class="form-group">
                            <label for="contactPhone">S\u1ed1 \u0111i\u1ec7n tho\u1ea1i</label>
                            <input type="tel" id="contactPhone" class="form-input"
                                   placeholder="Nh\u1eadp s\u1ed1 \u0111i\u1ec7n tho\u1ea1i (t\u00f9y ch\u1ecdn)">
                        </div>

                        <div class="form-group">
                            <label for="contactMessage">N\u1ed9i dung</label>
                            <!--
                                textarea: h\u1ed9p nh\u1eadp nhi\u1ec1u d\u00f2ng.
                                resize: vertical (CSS) \u2014 ch\u1ec9 cho ph\u00e9p k\u00e9o d\u1ecdc.
                            -->
                            <textarea id="contactMessage" class="form-textarea"
                                      placeholder="Nh\u1eadp n\u1ed9i dung c\u1ea7n t\u01b0 v\u1ea5n..." required></textarea>
                        </div>

                        <button type="submit" class="form-submit-btn" id="submitBtn">
                            G\u1eedi tin nh\u1eafn
                        </button>

                    </form>
                </div>

            </div>
        </div>
    </main>

""" + footer() + "\n\n" + scripts("""
    <!-- Script x\u1eed l\u00fd form li\u00ean h\u1ec7 -->
    <script>
        // handleSubmit: ch\u1ea1y khi ng\u01b0\u1eddi d\u00f9ng b\u1ea5m n\u00fat G\u1eedi
        // event.preventDefault(): ng\u0103n form g\u1eedi th\u1eadt (ch\u1ec9 demo)
        function handleSubmit(event) {
            event.preventDefault();
            var name = document.getElementById('contactName').value;
            alert('C\u1ea3m \u01a1n ' + name + '! Tin nh\u1eafn c\u1ee7a b\u1ea1n \u0111\u00e3 \u0111\u01b0\u1ee3c ghi nh\u1eadn. Ch\u00fang t\u00f4i s\u1ebd li\u00ean h\u1ec7 s\u1edbm!');
            document.getElementById('contactForm').reset();
        }
    </script>""") + """
</body>
</html>"""

# ===== ACCESSORIES.HTML =====
accessories = """<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Ph\u1ee5 ki\u1ec7n ch\u00ednh h\u00e3ng t\u1ea1i SmartMobile: \u1ed1p l\u01b0ng, tai nghe, c\u1ee7c s\u1ea1c v\u00e0 nhi\u1ec1u h\u01a1n n\u1eefa.">
    <title>Ph\u1ee5 ki\u1ec7n - SmartMobile</title>
    <link rel="stylesheet" href="style.css">
    <link rel="shortcut icon" href="images/logo-phone.svg">
</head>
<body>

""" + header() + """

    <main>
        <section class="product-section">
            <div class="container">

                <div class="section-title">
                    <h2>Ph\u1ee5 ki\u1ec7n ch\u00ednh h\u00e3ng</h2>
                    <p>\u01acu \u0111\u00e3i gi\u1ea3m gi\u00e1 khi mua k\u00e8m \u0111i\u1ec7n tho\u1ea1i</p>
                </div>

                <!-- L\u01b0u \u00fd: \u1ea3nh s\u1ea3n ph\u1ea9m s\u1ebd \u0111\u01b0\u1ee3c b\u1ed5 sung sau. Hi\u1ec7n t\u1ea1i d\u00f9ng placeholder. -->
                <div class="product-grid">

                    <!-- \u1ed4P L\u01afNG 1 -->
                    <article class="product-card">
                        <!-- \u1ea2nh s\u1ebd \u0111\u01b0\u1ee3c thay th\u1ebf b\u1eb1ng \u0111\u01b0\u1edcng d\u1eabn \u1ea3nh th\u1eadt sau -->
                        <img src="images/placeholder.png" alt="\u1ed4p l\u01b0ng \u0111i\u1ec7n tho\u1ea1i" style="background:#f1f5f9;height:210px;object-fit:contain;">
                        <h3>\u1ed4p l\u01b0ng ch\u1ed1ng s\u1ed1c</h3>
                        <p class="spec">T\u01b0\u01a1ng th\u00edch nhi\u1ec1u d\u00f2ng m\u00e1y</p>
                        <p class="desc">B\u1ea3o v\u1ec7 m\u00e1y kh\u1ecfi va \u0111\u1eadp, ch\u1ea5t li\u1ec7u silicon cao c\u1ea5p, m\u1ec1m m\u1ea1i.</p>
                        <div class="price-box">
                            <span class="new-price">150.000\u0111</span>
                            <span class="old-price">200.000\u0111</span>
                        </div>
                        <div class="product-actions">
                            <span class="detail-link" style="display:flex;align-items:center;justify-content:center;">S\u1eafp c\u00f3 \u1ea3nh</span>
                            <button class="add-cart-btn"
                                data-id="op_lung_1"
                                data-name="\u1ed4p l\u01b0ng ch\u1ed1ng s\u1ed1c"
                                data-price="150.000\u0111"
                                data-image="images/placeholder.png">Th\u00eam v\u00e0o gi\u1ecf</button>
                        </div>
                    </article>

                    <!-- \u1ed4P L\u01afNG 2 -->
                    <article class="product-card">
                        <img src="images/placeholder.png" alt="\u1ed4p l\u01b0ng trong su\u1ed1t" style="background:#f1f5f9;height:210px;object-fit:contain;">
                        <h3>\u1ed4p l\u01b0ng trong su\u1ed1t</h3>
                        <p class="spec">TPU m\u1ec1m, ch\u1ed1ng \u0111\u1ed1i v\u00e0ng</p>
                        <p class="desc">Gi\u1eef nguy\u00ean v\u1ebb \u0111\u1eb9p g\u1ed1c c\u1ee7a m\u00e1y, ch\u1ed1ng tr\u00e0y x\u01b0\u1edbc hi\u1ec7u qu\u1ea3.</p>
                        <div class="price-box">
                            <span class="new-price">80.000\u0111</span>
                            <span class="old-price">120.000\u0111</span>
                        </div>
                        <div class="product-actions">
                            <span class="detail-link" style="display:flex;align-items:center;justify-content:center;">S\u1eafp c\u00f3 \u1ea3nh</span>
                            <button class="add-cart-btn"
                                data-id="op_lung_2"
                                data-name="\u1ed4p l\u01b0ng trong su\u1ed1t"
                                data-price="80.000\u0111"
                                data-image="images/placeholder.png">Th\u00eam v\u00e0o gi\u1ecf</button>
                        </div>
                    </article>

                    <!-- TAI NGHE 1 -->
                    <article class="product-card">
                        <img src="images/placeholder.png" alt="Tai nghe c\u00f3 d\u00e2y" style="background:#f1f5f9;height:210px;object-fit:contain;">
                        <h3>Tai nghe nh\u00e9t tai c\u00f3 d\u00e2y</h3>
                        <p class="spec">3.5mm • Jack c\u1eafm\u0000ph\u1ed5 th\u00f4ng</p>
                        <p class="desc">\u00c2m thanh r\u00f5 r\u00e0ng, thi\u1ebft k\u1ebf nh\u1eb9, ph\u00f9 h\u1ee3p d\u00f9ng h\u00e0ng ng\u00e0y.</p>
                        <div class="price-box">
                            <span class="new-price">120.000\u0111</span>
                            <span class="old-price">180.000\u0111</span>
                        </div>
                        <div class="product-actions">
                            <span class="detail-link" style="display:flex;align-items:center;justify-content:center;">S\u1eafp c\u00f3 \u1ea3nh</span>
                            <button class="add-cart-btn"
                                data-id="tai_nghe_1"
                                data-name="Tai nghe nh\u00e9t tai c\u00f3 d\u00e2y"
                                data-price="120.000\u0111"
                                data-image="images/placeholder.png">Th\u00eam v\u00e0o gi\u1ecf</button>
                        </div>
                    </article>

                    <!-- TAI NGHE 2 -->
                    <article class="product-card">
                        <img src="images/placeholder.png" alt="Tai nghe kh\u00f4ng d\u00e2y" style="background:#f1f5f9;height:210px;object-fit:contain;">
                        <h3>Tai nghe kh\u00f4ng d\u00e2y Bluetooth</h3>
                        <p class="spec">Bluetooth 5.0 • Pin 6 gi\u1edd</p>
                        <p class="desc">K\u1ebft n\u1ed1i \u1ed5n \u0111\u1ecbnh, ti\u1ec7n l\u1ee3i khi t\u1eadp th\u1ec3 d\u1ee5c v\u00e0 di chuy\u1ec3n.</p>
                        <div class="price-box">
                            <span class="new-price">350.000\u0111</span>
                            <span class="old-price">450.000\u0111</span>
                        </div>
                        <div class="product-actions">
                            <span class="detail-link" style="display:flex;align-items:center;justify-content:center;">S\u1eafp c\u00f3 \u1ea3nh</span>
                            <button class="add-cart-btn"
                                data-id="tai_nghe_2"
                                data-name="Tai nghe kh\u00f4ng d\u00e2y Bluetooth"
                                data-price="350.000\u0111"
                                data-image="images/placeholder.png">Th\u00eam v\u00e0o gi\u1ecf</button>
                        </div>
                    </article>

                    <!-- C\u1ee4C S\u1ea0C 1 -->
                    <article class="product-card">
                        <img src="images/placeholder.png" alt="C\u1ee7c s\u1ea1c nhanh 20W" style="background:#f1f5f9;height:210px;object-fit:contain;">
                        <h3>C\u1ee7c s\u1ea1c nhanh 20W</h3>
                        <p class="spec">USB-C • S\u1ea1c nhanh PD</p>
                        <p class="desc">S\u1ea1c nhanh h\u01a1n 3 l\u1ea7n s\u1ea1c th\u01b0\u1eddng, t\u01b0\u01a1ng th\u00edch nhi\u1ec1u thi\u1ebft b\u1ecb.</p>
                        <div class="price-box">
                            <span class="new-price">200.000\u0111</span>
                            <span class="old-price">280.000\u0111</span>
                        </div>
                        <div class="product-actions">
                            <span class="detail-link" style="display:flex;align-items:center;justify-content:center;">S\u1eafp c\u00f3 \u1ea3nh</span>
                            <button class="add-cart-btn"
                                data-id="cuc_sac_1"
                                data-name="C\u1ee7c s\u1ea1c nhanh 20W"
                                data-price="200.000\u0111"
                                data-image="images/placeholder.png">Th\u00eam v\u00e0o gi\u1ecf</button>
                        </div>
                    </article>

                    <!-- C\u1ee4C S\u1ea0C 2 -->
                    <article class="product-card">
                        <img src="images/placeholder.png" alt="C\u1ee7c s\u1ea1c kh\u00f4ng d\u00e2y" style="background:#f1f5f9;height:210px;object-fit:contain;">
                        <h3>C\u1ee7c s\u1ea1c kh\u00f4ng d\u00e2y 15W</h3>
                        <p class="spec">Wireless • T\u01b0\u01a1ng th\u00edch Qi</p>
                        <p class="desc">S\u1ea1c kh\u00f4ng d\u00e2y ti\u1ec7n l\u1ee3i, \u0111\u1eb7t m\u00e1y l\u00ean l\u00e0 s\u1ea1c, kh\u00f4ng c\u1ea7n c\u1eafm c\u00e1p.</p>
                        <div class="price-box">
                            <span class="new-price">320.000\u0111</span>
                            <span class="old-price">400.000\u0111</span>
                        </div>
                        <div class="product-actions">
                            <span class="detail-link" style="display:flex;align-items:center;justify-content:center;">S\u1eafp c\u00f3 \u1ea3nh</span>
                            <button class="add-cart-btn"
                                data-id="cuc_sac_2"
                                data-name="C\u1ee7c s\u1ea1c kh\u00f4ng d\u00e2y 15W"
                                data-price="320.000\u0111"
                                data-image="images/placeholder.png">Th\u00eam v\u00e0o gi\u1ecf</button>
                        </div>
                    </article>

                </div><!-- /.product-grid -->
            </div>
        </section>
    </main>

""" + footer() + "\n\n" + scripts() + """
</body>
</html>"""

# ===== LOGIN.HTML =====
login = """<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Đăng nhập tài khoản SmartMobile.">
    <title>Đăng nhập - SmartMobile</title>
    <link rel="stylesheet" href="style.css">
    <link rel="shortcut icon" href="images/logo-phone.svg">
</head>
<body>

""" + header() + """

    <main class="auth-section">
        <div class="container auth-container">
            <div class="auth-box">
                <h2>Đăng nhập</h2>
                <form id="loginForm" onsubmit="handleLogin(event)">
                    <div class="form-group">
                        <label for="loginEmail">Email</label>
                        <input type="email" id="loginEmail" class="form-input" placeholder="VD: nguyenvana@gmail.com" required>
                    </div>
                    <div class="form-group">
                        <label for="loginPassword">Mật khẩu</label>
                        <input type="password" id="loginPassword" class="form-input" placeholder="Nhập mật khẩu" required>
                    </div>
                    <button type="submit" class="form-submit-btn btn btn-primary">Đăng nhập</button>
                </form>
                <p class="auth-link-text">
                    Chưa có tài khoản? <a href="register.html">Đăng ký ngay</a>
                </p>
            </div>
        </div>
    </main>

""" + footer() + "\n\n" + scripts("""
    <script>
        function handleLogin(event) {
            event.preventDefault();
            var email = document.getElementById('loginEmail').value;
            // Giả lập đăng nhập: lấy tên từ email
            var username = email.split('@')[0];
            localStorage.setItem("isLoggedIn", "true");
            localStorage.setItem("username", username);
            alert("Đăng nhập thành công!");
            window.location.href = "index.html";
        }
    </script>""") + """
</body>
</html>"""

# ===== REGISTER.HTML =====
register = """<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Đăng ký tài khoản SmartMobile.">
    <title>Đăng ký - SmartMobile</title>
    <link rel="stylesheet" href="style.css">
    <link rel="shortcut icon" href="images/logo-phone.svg">
</head>
<body>

""" + header() + """

    <main class="auth-section">
        <div class="container auth-container">
            <div class="auth-box">
                <h2>Đăng ký</h2>
                <form id="registerForm" onsubmit="handleRegister(event)">
                    <div class="form-group">
                        <label for="regName">Họ và tên</label>
                        <input type="text" id="regName" class="form-input" placeholder="VD: Nguyễn Văn A" required>
                    </div>
                    <div class="form-group">
                        <label for="regEmail">Email</label>
                        <input type="email" id="regEmail" class="form-input" placeholder="VD: nguyenvana@gmail.com" required>
                    </div>
                    <div class="form-group">
                        <label for="regPassword">Mật khẩu</label>
                        <input type="password" id="regPassword" class="form-input" placeholder="Nhập mật khẩu" required>
                    </div>
                    <div class="form-group">
                        <label for="regConfirm">Xác nhận mật khẩu</label>
                        <input type="password" id="regConfirm" class="form-input" placeholder="Nhập lại mật khẩu" required>
                    </div>
                    <button type="submit" class="form-submit-btn btn btn-primary">Đăng ký</button>
                </form>
                <p class="auth-link-text">
                    Đã có tài khoản? <a href="login.html">Đăng nhập</a>
                </p>
            </div>
        </div>
    </main>

""" + footer() + "\n\n" + scripts("""
    <script>
        function handleRegister(event) {
            event.preventDefault();
            var pass = document.getElementById('regPassword').value;
            var confirm = document.getElementById('regConfirm').value;
            if (pass !== confirm) {
                alert("Mật khẩu xác nhận không khớp!");
                return;
            }
            alert("Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.");
            window.location.href = "login.html";
        }
    </script>""") + """
</body>
</html>"""

# Ghi file
with open('about.html', 'w', encoding='utf-8') as f: f.write(about)
with open('news.html', 'w', encoding='utf-8') as f: f.write(news)
with open('contact.html', 'w', encoding='utf-8') as f: f.write(contact)
with open('accessories.html', 'w', encoding='utf-8') as f: f.write(accessories)
with open('login.html', 'w', encoding='utf-8') as f: f.write(login)
with open('register.html', 'w', encoding='utf-8') as f: f.write(register)
print("Done! 6 files written successfully.")
