using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using ThuVien.Models;
using ThuVien.Utilies;

namespace ThuVien.Repository
{
    /// <summary>
    /// Repository phân tích thói quen đọc sách
    /// </summary>
    public class ThoiQuanDocSachRepository
    {
        /// <summary>
        /// Lưu/cập nhật thói quen đọc
        /// </summary>
        public void InsertOrUpdate(ThoiQuanDocSach thoiQuan)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                
                // Kiểm tra đã tồn tại chưa
                string checkQuery = "SELECT COUNT(*) FROM ThoiQuanDocSach WHERE MaBanDoc = @MaBanDoc";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@MaBanDoc", thoiQuan.MaBanDoc);
                    int count = (int)checkCmd.ExecuteScalar();
                    
                    string query;
                    if (count > 0)
                    {
                        // Cập nhật
                        query = @"UPDATE ThoiQuanDocSach SET 
                                TheLoaiUaThich = @TheLoaiUaThich,
                                TacGiaUaThich = @TacGiaUaThich,
                                SoLuongSachDaMuon = @SoLuongSachDaMuon,
                                SoLuongSachDaDoc = @SoLuongSachDaDoc,
                                TrungBinhSoTrang = @TrungBinhSoTrang,
                                ThoiGianDocUaThich = @ThoiGianDocUaThich,
                                NgayCapNhat = @NgayCapNhat
                                WHERE MaBanDoc = @MaBanDoc";
                    }
                    else
                    {
                        // Thêm mới
                        query = @"INSERT INTO ThoiQuanDocSach 
                                (MaBanDoc, TheLoaiUaThich, TacGiaUaThich, SoLuongSachDaMuon, 
                                SoLuongSachDaDoc, TrungBinhSoTrang, ThoiGianDocUaThich, NgayCapNhat)
                                VALUES (@MaBanDoc, @TheLoaiUaThich, @TacGiaUaThich, @SoLuongSachDaMuon,
                                @SoLuongSachDaDoc, @TrungBinhSoTrang, @ThoiGianDocUaThich, @NgayCapNhat)";
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBanDoc", thoiQuan.MaBanDoc);
                        cmd.Parameters.AddWithValue("@TheLoaiUaThich", thoiQuan.TheLoaiUaThich ?? "");
                        cmd.Parameters.AddWithValue("@TacGiaUaThich", thoiQuan.TacGiaUaThich ?? "");
                        cmd.Parameters.AddWithValue("@SoLuongSachDaMuon", thoiQuan.SoLuongSachDaMuon);
                        cmd.Parameters.AddWithValue("@SoLuongSachDaDoc", thoiQuan.SoLuongSachDaDoc);
                        cmd.Parameters.AddWithValue("@TrungBinhSoTrang", thoiQuan.TrungBinhSoTrang);
                        cmd.Parameters.AddWithValue("@ThoiGianDocUaThich", thoiQuan.ThoiGianDocUaThich ?? "");
                        cmd.Parameters.AddWithValue("@NgayCapNhat", thoiQuan.NgayCapNhat);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Lấy thói quen đọc của bạn đọc
        /// </summary>
        public ThoiQuanDocSach GetByMaBanDoc(int maBanDoc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM ThoiQuanDocSach WHERE MaBanDoc = @MaBanDoc";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ThoiQuanDocSach
                            {
                                MaThoiQuan = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                TheLoaiUaThich = reader.GetString(2),
                                TacGiaUaThich = reader.GetString(3),
                                SoLuongSachDaMuon = reader.GetInt32(4),
                                SoLuongSachDaDoc = reader.GetInt32(5),
                                TrungBinhSoTrang = reader.GetDouble(6),
                                ThoiGianDocUaThich = reader.GetString(7),
                                NgayCapNhat = reader.GetDateTime(8)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Phân tích và cập nhật thói quen đọc từ lịch sử mượn sách
        /// </summary>
        public void CapNhatTuLichSuMuon(int maBanDoc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                
                // Lấy thống kê từ lịch sử mượn
                string query = @"
                    SELECT 
                        COUNT(DISTINCT pm.MaPhieuMuon) as SoLuongMuon,
                        COUNT(DISTINCT CASE WHEN pm.DaTra = 1 THEN pm.MaPhieuMuon END) as SoLuongDaDoc,
                        AVG(CAST(s.SoTrang as FLOAT)) as TrungBinhTrang,
                        STRING_AGG(DISTINCT l.TenLoai, ',') as TheLoai,
                        STRING_AGG(DISTINCT s.TacGia, ',') as TacGia
                    FROM PhieuMuon pm
                    INNER JOIN ChiTietPhieuMuon ct ON pm.MaPhieuMuon = ct.MaPhieuMuon
                    INNER JOIN Sach s ON ct.MaSach = s.MaSach
                    INNER JOIN LoaiSach l ON s.MaLoai = l.MaLoai
                    WHERE pm.MaBanDoc = @MaBanDoc";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var thoiQuan = new ThoiQuanDocSach
                            {
                                MaBanDoc = maBanDoc,
                                SoLuongSachDaMuon = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                SoLuongSachDaDoc = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                TrungBinhSoTrang = reader.IsDBNull(2) ? 0 : reader.GetDouble(2),
                                TheLoaiUaThich = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                TacGiaUaThich = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                ThoiGianDocUaThich = "",
                                NgayCapNhat = DateTime.Now
                            };
                            
                            reader.Close();
                            InsertOrUpdate(thoiQuan);
                        }
                    }
                }
            }
        }
    }
}
