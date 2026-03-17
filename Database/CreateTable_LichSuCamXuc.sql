-- =============================================
-- Script tạo bảng LichSuCamXuc
-- Lưu lịch sử cảm xúc và gợi ý sách cho bạn đọc
-- =============================================

USE [ThuVien]  -- Thay tên database phù hợp với dự án của bạn
GO

-- Kiểm tra và xóa bảng cũ nếu tồn tại (chỉ dùng khi phát triển)
IF OBJECT_ID('dbo.LichSuCamXuc', 'U') IS NOT NULL
    DROP TABLE dbo.LichSuCamXuc;
GO

-- Tạo bảng LichSuCamXuc
CREATE TABLE dbo.LichSuCamXuc
(
    MaLichSu INT IDENTITY(1,1) PRIMARY KEY,
    MaBanDoc INT NOT NULL,
    TrangThai NVARCHAR(500) NOT NULL,           -- Văn bản người dùng nhập
    CamXucPhanTich NVARCHAR(100) NOT NULL,      -- Cảm xúc phân tích được: buồn, vui, lo lắng...
    DoTinCay FLOAT NOT NULL DEFAULT 0.5,        -- Độ tin cậy của phân tích (0-1)
    ThoiGian DATETIME NOT NULL DEFAULT GETDATE(),
    GoiYSachIds NVARCHAR(500),                  -- Danh sách ID sách được gợi ý: "12,45,67"
    
    -- Foreign key (nếu có bảng BanDoc)
    CONSTRAINT FK_LichSuCamXuc_BanDoc FOREIGN KEY (MaBanDoc) 
        REFERENCES dbo.BanDoc(MaBanDoc) ON DELETE CASCADE
);
GO

-- Tạo index để tăng tốc truy vấn
CREATE NONCLUSTERED INDEX IX_LichSuCamXuc_MaBanDoc 
    ON dbo.LichSuCamXuc(MaBanDoc);
GO

CREATE NONCLUSTERED INDEX IX_LichSuCamXuc_ThoiGian 
    ON dbo.LichSuCamXuc(ThoiGian DESC);
GO

CREATE NONCLUSTERED INDEX IX_LichSuCamXuc_CamXuc 
    ON dbo.LichSuCamXuc(CamXucPhanTich);
GO

-- Thêm dữ liệu mẫu (tuỳ chọn)
INSERT INTO dbo.LichSuCamXuc (MaBanDoc, TrangThai, CamXucPhanTich, DoTinCay, ThoiGian, GoiYSachIds)
VALUES 
    (1, 'Hôm nay tôi buồn quá, cảm thấy mệt mỏi', 'buồn', 0.85, GETDATE(), '1,5,12'),
    (1, 'Tôi rất vui vì được nghỉ cuối tuần', 'vui', 0.90, DATEADD(day, -1, GETDATE()), '3,7,9'),
    (2, 'Lo lắng về kỳ thi sắp tới', 'lo lắng', 0.88, DATEADD(day, -2, GETDATE()), '2,8,15');
GO

-- Kiểm tra dữ liệu
SELECT * FROM dbo.LichSuCamXuc;
GO

PRINT 'Tạo bảng LichSuCamXuc thành công!';
GO
