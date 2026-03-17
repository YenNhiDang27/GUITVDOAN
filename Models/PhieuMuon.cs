
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThuVien.Models
{
    public class PhieuMuon
    {
        public int MaPhieuMuon { get; set; }
        public int MaBanDoc { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime NgayHenTra { get; set; }
        public bool DaTra { get; set; }
        public string NguoiLapPhieu { get; set; }
    }
}
