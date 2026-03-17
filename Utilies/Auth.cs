using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;

namespace ThuVien.Utilies
{
    public static class Auth
    {
        public static int MaTaiKhoan { get; private set; }
        public static string TenDangNhap { get; private set; }
        public static string LoaiNguoiDung { get; private set; }
        public static int? MaBanDoc { get; private set; }

        public static void DangNhap(TaiKhoan taiKhoan)
        {
            MaTaiKhoan = taiKhoan.MaTaiKhoan;
            TenDangNhap = taiKhoan.TenDangNhap;
            LoaiNguoiDung = taiKhoan.LoaiNguoiDung;
            MaBanDoc = taiKhoan.MaBanDoc;
        }

        public static void DangXuat()
        {
            MaTaiKhoan = 0;
            TenDangNhap = null;
            LoaiNguoiDung = null;
            MaBanDoc = null;
        }
    }

}
