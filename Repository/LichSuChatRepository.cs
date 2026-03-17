using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using ThuVien.Models;
using ThuVien.Utilies;

namespace ThuVien.Repository
{
    /// <summary>
    /// Repository quản lý lịch sử chat với AI
    /// </summary>
    public class LichSuChatRepository
    {
        /// <summary>
        /// Lưu lịch sử chat
        /// </summary>
        public void Insert(LichSuChat lichSu)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO LichSuChat (MaBanDoc, NoiDungNguoiDung, NoiDungAI, ThoiGian, LoaiYeuCau)
                                VALUES (@MaBanDoc, @NoiDungNguoiDung, @NoiDungAI, @ThoiGian, @LoaiYeuCau)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", (object)lichSu.MaBanDoc ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoiDungNguoiDung", lichSu.NoiDungNguoiDung);
                    cmd.Parameters.AddWithValue("@NoiDungAI", lichSu.NoiDungAI);
                    cmd.Parameters.AddWithValue("@ThoiGian", lichSu.ThoiGian);
                    cmd.Parameters.AddWithValue("@LoaiYeuCau", lichSu.LoaiYeuCau);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Lấy lịch sử chat của bạn đọc
        /// </summary>
        public List<LichSuChat> GetByMaBanDoc(int maBanDoc, int soLuong = 50)
        {
            List<LichSuChat> list = new List<LichSuChat>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT TOP (@SoLuong) * FROM LichSuChat 
                                WHERE MaBanDoc = @MaBanDoc 
                                ORDER BY ThoiGian DESC";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new LichSuChat
                            {
                                MaLichSu = reader.GetInt32(0),
                                MaBanDoc = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                                NoiDungNguoiDung = reader.GetString(2),
                                NoiDungAI = reader.GetString(3),
                                ThoiGian = reader.GetDateTime(4),
                                LoaiYeuCau = reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Xóa lịch sử chat cũ (quá 30 ngày)
        /// </summary>
        public void XoaLichSuCu(int soNgay = 30)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM LichSuChat WHERE ThoiGian < DATEADD(day, -@SoNgay, GETDATE())";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoNgay", soNgay);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
