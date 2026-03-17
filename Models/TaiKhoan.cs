using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThuVien.Models
{
    public class TaiKhoan
    {
        public int MaTaiKhoan { get; set; }
        public required string TenDangNhap { get; set; }
        public required string MatKhau { get; set; }
        public required string LoaiNguoiDung { get; set; }
        public int? MaBanDoc { get; set; }
        public required string Email { get; set; }
        public required string SoDienThoai { get; set; }

        public static TaiKhoan? DangNhapHienTai { get; set; }
    }
}
