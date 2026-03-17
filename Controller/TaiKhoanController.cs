using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ThuVien.Models;
using ThuVien.Repository;
using ThuVien.Utilies;

namespace ThuVien.Controller
{
    public class TaiKhoanController
    {
        private TaiKhoanRepository repository = new TaiKhoanRepository();

        public TaiKhoan? TimTaiKhoanTheoEmail(string email)
        {
            return LayDanhSachTaiKhoan().FirstOrDefault(tk =>
                !string.IsNullOrEmpty(tk.Email) &&
                tk.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
            );
        }

        public string? TimEmailTheoSoDienThoai(string soDienThoai)
        {
            return repository.GetEmailBySoDienThoai(soDienThoai);
        }

        public List<TaiKhoan> LayDanhSachTaiKhoan()
        {
            return repository.GetAll();
        }

        public TaiKhoan? TimTaiKhoanTheoTen(string tenDangNhap)
        {
            return repository.GetByTenDangNhap(tenDangNhap);
        }

        public bool ThemTaiKhoan(TaiKhoan taiKhoan)
        {
            return repository.Add(taiKhoan);
        }

        public int? TimMaBanDocTheoSoDienThoai(string soDienThoai)
        {
            return repository.GetMaBanDocBySoDienThoai(soDienThoai);
        }

        public bool CapNhatTaiKhoan(TaiKhoan taiKhoan)
        {
            return repository.Update(taiKhoan);
        }

        public bool XoaTaiKhoan(int maTaiKhoan)
        {
            return repository.Delete(maTaiKhoan);
        }

        public TaiKhoan? DangNhap(string tenDangNhap, string matKhau)
        {
            return repository.DangNhap(tenDangNhap, matKhau);
        }

        public bool DoiMatKhau(int maTaiKhoan, string matKhauMoi)
        {
            return repository.DoiMatKhau(maTaiKhoan, matKhauMoi);
        }
    }
}