-- =============================================
-- Script: Tạo bảng GoiYSach cho hệ thống gợi ý sách thông minh
-- Mô tả: Lưu trữ các gợi ý sách từ ML Recommendation System
-- Ngày tạo: 2025
-- =============================================

USE [ThuVien] -- Thay đổi tên database nếu cần
GO

-- Kiểm tra và xóa bảng nếu đã tồn tại (chỉ dùng khi test)
-- DROP TABLE IF EXISTS GoiYSach
-- GO

-- Tạo bảng GoiYSach
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GoiYSach]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[GoiYSach](
        [MaGoiY] [int] IDENTITY(1,1) NOT NULL,
        [MaBanDoc] [int] NOT NULL,
        [MaSach] [int] NOT NULL,
        [DiemGoiY] [decimal](5, 4) NOT NULL,
        [LyDoGoiY] [nvarchar](500) NULL,
        [NgayGoiY] [datetime] NOT NULL,
        [DaXem] [bit] NOT NULL DEFAULT(0),
        [DaMuon] [bit] NOT NULL DEFAULT(0),
        CONSTRAINT [PK_GoiYSach] PRIMARY KEY CLUSTERED ([MaGoiY] ASC),
        CONSTRAINT [FK_GoiYSach_BanDoc] FOREIGN KEY([MaBanDoc]) REFERENCES [dbo].[BanDoc] ([MaBanDoc]),
        CONSTRAINT [FK_GoiYSach_Sach] FOREIGN KEY([MaSach]) REFERENCES [dbo].[Sach] ([MaSach])
    )
    
    PRINT 'Tạo bảng GoiYSach thành công!'
END
ELSE
BEGIN
    PRINT 'Bảng GoiYSach đã tồn tại!'
END
GO

-- Tạo các index để tăng hiệu suất truy vấn
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_GoiYSach_MaBanDoc')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_GoiYSach_MaBanDoc] 
    ON [dbo].[GoiYSach] ([MaBanDoc])
    INCLUDE ([DiemGoiY], [NgayGoiY])
    
    PRINT 'Tạo index IX_GoiYSach_MaBanDoc thành công!'
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_GoiYSach_DiemGoiY')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_GoiYSach_DiemGoiY] 
    ON [dbo].[GoiYSach] ([DiemGoiY] DESC)
    
    PRINT 'Tạo index IX_GoiYSach_DiemGoiY thành công!'
END
GO

-- Thêm constraints kiểm tra
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_GoiYSach_DiemGoiY')
BEGIN
    ALTER TABLE [dbo].[GoiYSach]
    ADD CONSTRAINT [CK_GoiYSach_DiemGoiY] CHECK ([DiemGoiY] >= 0 AND [DiemGoiY] <= 1)
    
    PRINT 'Thêm constraint CK_GoiYSach_DiemGoiY thành công!'
END
GO

-- Tạo view để xem gợi ý chi tiết
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_GoiYSachChiTiet')
    DROP VIEW [dbo].[vw_GoiYSachChiTiet]
GO

CREATE VIEW [dbo].[vw_GoiYSachChiTiet]
AS
SELECT 
    gy.MaGoiY,
    gy.MaBanDoc,
    bd.HoTen AS TenBanDoc,
    bd.Email,
    gy.MaSach,
    s.TenSach,
    s.TacGia,
    ls.TenLoai,
    nxb.TenNXB,
    s.NamXuatBan,
    s.SoLuong,
    s.TinhTrangSach,
    gy.DiemGoiY,
    CAST(gy.DiemGoiY * 100 AS INT) AS PhanTramPhuHop,
    gy.LyDoGoiY,
    gy.NgayGoiY,
    gy.DaXem,
    gy.DaMuon
FROM GoiYSach gy
INNER JOIN BanDoc bd ON gy.MaBanDoc = bd.MaBanDoc
INNER JOIN Sach s ON gy.MaSach = s.MaSach
INNER JOIN LoaiSach ls ON s.MaLoai = ls.MaLoai
INNER JOIN NhaXuatBan nxb ON s.MaNXB = nxb.MaNXB
GO

PRINT 'Tạo view vw_GoiYSachChiTiet thành công!'
GO

-- Stored Procedure để lấy gợi ý theo bạn đọc
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_LayGoiYTheoBanDoc')
    DROP PROCEDURE [dbo].[sp_LayGoiYTheoBanDoc]
GO

CREATE PROCEDURE [dbo].[sp_LayGoiYTheoBanDoc]
    @MaBanDoc INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM vw_GoiYSachChiTiet
    WHERE MaBanDoc = @MaBanDoc
    ORDER BY DiemGoiY DESC, NgayGoiY DESC
END
GO

PRINT 'Tạo stored procedure sp_LayGoiYTheoBanDoc thành công!'
GO

-- Stored Procedure để xóa gợi ý cũ (hơn 30 ngày)
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_XoaGoiYCu')
    DROP PROCEDURE [dbo].[sp_XoaGoiYCu]
GO

CREATE PROCEDURE [dbo].[sp_XoaGoiYCu]
    @SoNgayGiu INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NgayXoa DATETIME = DATEADD(DAY, -@SoNgayGiu, GETDATE())
    
    DELETE FROM GoiYSach
    WHERE NgayGoiY < @NgayXoa
    
    SELECT @@ROWCOUNT AS SoLuongDaXoa
END
GO

PRINT 'Tạo stored procedure sp_XoaGoiYCu thành công!'
GO

-- Stored Procedure để lấy thống kê gợi ý
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_ThongKeGoiY')
    DROP PROCEDURE [dbo].[sp_ThongKeGoiY]
GO

CREATE PROCEDURE [dbo].[sp_ThongKeGoiY]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TongSoGoiY,
        COUNT(DISTINCT MaBanDoc) AS SoBanDocCoGoiY,
        COUNT(DISTINCT MaSach) AS SoSachDuocGoiY,
        SUM(CASE WHEN DaXem = 1 THEN 1 ELSE 0 END) AS SoGoiYDaXem,
        SUM(CASE WHEN DaMuon = 1 THEN 1 ELSE 0 END) AS SoGoiYDaMuon,
        AVG(DiemGoiY) AS DiemGoiYTrungBinh,
        MAX(NgayGoiY) AS LanGoiYGanNhat
    FROM GoiYSach
END
GO

PRINT 'Tạo stored procedure sp_ThongKeGoiY thành công!'
GO

-- Dữ liệu mẫu (tùy chọn - chỉ để test)
/*
-- Xóa dữ liệu cũ
DELETE FROM GoiYSach
GO

-- Thêm dữ liệu mẫu
INSERT INTO GoiYSach (MaBanDoc, MaSach, DiemGoiY, LyDoGoiY, NgayGoiY, DaXem, DaMuon)
VALUES
(1, 1, 0.95, N'Bạn thích thể loại Văn học. Bạn đã mượn sách của tác giả Nguyễn Nhật Ánh.', GETDATE(), 0, 0),
(1, 2, 0.85, N'Bạn thích thể loại Tiểu thuyết.', GETDATE(), 0, 0),
(1, 3, 0.75, N'Được 3 độc giả có sở thích tương tự gợi ý.', GETDATE(), 1, 0)
GO

PRINT 'Thêm dữ liệu mẫu thành công!'
*/

PRINT '==============================================='
PRINT 'Hoàn tất tạo database cho hệ thống gợi ý sách!'
PRINT '==============================================='
GO
