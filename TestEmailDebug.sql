-- ============================================
-- SCRIPT KIỂM TRA VÀ SỬA LỖI EMAIL
-- ============================================

-- 1️⃣ KIỂM TRA CẤU TRÚC BẢNG BanDoc
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'BanDoc'
ORDER BY ORDINAL_POSITION;

-- 2️⃣ KIỂM TRA DỮ LIỆU HIỆN TẠI
SELECT TOP 10 
    MaBanDoc, 
    HoTen,
    CASE 
        WHEN Email IS NULL THEN '❌ NULL'
        WHEN Email = '' THEN '❌ RỖNG'
        ELSE Email
    END AS Email_Status
FROM BanDoc;

-- 3️⃣ KIỂM TRA PHIẾU MƯỢN QUÁ HẠN VÀ EMAIL
SELECT 
    pm.MaPhieuMuon,
    pm.MaBanDoc,
    pm.NguoiLapPhieu,
    bd.HoTen,
    bd.Email,
    pm.NgayMuon,
    pm.NgayHenTra,
    pm.DaTra,
    DATEDIFF(DAY, pm.NgayHenTra, GETDATE()) AS SoNgayQuaHan,
    CASE 
        WHEN bd.Email IS NULL THEN '❌ Không có email'
        WHEN bd.Email = '' THEN '❌ Email rỗng'
        ELSE '✅ OK'
    END AS TrangThaiEmail
FROM PhieuMuon pm
LEFT JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
WHERE pm.DaTra = 0 AND pm.NgayHenTra < GETDATE()
ORDER BY pm.NgayHenTra;

-- 👇 KẾT QUẢ MONG ĐỢI:
-- Nếu có dữ liệu => DataGridView sẽ hiển thị cột Email
-- Nếu KHÔNG có dữ liệu => Tạo phiếu mượn quá hạn để test

-- ============================================
-- 4️⃣ NẾU CHƯA CÓ CỘT EMAIL, CHẠY LỆNH NÀY:
-- ============================================
-- Bỏ comment 2 dòng dưới nếu bảng BanDoc chưa có cột Email
-- ALTER TABLE BanDoc ADD Email NVARCHAR(100) NULL;
-- GO

-- ============================================
-- 5️⃣ TẠO PHIẾU MƯỢN QUÁ HẠN ĐỂ TEST (NẾU CẦN)
-- ============================================
-- Tạo 1 phiếu mượn quá hạn để test gửi email
-- Bỏ comment các dòng dưới để chạy:

/*
-- Lấy MaBanDoc có email
DECLARE @MaBanDoc INT = (SELECT TOP 1 MaBanDoc FROM BanDoc WHERE Email IS NOT NULL AND Email LIKE '%@gmail.com');
DECLARE @MaSach INT = (SELECT TOP 1 MaSach FROM Sach);

-- Tạo phiếu mượn quá hạn 3 ngày
INSERT INTO PhieuMuon (MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu)
VALUES (@MaBanDoc, GETDATE() - 10, GETDATE() - 3, 0, 'Admin');

-- Lấy MaPhieuMuon vừa tạo
DECLARE @MaPhieuMuon INT = SCOPE_IDENTITY();

-- Thêm chi tiết phiếu mượn
INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, SoLuongSachMuon, PhiPhat)
VALUES (@MaPhieuMuon, @MaSach, 1, 15000);

-- Kiểm tra kết quả
SELECT 
    pm.MaPhieuMuon,
    bd.HoTen,
    bd.Email,
    pm.NgayHenTra,
    DATEDIFF(DAY, pm.NgayHenTra, GETDATE()) AS SoNgayQuaHan
FROM PhieuMuon pm
JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
WHERE pm.MaPhieuMuon = @MaPhieuMuon;
*/
