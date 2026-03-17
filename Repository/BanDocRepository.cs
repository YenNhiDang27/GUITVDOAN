using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;
using System.Data;
using ThuVien.Utilies;
using Microsoft.Data.SqlClient;


namespace ThuVien.Repository
{
    public class BanDocRepository
    {


        public List<BanDoc> GetAll()
        {
            List<BanDoc> list = new List<BanDoc>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM BanDoc";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new BanDoc
                        {
                            MaBanDoc = reader["MaBanDoc"] != DBNull.Value ? Convert.ToInt32(reader["MaBanDoc"]) : 0,
                            HoTen = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : null,
                            NgaySinh = reader["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySinh"]) : DateTime.MinValue,
                            DiaChi = reader["DiaChi"] != DBNull.Value ? reader["DiaChi"].ToString() : null,
                            NgayDangKy = reader["NgayDangKy"] != DBNull.Value ? Convert.ToDateTime(reader["NgayDangKy"]) : DateTime.MinValue,
                            HinhAnh = reader["HinhAnh"] != DBNull.Value ? reader["HinhAnh"].ToString() : null,
                            GioiTinh = reader["GioiTinh"] != DBNull.Value ? Convert.ToBoolean(reader["GioiTinh"]) : false,
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                            SDT = reader["SDT"] != DBNull.Value ? reader["SDT"].ToString() : null,
                            CCCD = reader["CCCD"] != DBNull.Value ? reader["CCCD"].ToString() : null
                        });
                    }
                }
            }
            return list;
        }

        public BanDoc? GetBanDocBySDT(string sdt)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM BanDoc WHERE SDT = @SDT";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BanDoc
                            {
                                MaBanDoc = reader["MaBanDoc"] != DBNull.Value ? Convert.ToInt32(reader["MaBanDoc"]) : 0,
                                HoTen = reader["HoTen"] != DBNull.Value ? reader["HoTen"].ToString() : null,
                                NgaySinh = reader["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySinh"]) : DateTime.MinValue,
                                DiaChi = reader["DiaChi"] != DBNull.Value ? reader["DiaChi"].ToString() : null,
                                NgayDangKy = reader["NgayDangKy"] != DBNull.Value ? Convert.ToDateTime(reader["NgayDangKy"]) : DateTime.MinValue,
                                HinhAnh = reader["HinhAnh"] != DBNull.Value ? reader["HinhAnh"].ToString() : null,
                                GioiTinh = reader["GioiTinh"] != DBNull.Value ? Convert.ToBoolean(reader["GioiTinh"]) : false,
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                SDT = reader["SDT"] != DBNull.Value ? reader["SDT"].ToString() : null,
                                CCCD = reader["CCCD"] != DBNull.Value ? reader["CCCD"].ToString() : null
                            };
                        }
                    }
                }
            }
            return null; // Không tìm thấy bạn đọc
        }

        public void Add(BanDoc banDoc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO BanDoc (HoTen, GioiTinh, NgaySinh, CCCD, SDT, Email, DiaChi, NgayDangKy, HinhAnh) VALUES (@HoTen, @GioiTinh, @NgaySinh, @CCCD, @SDT, @Email, @DiaChi, @NgayDangKy, @HinhAnh)";

                using (SqlCommand cmd = new SqlCommand(sql, conn)) // Đổi query thành sql
                {
                    cmd.Parameters.AddWithValue("@HoTen", banDoc.HoTen);
                    cmd.Parameters.AddWithValue("@NgaySinh", banDoc.NgaySinh);
                    cmd.Parameters.AddWithValue("@DiaChi", banDoc.DiaChi);
                    cmd.Parameters.AddWithValue("@NgayDangKy", banDoc.NgayDangKy);
                    cmd.Parameters.AddWithValue("@HinhAnh", banDoc.HinhAnh ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GioiTinh", banDoc.GioiTinh);
                    cmd.Parameters.AddWithValue("@Email", banDoc.Email);
                    cmd.Parameters.AddWithValue("@SDT", banDoc.SDT);
                    cmd.Parameters.AddWithValue("@CCCD", banDoc.CCCD); // Đổi command thành cmd
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(BanDoc banDoc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "UPDATE BanDoc SET HoTen=@HoTen, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, CCCD=@CCCD, SDT=@SDT, Email=@Email, DiaChi=@DiaChi, NgayDangKy=@NgayDangKy, HinhAnh=@HinhAnh WHERE MaBanDoc=@MaBanDoc";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", banDoc.HoTen);
                    cmd.Parameters.AddWithValue("@NgaySinh", banDoc.NgaySinh);
                    cmd.Parameters.AddWithValue("@DiaChi", banDoc.DiaChi);
                    cmd.Parameters.AddWithValue("@NgayDangKy", banDoc.NgayDangKy);
                    cmd.Parameters.AddWithValue("@HinhAnh", banDoc.HinhAnh ?? (object)DBNull.Value); 
                    cmd.Parameters.AddWithValue("@GioiTinh", banDoc.GioiTinh);
                    cmd.Parameters.AddWithValue("@Email", banDoc.Email);
                    cmd.Parameters.AddWithValue("@SDT", banDoc.SDT);
                    cmd.Parameters.AddWithValue("@CCCD", banDoc.CCCD);
                    cmd.Parameters.AddWithValue("@MaBanDoc", banDoc.MaBanDoc);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int maBanDoc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM BanDoc WHERE MaBanDoc=@MaBanDoc";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public BanDoc? GetBanDocById(int maBanDoc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM BanDoc WHERE MaBanDoc = @MaBanDoc";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new BanDoc
                    {
                        MaBanDoc = reader.GetInt32(0),
                        HoTen = reader.GetString(1),
                        NgaySinh = reader.GetDateTime(2),
                        DiaChi = reader.GetString(3),
                        NgayDangKy = reader.GetDateTime(4),
                        HinhAnh = reader.IsDBNull(5) ? null : reader.GetString(5),
                        GioiTinh = reader.GetBoolean(6),
                        Email = reader.GetString(7),
                        SDT = reader.GetString(8),
                        CCCD = reader.IsDBNull(9) ? null : reader.GetString(9)
                    };
                }
                return null;
            }
        }

        public string GetEmailBySoDienThoai(string soDienThoai)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT Email FROM BanDoc WHERE SDT = @SoDienThoai";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result?.ToString() ?? string.Empty;
            }
        }

        public string GetTinhTrangMuon(int maBanDoc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM PhieuMuon WHERE MaBanDoc = @MaBanDoc AND Datra = 0";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                conn.Open();

                int count = (int)cmd.ExecuteScalar();
                return count > 0 ? "Đang mượn" : "Đã trả";
            }
        }

    }

}
