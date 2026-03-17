-- ============================================
-- SCRIPT CẬP NHẬT EMAIL TRONG DATABASE
-- ============================================

-- 🔍 BƯỚC 1: TÌM EMAIL CẦN SỬA
SELECT 
    bd.MaBanDoc,
    bd.HoTen,
    bd.Email AS Email_Cu,
    bd.SDT
FROM BanDoc bd
WHERE bd.Email = 'zwin3004@gmail.com';

-- Kết quả mong đợi: MaBanDoc = 4, HoTen = ...

-- ============================================
-- 📝 BƯỚC 2: CẬP NHẬT EMAIL MỚI
-- ============================================
-- Thay 'email_moi_cua_ban@gmail.com' bằng email thật của bạn

-- Cách 1: Cập nhật theo MaBanDoc
UPDATE BanDoc 
SET Email = 'ngoctienvx27@gmail.com'  -- ← Thay email của bạn ở đây
WHERE MaBanDoc = 4;

-- Cách 2: Cập nhật theo email cũ
-- UPDATE BanDoc 
-- SET Email = 'ngoctienvx27@gmail.com'
-- WHERE Email = 'zwin3004@gmail.com';

-- ============================================
-- ✅ BƯỚC 3: KIỂM TRA KẾT QUẢ
-- ============================================
SELECT 
    bd.MaBanDoc,
    bd.HoTen,
    bd.Email AS Email_Moi,
    bd.SDT
FROM BanDoc bd
WHERE bd.MaBanDoc = 4;

-- ============================================
-- 🎯 BƯỚC 4: KIỂM TRA PHIẾU MƯỢN QUÁ HẠN
-- ============================================
-- Xem phiếu mượn quá hạn có email mới chưa
SELECT 
    pm.MaPhieuMuon,
    bd.HoTen,
    bd.Email,
    pm.NgayHenTra,
    DATEDIFF(DAY, pm.NgayHenTra, GETDATE()) AS SoNgayQuaHan
FROM PhieuMuon pm
JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
WHERE pm.DaTra = 0 AND pm.NgayHenTra < GETDATE();

-- ============================================
-- 💡 GỢI Ý
-- ============================================
-- Nếu muốn cập nhật email cho NHIỀU bạn đọc:
/*
UPDATE BanDoc 
SET Email = 'ngoctienvx27@gmail.com'
WHERE MaBanDoc IN (4, 5, 6, 7);  -- Danh sách MaBanDoc
*/

-- Nếu muốn cập nhật email cho TẤT CẢ bạn đọc có email cũ:
/*
UPDATE BanDoc 
SET Email = 'ngoctienvx27@gmail.com'
WHERE Email LIKE '%zwin%';
*/
