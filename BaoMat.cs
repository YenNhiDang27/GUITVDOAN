using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ThuVien
{
    public static class BaoMat
    {
        // Hàm mã hóa chuỗi sang SHA-256
        public static string MaHoaMatKhau(string matKhau)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Chuyển chuỗi đầu vào thành mảng byte
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(matKhau));
                // Kiểm tra kỹ các dấu phẩy và dấu cách
                string sql = "UPDATE TaiKhoan SET MatKhau = @pass WHERE TenDangNhap = @user";
                // Chuyển mảng byte về lại chuỗi String (Hex)
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
