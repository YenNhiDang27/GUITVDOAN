using System;

namespace ThuVien.Models
{
    /// <summary>
    /// Model lưu thông tin cảm xúc được phân tích từ trạng thái người dùng
    /// </summary>
    public class CamXuc
    {
        public int MaCamXuc { get; set; }
        public string TenCamXuc { get; set; } // Vui vẻ, Buồn bã, Lo lắng, Hạnh phúc, Căng thẳng, Tức giận, Bình thường...
        public string MoTa { get; set; }
        public string TagSach { get; set; } // Tags để tìm sách phù hợp: "inspiring,motivational,happy"
    }

    /// <summary>
    /// Kết quả phân tích cảm xúc từ văn bản người dùng
    /// </summary>
    public class PhanTichCamXuc
    {
        public string CamXucChinh { get; set; } // Cảm xúc chính: "buồn", "vui", "lo lắng"...
        public double DoTinCay { get; set; } // 0-1: Độ chắc chắn của việc phân tích
        public string LyDo { get; set; } // Giải thích tại sao phân loại vào cảm xúc này
        public string[] TuKhoaCamXuc { get; set; } // Các từ khóa liên quan: ["buồn", "cô đơn", "mệt mỏi"]
        public string GoiYTheLoai { get; set; } // Thể loại sách phù hợp: "Tâm lý học", "Triết học"...
    }

    /// <summary>
    /// Lưu lịch sử trạng thái cảm xúc của bạn đọc
    /// </summary>
    public class LichSuCamXuc
    {
        public int MaLichSu { get; set; }
        public int MaBanDoc { get; set; }
        public string TrangThai { get; set; } // Văn bản người dùng nhập: "Hôm nay tôi buồn quá"
        public string CamXucPhanTich { get; set; } // Kết quả phân tích: "Buồn bã"
        public double DoTinCay { get; set; }
        public DateTime ThoiGian { get; set; }
        public string GoiYSachIds { get; set; } // "12,45,67" - Danh sách sách đã gợi ý
    }
}
