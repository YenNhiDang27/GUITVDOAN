-- ============================================
-- RESET TẤT CẢ MẬT KHẨU VỀ DẠNG DỄ NHỚ (TẠM THỜI)
-- Mẫu: Tv@{TenDangNhap}{4 số cuối SDT}
-- Ví dụ: Tv@admin17727
-- ============================================

BEGIN TRAN;

BEGIN TRY
    -- 1) Xem trước mật khẩu mới
    SELECT
        MaTaiKhoan,
        TenDangNhap,
        SoDienThoai,
        CONCAT('Tv@', TenDangNhap, COALESCE(RIGHT(SoDienThoai, 4), '0000')) AS MatKhauMoi_DeNho
    FROM TaiKhoan
    WHERE TenDangNhap IS NOT NULL;

    -- 2) Cập nhật mật khẩu
    UPDATE TaiKhoan
    SET MatKhau = CONCAT('Tv@', TenDangNhap, COALESCE(RIGHT(SoDienThoai, 4), '0000'))
    WHERE TenDangNhap IS NOT NULL;

    COMMIT TRAN;
    PRINT N'✅ Đã cập nhật tất cả mật khẩu về dạng dễ nhớ.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN;

    PRINT N'❌ Lỗi: ' + ERROR_MESSAGE();
END CATCH;

-- 3) Kiểm tra sau khi cập nhật
SELECT
    MaTaiKhoan,
    TenDangNhap,
    MatKhau,
    SoDienThoai
FROM TaiKhoan
ORDER BY MaTaiKhoan;

-- Lưu ý:
-- Sau khi chạy script, mở ứng dụng và đăng nhập 1 lần (hoặc khởi động app)
-- để hệ thống tự nâng cấp/lưu lại mật khẩu dạng băm PBKDF2 an toàn.
