using System;

namespace ThuVien.Models
{
    /// <summary>
    /// Phân tích thói quen đọc sách của bạn đọc
    /// </summary>
    public class ThoiQuanDocSach
    {
        public int MaThoiQuan { get; set; }
        public int MaBanDoc { get; set; }
        public string TheLoaiUaThich { get; set; }  // JSON array
        public string TacGiaUaThich { get; set; }   // JSON array
        public int SoLuongSachDaMuon { get; set; }
        public int SoLuongSachDaDoc { get; set; }
        public double TrungBinhSoTrang { get; set; }
        public string ThoiGianDocUaThich { get; set; }  // "sang", "trua", "chieu", "toi"
        public DateTime NgayCapNhat { get; set; }
    }
}
