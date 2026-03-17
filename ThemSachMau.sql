-- ============================================
-- SCRIPT THÊM 10 SÁCH MẪU NHIỀU THỂ LOẠI
-- ============================================
BEGIN TRAN;

BEGIN TRY
    -- 1) Bổ sung thể loại (nếu chưa có)
    INSERT INTO LoaiSach (TenLoai)
    SELECT v.TenLoai
    FROM (VALUES
        (N'Tiểu thuyết'),
        (N'Khoa học viễn tưởng'),
        (N'Tâm lý - Kỹ năng sống'),
        (N'Lịch sử'),
        (N'Công nghệ thông tin'),
        (N'Kinh doanh'),
        (N'Thiếu nhi'),
        (N'Trinh thám'),
        (N'Văn học Việt Nam'),
        (N'Y học - Sức khỏe')
    ) v(TenLoai)
    WHERE NOT EXISTS (
        SELECT 1 FROM LoaiSach ls WHERE ls.TenLoai = v.TenLoai
    );

    -- 2) Bổ sung nhà xuất bản (nếu chưa có)
    INSERT INTO NhaXuatBan (TenNXB)
    SELECT v.TenNXB
    FROM (VALUES
        (N'NXB Trẻ'),
        (N'NXB Kim Đồng'),
        (N'NXB Tổng hợp TP.HCM'),
        (N'NXB Lao Động'),
        (N'NXB Giáo Dục Việt Nam')
    ) v(TenNXB)
    WHERE NOT EXISTS (
        SELECT 1 FROM NhaXuatBan nxb WHERE nxb.TenNXB = v.TenNXB
    );

    -- 3) Thêm 10 sách đa dạng thể loại (nếu chưa có theo tên sách)
    INSERT INTO Sach
    (
        TenSach, TacGia, MaLoai, MaNXB,
        NamXuatBan, SoLuong, HinhAnh, SoTrang,
        GiaTien, TinhTrangSach, GioiThieu
    )
    SELECT
        s.TenSach,
        s.TacGia,
        ls.MaLoai,
        nxb.MaNXB,
        s.NamXuatBan,
        s.SoLuong,
        s.HinhAnh,
        s.SoTrang,
        s.GiaTien,
        s.TinhTrangSach,
        s.GioiThieu
    FROM
    (
        VALUES
        (N'Dế Mèn Phiêu Lưu Ký', N'Tô Hoài', N'Thiếu nhi', N'NXB Kim Đồng', 2020, 15, N'demen.jpg', 220, 65000, N'Còn', N'Tác phẩm thiếu nhi kinh điển của văn học Việt Nam.'),
        (N'Tôi Thấy Hoa Vàng Trên Cỏ Xanh', N'Nguyễn Nhật Ánh', N'Văn học Việt Nam', N'NXB Trẻ', 2019, 12, N'hoavang.jpg', 378, 98000, N'Còn', N'Câu chuyện tuổi thơ trong trẻo tại làng quê Việt Nam.'),
        (N'Sherlock Holmes: Dấu Bộ Tứ', N'Arthur Conan Doyle', N'Trinh thám', N'NXB Lao Động', 2018, 10, N'sherlock.jpg', 320, 89000, N'Còn', N'Tác phẩm trinh thám kinh điển với thám tử Sherlock Holmes.'),
        (N'Dune: Hành Tinh Cát', N'Frank Herbert', N'Khoa học viễn tưởng', N'NXB Tổng hợp TP.HCM', 2021, 8, N'dune.jpg', 540, 165000, N'Còn', N'Tiểu thuyết khoa học viễn tưởng nổi tiếng thế giới.'),
        (N'Đắc Nhân Tâm', N'Dale Carnegie', N'Tâm lý - Kỹ năng sống', N'NXB Trẻ', 2022, 20, N'dacnhantam.jpg', 320, 86000, N'Còn', N'Cuốn sách nền tảng về giao tiếp và ứng xử.'),
        (N'Lịch Sử Việt Nam Bằng Tranh', N'Nhiều tác giả', N'Lịch sử', N'NXB Kim Đồng', 2020, 18, N'lsvntranh.jpg', 260, 92000, N'Còn', N'Tóm lược lịch sử Việt Nam qua hình ảnh sinh động.'),
        (N'Clean Code', N'Robert C. Martin', N'Công nghệ thông tin', N'NXB Giáo Dục Việt Nam', 2021, 9, N'cleancode.jpg', 464, 210000, N'Còn', N'Nguyên tắc viết mã sạch, dễ bảo trì cho lập trình viên.'),
        (N'Khởi Nghiệp Tinh Gọn', N'Eric Ries', N'Kinh doanh', N'NXB Lao Động', 2020, 11, N'leanstartup.jpg', 336, 120000, N'Còn', N'Phương pháp xây dựng sản phẩm và doanh nghiệp tinh gọn.'),
        (N'Nhà Giả Kim', N'Paulo Coelho', N'Tiểu thuyết', N'NXB Tổng hợp TP.HCM', 2017, 14, N'nhagiakim.jpg', 228, 79000, N'Còn', N'Hành trình theo đuổi ước mơ và ý nghĩa cuộc sống.'),
        (N'Sống Khỏe Mỗi Ngày', N'Bs. Trần Minh', N'Y học - Sức khỏe', N'NXB Giáo Dục Việt Nam', 2023, 16, N'songkhoe.jpg', 240, 88000, N'Còn', N'Kiến thức chăm sóc sức khỏe cơ bản cho mọi gia đình.')
    ) s
    (
        TenSach, TacGia, TenLoai, TenNXB,
        NamXuatBan, SoLuong, HinhAnh, SoTrang,
        GiaTien, TinhTrangSach, GioiThieu
    )
    INNER JOIN LoaiSach ls ON ls.TenLoai = s.TenLoai
    INNER JOIN NhaXuatBan nxb ON nxb.TenNXB = s.TenNXB
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM Sach x
        WHERE x.TenSach = s.TenSach
    );

    COMMIT TRAN;
    PRINT N'✅ Đã thêm dữ liệu sách mẫu thành công.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN;

    PRINT N'❌ Lỗi khi thêm dữ liệu sách mẫu: ' + ERROR_MESSAGE();
END CATCH;


-- ============================================
-- 🔎 KIỂM TRA NHANH 10 SÁCH VỪA THÊM
-- ============================================
SELECT TOP 20
    s.MaSach,
    s.TenSach,
    s.TacGia,
    ls.TenLoai,
    nxb.TenNXB,
    s.NamXuatBan,
    s.SoLuong,
    s.GiaTien
FROM Sach s
INNER JOIN LoaiSach ls ON s.MaLoai = ls.MaLoai
INNER JOIN NhaXuatBan nxb ON s.MaNXB = nxb.MaNXB
ORDER BY s.MaSach DESC;
