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
    public class ChiTietPhieuMuonRepository
    {
        public ChiTietPhieuMuonRepository()
        {
          
        }
        public List<dynamic> GetChiTietPhieuMuonChiTiet(int maPhieuMuon)
        {
            List<dynamic> danhSach = new List<dynamic>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
SELECT 
    ct.MaSach, s.TenSach, s.TacGia, l.TenLoai, nxb.TenNXB, 
    s.NamXuatBan, ct.SoLuong AS SoLuong, s.HinhAnh, s.SoTrang, s.GiaTien, s.TinhTrangSach,
    pm.NgayMuon, pm.NgayHenTra
FROM ChiTietPhieuMuon ct
INNER JOIN Sach s ON ct.MaSach = s.MaSach
INNER JOIN LoaiSach l ON s.MaLoai = l.MaLoai
INNER JOIN NhaXuatBan nxb ON s.MaNXB = nxb.MaNXB
INNER JOIN PhieuMuon pm ON ct.MaPhieuMuon = pm.MaPhieuMuon
WHERE ct.MaPhieuMuon = @MaPhieuMuon";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    danhSach.Add(new
                    {
                        MaSach = reader["MaSach"],
                        TenSach = reader["TenSach"],
                        TacGia = reader["TacGia"],
                        TenLoai = reader["TenLoai"],
                        TenNXB = reader["TenNXB"],
                        NamXuatBan = reader["NamXuatBan"],
                        SoLuong = reader["SoLuong"],
                        HinhAnh = reader["HinhAnh"],
                        SoTrang = reader["SoTrang"],
                        GiaTien = reader["GiaTien"],
                        TinhTrangSach = reader["TinhTrangSach"],
                        NgayMuon = Convert.ToDateTime(reader["NgayMuon"]).ToString("dd/MM/yyyy"),
                        NgayHenTra = Convert.ToDateTime(reader["NgayHenTra"]).ToString("dd/MM/yyyy")
                    });
                }
            }
            return danhSach;
        }

        public void Update(ChiTietPhieuMuon chiTiet)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE chitietphieumuon SET maphieumuon = @maphieumuon, masach = @masach, ngaytra = @ngaytra, phiphat = @phiphat WHERE machitiet = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maphieumuon", chiTiet.MaPhieuMuon);
                    cmd.Parameters.AddWithValue("@masach", chiTiet.MaSach);
                    cmd.Parameters.AddWithValue("@ngaytra", chiTiet.NgayTra.HasValue ? chiTiet.NgayTra.Value : DBNull.Value); cmd.Parameters.AddWithValue("@phiphat", chiTiet.PhiPhat);
                    cmd.Parameters.AddWithValue("@id", chiTiet.MaChiTiet);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ThemChiTietPhieuMuon(int maPhieuMuon, int maSach, int soLuong)
        {
            using (SqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                try
                {
                    var command = new SqlCommand(
                        "INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, SoLuong) VALUES (@MaPhieuMuon, @MaSach, @SoLuong)",
                        connection);
                    command.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                    command.Parameters.AddWithValue("@MaSach", maSach);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi thêm chi tiết phiếu mượn: " + ex.Message);
                    throw;
                }
            }
        }

        public void ThemChiTiet(ChiTietPhieuMuon chiTiet)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, NgayTra, PhiPhat) VALUES (@MaPhieuMuon, @MaSach, @NgayTra, @PhiPhat)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieuMuon", chiTiet.MaPhieuMuon);
                cmd.Parameters.AddWithValue("@MaSach", chiTiet.MaSach);
                cmd.Parameters.AddWithValue("@NgayTra", chiTiet.NgayTra.HasValue ? chiTiet.NgayTra.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@PhiPhat", chiTiet.PhiPhat);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public ChiTietPhieuMuon? GetById(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM chitietphieumuon WHERE machitiet = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ChiTietPhieuMuon
                            {
                                MaChiTiet = reader.GetInt32(0),
                                MaPhieuMuon = reader.GetInt32(1),
                                MaSach = reader.GetInt32(2),
                                NgayTra = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                PhiPhat = reader.GetDecimal(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void XoaChiTiet(int maChiTiet)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM ChiTietPhieuMuon WHERE MaChiTiet = @MaChiTiet";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaChiTiet", maChiTiet);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<ChiTietPhieuMuon> LayDanhSach()
        {
            List<ChiTietPhieuMuon> danhSach = new List<ChiTietPhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM ChiTietPhieuMuon";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSach.Add(new ChiTietPhieuMuon
                        {
                            MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]),
                            MaPhieuMuon = Convert.ToInt32(reader["MaPhieuMuon"]),
                            MaSach = Convert.ToInt32(reader["MaSach"]),
                            NgayTra = reader["NgayTra"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayTra"]),
                            PhiPhat = Convert.ToDecimal(reader["PhiPhat"])
                        });
                    }
                }
            }
            return danhSach;
        }
        public List<ChiTietPhieuMuon> GetChiTietPhieuMuon(int maPhieuMuon)
        {
            List<ChiTietPhieuMuon> danhSach = new List<ChiTietPhieuMuon>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
            SELECT MaChiTiet, MaPhieuMuon, MaSach, NgayTra, PhiPhat
            FROM ChiTietPhieuMuon
            WHERE MaPhieuMuon = @MaPhieuMuon";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    danhSach.Add(new ChiTietPhieuMuon
                    {
                        MaChiTiet = (int)reader["MaChiTiet"],
                        MaPhieuMuon = (int)reader["MaPhieuMuon"],
                        MaSach = (int)reader["MaSach"],
                        NgayTra = reader["NgayTra"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["NgayTra"],
                        PhiPhat = (decimal)reader["PhiPhat"]
                    });
                }
            }
            return danhSach;
        }


    }

}
