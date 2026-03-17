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
    public class SachRepository
    {
        public SachRepository()
        {
            
        }

        public List<Sach> GetAll()
        {
            List<Sach> list = new List<Sach>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Sach";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Sach
                        {
                            MaSach = reader.GetInt32(0),
                            TenSach = reader.GetString(1),
                            TacGia = reader.GetString(2),
                            MaLoai = reader.GetInt32(3),
                            MaNXB = reader.GetInt32(4),
                            NamXuatBan = reader.GetInt32(5),
                            SoLuong = reader.GetInt32(6),
                            HinhAnh = reader.GetString(7),
                            SoTrang = reader.GetInt32(8),
                            GiaTien = reader.GetInt32(9),
                            TinhTrangSach = reader.GetString(10),
                            GioiThieu = reader.GetString(11)
                        });
                    }
                }
            }
            return list;
        }

        public Sach? GetById(int maSach)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT s.MaSach, s.TenSach, s.TacGia, s.MaLoai, l.TenLoai, s.MaNXB, nxb.TenNXB, 
            s.NamXuatBan, s.SoLuong, s.HinhAnh, s.SoTrang, s.GiaTien, s.TinhTrangSach, s.GioiThieu
        FROM Sach s
        INNER JOIN LoaiSach l ON s.MaLoai = l.MaLoai
        INNER JOIN NhaXuatBan nxb ON s.MaNXB = nxb.MaNXB
        WHERE s.MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", maSach);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Sach
                        {
                            MaSach = (int)reader["MaSach"],
                            TenSach = reader["TenSach"].ToString(),
                            TacGia = reader["TacGia"].ToString(),
                            MaLoai = (int)reader["MaLoai"],
                            TenLoai = reader["TenLoai"].ToString(),
                            MaNXB = (int)reader["MaNXB"],
                            TenNXB = reader["TenNXB"].ToString(),
                            NamXuatBan = Convert.ToInt32(reader["NamXuatBan"]),
                            SoLuong = Convert.ToInt32(reader["SoLuong"]), // Thêm dòng này
                            HinhAnh = reader["HinhAnh"].ToString(),
                            SoTrang = Convert.ToInt32(reader["SoTrang"]),
                            GiaTien = Convert.ToInt32(reader["GiaTien"]),
                            TinhTrangSach = reader["TinhTrangSach"].ToString(),
                            GioiThieu = reader["GioiThieu"]?.ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void CapNhatTinhTrangSach(int maSach)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                // Lấy số lượng hiện tại
                var cmd = new SqlCommand("SELECT SoLuong FROM Sach WHERE MaSach = @MaSach", conn);
                cmd.Parameters.AddWithValue("@MaSach", maSach);
                int soLuong = (int)cmd.ExecuteScalar();

                // Cập nhật trạng thái
                string tinhTrang = soLuong > 0 ? "Còn" : "Đã hết";
                var updateCmd = new SqlCommand("UPDATE Sach SET TinhTrangSach = @TinhTrang WHERE MaSach = @MaSach", conn);
                updateCmd.Parameters.AddWithValue("@TinhTrang", tinhTrang);
                updateCmd.Parameters.AddWithValue("@MaSach", maSach);
                updateCmd.ExecuteNonQuery();
            }
        }

        public void Add(Sach sach)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Sach (TenSach, TacGia, MaLoai, MaNXB, NamXuatBan, SoLuong, HinhAnh, SoTrang, GiaTien, TinhTrangSach, GioiThieu) VALUES (@TenSach, @TacGia, @MaLoai, @MaNXB, @NamXuatBan, @SoLuong, @HinhAnh, @SoTrang, @GiaTien, @TinhTrangSach, @GioiThieu)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenSach", sach.TenSach);
                    cmd.Parameters.AddWithValue("@TacGia", sach.TacGia);
                    cmd.Parameters.AddWithValue("@MaLoai", sach.MaLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", sach.MaNXB);
                    cmd.Parameters.AddWithValue("@NamXuatBan", sach.NamXuatBan);
                    cmd.Parameters.AddWithValue("@SoLuong", sach.SoLuong);
                    cmd.Parameters.AddWithValue("@HinhAnh", sach.HinhAnh);
                    cmd.Parameters.AddWithValue("@SoTrang", sach.SoTrang);
                    cmd.Parameters.AddWithValue("@GiaTien", sach.GiaTien);
                    cmd.Parameters.AddWithValue("@TinhTrangSach", sach.TinhTrangSach);
                    cmd.Parameters.AddWithValue("@GioiThieu", sach.GioiThieu);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Sach sach)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Sach SET TenSach = @TenSach, TacGia = @TacGia, MaLoai = @MaLoai, MaNXB = @MaNXB, NamXuatBan = @NamXuatBan, SoLuong = @SoLuong, HinhAnh = @HinhAnh, SoTrang = @SoTrang, GiaTien = @GiaTien, TinhTrangSach = @TinhTrangSach, GioiThieu = @GioiThieu WHERE MaSach = @MaSach";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSach", sach.MaSach);
                    cmd.Parameters.AddWithValue("@TenSach", sach.TenSach);
                    cmd.Parameters.AddWithValue("@TacGia", sach.TacGia);
                    cmd.Parameters.AddWithValue("@MaLoai", sach.MaLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", sach.MaNXB);
                    cmd.Parameters.AddWithValue("@NamXuatBan", sach.NamXuatBan);
                    cmd.Parameters.AddWithValue("@SoLuong", sach.SoLuong);
                    cmd.Parameters.AddWithValue("@HinhAnh", sach.HinhAnh);
                    cmd.Parameters.AddWithValue("@SoTrang", sach.SoTrang);
                    cmd.Parameters.AddWithValue("@GiaTien", sach.GiaTien);
                    cmd.Parameters.AddWithValue("@TinhTrangSach", sach.TinhTrangSach);
                    cmd.Parameters.AddWithValue("@GioiThieu", sach.GioiThieu);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int maSach)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Sach WHERE MaSach = @MaSach";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSach", maSach);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
