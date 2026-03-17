using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThuVien.Models
{
    public class Sach
    {
        public int MaSach { get; set; }
        public string? TenSach { get; set; }
        public string? TacGia { get; set; }
        public int MaLoai { get; set; }
        public int MaNXB { get; set; }
        public string? TenLoai { get; set; }
        public string? TenNXB { get; set; }
        public int NamXuatBan { get; set; }
        public int SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public int SoTrang { get; set; }
        public int GiaTien { get; set; }
        public string? TinhTrangSach { get; set; }
        public string? GioiThieu { get; set; }
    }
}
