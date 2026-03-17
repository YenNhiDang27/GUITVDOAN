using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThuVien.Models
{
    public class ChiTietPhieuMuon
    {
        public int MaChiTiet { get; set; }
        public int MaPhieuMuon { get; set; }
        public int MaSach { get; set; }
        public int SoLuong { get; set; }
        public DateTime? NgayTra { get; set; }
        public decimal PhiPhat { get; set; }
    }
}
