using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ThuVien.Models;
using ThuVien.Utilies;
namespace ThuVien.Repository
{
    public class TaiKhoanRepository
    {
        public string? GetEmailBySoDienThoai(string soDienThoai)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT Email FROM BanDoc WHERE SDT = @SoDienThoai";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);

                conn.Open();
                var result = cmd.ExecuteScalar();

                // Trả về null nếu không có kết quả
                return result != null ? result.ToString() : null;
            }
        }

        public List<TaiKhoan> GetAll()
        {
            List<TaiKhoan> danhSachTaiKhoan = new List<TaiKhoan>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM TaiKhoan";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    danhSachTaiKhoan.Add(new TaiKhoan
                    {
                        MaTaiKhoan = reader.GetInt32(0),
                        TenDangNhap = reader.GetString(1),
                        Email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        MatKhau = reader.GetString(3),
                        LoaiNguoiDung = reader.GetString(4),
                        MaBanDoc = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                        SoDienThoai = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                    });
                }
            }
            return danhSachTaiKhoan;
        }

        public bool DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi)
        {
            string query = @"UPDATE TaiKhoan SET MatKhau = @MatKhauMoi WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhauCu";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhauCu", matKhauCu);
                cmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public int? GetMaBanDocBySoDienThoai(string soDienThoai)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT MaBanDoc FROM BanDoc WHERE SDT = @SoDienThoai";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);

                conn.Open();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }
            return null;
        }
        public TaiKhoan? DangNhap(string tenDangNhap, string matKhau)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Đảm bảo các trường required không bị null
                    var tenDangNhapDb = reader.GetString(1) ?? string.Empty;
                    var emailDb = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    var matKhauDb = reader.GetString(3) ?? string.Empty;
                    var loaiNguoiDungDb = reader.GetString(4) ?? string.Empty;

                    var taiKhoan = new TaiKhoan
                    {
                        MaTaiKhoan = reader.GetInt32(0),
                        TenDangNhap = tenDangNhapDb,
                        Email = emailDb,
                        MatKhau = matKhauDb,
                        LoaiNguoiDung = loaiNguoiDungDb,
                        MaBanDoc = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                        SoDienThoai = reader.IsDBNull(6) ? string.Empty : reader.GetString(6) 
                    };

                    // Kiểm tra MaBanDoc nếu cần
                    if (taiKhoan.MaBanDoc == null)
                    {
                        throw new Exception("Tài khoản này chưa liên kết với bạn đọc!");
                    }

                    return taiKhoan;
                }
            }
            return null; // Đăng nhập thất bại
        }

        public bool DoiMatKhau(int maTaiKhoan, string matKhauMoi)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE TaiKhoan SET MatKhau = @MatKhauMoi WHERE MaTaiKhoan = @MaTaiKhoan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public TaiKhoan? GetByTenDangNhap(string tenDangNhap)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new TaiKhoan
                    {
                        MaTaiKhoan = reader.GetInt32(0),
                        TenDangNhap = reader.GetString(1),
                        Email = reader.GetString(2),
                        MatKhau = reader.GetString(3),
                        LoaiNguoiDung = reader.GetString(4),
                        MaBanDoc = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                        SoDienThoai = reader.IsDBNull(6) ? string.Empty : reader.GetString(6) 
                    };
                }
            }
            return null;
        }

        public bool Add(TaiKhoan taiKhoan)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, LoaiNguoiDung, MaBanDoc, Email, SoDienThoai) " +
                               "VALUES (@TenDangNhap, @MatKhau, @LoaiNguoiDung, @MaBanDoc, @Email, @SoDienThoai)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoan.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", taiKhoan.MatKhau);
                cmd.Parameters.AddWithValue("@LoaiNguoiDung", taiKhoan.LoaiNguoiDung);
                cmd.Parameters.AddWithValue("@MaBanDoc", taiKhoan.MaBanDoc.HasValue ? taiKhoan.MaBanDoc.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", taiKhoan.Email ?? string.Empty);
                cmd.Parameters.AddWithValue("@SoDienThoai", taiKhoan.SoDienThoai ?? string.Empty);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(TaiKhoan taiKhoan)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE TaiKhoan SET TenDangNhap = @TenDangNhap, MatKhau = @MatKhau, " +
                               "LoaiNguoiDung = @LoaiNguoiDung, MaBanDoc = @MaBanDoc, Email = @Email, SoDienThoai = @SoDienThoai WHERE MaTaiKhoan = @MaTaiKhoan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", taiKhoan.MaTaiKhoan);
                cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoan.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", taiKhoan.MatKhau);
                cmd.Parameters.AddWithValue("@LoaiNguoiDung", taiKhoan.LoaiNguoiDung);
                cmd.Parameters.AddWithValue("@MaBanDoc", taiKhoan.MaBanDoc.HasValue ? (object)taiKhoan.MaBanDoc.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)taiKhoan.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SoDienThoai", (object)taiKhoan.SoDienThoai ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int maTaiKhoan)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM TaiKhoan WHERE MaTaiKhoan = @MaTaiKhoan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}


