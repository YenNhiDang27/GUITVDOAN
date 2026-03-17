using System;

namespace ThuVien.Models
{
    /// <summary>
    /// Lưu lịch sử trò chuyện với AI chatbot
    /// </summary>
    public class LichSuChat
    {
        public int MaLichSu { get; set; }
        public int? MaBanDoc { get; set; }  // null nếu là khách
        public string NoiDungNguoiDung { get; set; }
        public string NoiDungAI { get; set; }
        public DateTime ThoiGian { get; set; }
        public string LoaiYeuCau { get; set; }  // "goi_y", "tim_kiem", "thong_ke"
    }
}
