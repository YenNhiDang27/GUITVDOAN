using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using ThuVien.Models;
using ThuVien.Utilies;

namespace ThuVien.Repository
{
    /// <summary>
    /// Repository quản lý gợi ý sách từ AI
    /// </summary>
    public class GoiYSachRepository
    {
        /// <summary>
        /// Lưu gợi ý sách
        /// </summary>
        public void Insert(GoiYSach goiY)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO GoiYSach (MaBanDoc, MaSach, DiemGoiY, LyDoGoiY, NgayGoiY, DaXem, DaMuon)
                                VALUES (@MaBanDoc, @MaSach, @DiemGoiY, @LyDoGoiY, @NgayGoiY, @DaXem, @DaMuon)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", goiY.MaBanDoc);
                    cmd.Parameters.AddWithValue("@MaSach", goiY.MaSach);
                    cmd.Parameters.AddWithValue("@DiemGoiY", goiY.DiemGoiY);
                    cmd.Parameters.AddWithValue("@LyDoGoiY", goiY.LyDoGoiY);
                    cmd.Parameters.AddWithValue("@NgayGoiY", goiY.NgayGoiY);
                    cmd.Parameters.AddWithValue("@DaXem", goiY.DaXem);
                    cmd.Parameters.AddWithValue("@DaMuon", goiY.DaMuon);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Lấy danh sách gợi ý cho bạn đọc
        /// </summary>
        public List<GoiYSach> GetByMaBanDoc(int maBanDoc)
        {
            List<GoiYSach> list = new List<GoiYSach>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT * FROM GoiYSach 
                                WHERE MaBanDoc = @MaBanDoc 
                                ORDER BY DiemGoiY DESC, NgayGoiY DESC";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new GoiYSach
                            {
                                MaGoiY = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                MaSach = reader.GetInt32(2),
                                DiemGoiY = reader.GetDecimal(3),
                                LyDoGoiY = reader.GetString(4),
                                NgayGoiY = reader.GetDateTime(5),
                                DaXem = reader.GetBoolean(6),
                                DaMuon = reader.GetBoolean(7)
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Đánh dấu đã xem gợi ý
        /// </summary>
        public void DanhDauDaXem(int maGoiY)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE GoiYSach SET DaXem = 1 WHERE MaGoiY = @MaGoiY";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaGoiY", maGoiY);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
