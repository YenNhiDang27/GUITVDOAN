using System;

namespace ThuVien.Models
{
    /// <summary>
    /// Lưu các gợi ý sách từ AI cho người dùng
    /// </summary>
    public class GoiYSach
    {
        public int MaGoiY { get; set; }
        public int MaBanDoc { get; set; }
        public int MaSach { get; set; }
        public decimal DiemGoiY { get; set; }  // Độ phù hợp (0-1)
        public string LyDoGoiY { get; set; }
        public DateTime NgayGoiY { get; set; }
        public bool DaXem { get; set; }
        public bool DaMuon { get; set; }
    }
}
