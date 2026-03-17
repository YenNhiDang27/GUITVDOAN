using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ThuVien.Models;
using ThuVien.Utilies;

namespace ThuVien.Repository
{
    /// <summary>
    /// Repository quản lý lịch sử cảm xúc của bạn đọc
    /// </summary>
    public class LichSuCamXucRepository
    {
        public LichSuCamXucRepository()
        {
        }

        /// <summary>
        /// Lưu lịch sử cảm xúc của người dùng
        /// </summary>
        public int Luu(LichSuCamXuc lichSu)
        {
            try
            {
                string query = @"
                    INSERT INTO LichSuCamXuc (MaBanDoc, TrangThai, CamXucPhanTich, DoTinCay, ThoiGian, GoiYSachIds)
                    VALUES (@MaBanDoc, @TrangThai, @CamXucPhanTich, @DoTinCay, @ThoiGian, @GoiYSachIds);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBanDoc", lichSu.MaBanDoc);
                        cmd.Parameters.AddWithValue("@TrangThai", lichSu.TrangThai);
                        cmd.Parameters.AddWithValue("@CamXucPhanTich", lichSu.CamXucPhanTich);
                        cmd.Parameters.AddWithValue("@DoTinCay", lichSu.DoTinCay);
                        cmd.Parameters.AddWithValue("@ThoiGian", lichSu.ThoiGian);
                        cmd.Parameters.AddWithValue("@GoiYSachIds", lichSu.GoiYSachIds ?? "");

                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu lịch sử cảm xúc: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử cảm xúc của một bạn đọc
        /// </summary>
        public List<LichSuCamXuc> LayTheoMaBanDoc(int maBanDoc, int soLuong = 10)
        {
            try
            {
                string query = @"
                    SELECT TOP (@SoLuong) * 
                    FROM LichSuCamXuc 
                    WHERE MaBanDoc = @MaBanDoc 
                    ORDER BY ThoiGian DESC";

                var result = new List<LichSuCamXuc>();

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new LichSuCamXuc
                                {
                                    MaLichSu = Convert.ToInt32(reader["MaLichSu"]),
                                    MaBanDoc = Convert.ToInt32(reader["MaBanDoc"]),
                                    TrangThai = reader["TrangThai"].ToString(),
                                    CamXucPhanTich = reader["CamXucPhanTich"].ToString(),
                                    DoTinCay = Convert.ToDouble(reader["DoTinCay"]),
                                    ThoiGian = Convert.ToDateTime(reader["ThoiGian"]),
                                    GoiYSachIds = reader["GoiYSachIds"].ToString()
                                });
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy lịch sử cảm xúc: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thống kê cảm xúc của bạn đọc theo thời gian
        /// </summary>
        public Dictionary<string, int> ThongKeCamXuc(int maBanDoc, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            try
            {
                string query = @"
                    SELECT CamXucPhanTich, COUNT(*) as SoLan
                    FROM LichSuCamXuc
                    WHERE MaBanDoc = @MaBanDoc";

                if (tuNgay.HasValue)
                {
                    query += " AND ThoiGian >= @TuNgay";
                }

                if (denNgay.HasValue)
                {
                    query += " AND ThoiGian <= @DenNgay";
                }

                query += " GROUP BY CamXucPhanTich ORDER BY SoLan DESC";

                var result = new Dictionary<string, int>();

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);

                        if (tuNgay.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Value);
                        }

                        if (denNgay.HasValue)
                        {
                            cmd.Parameters.AddWithValue("@DenNgay", denNgay.Value);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result[reader["CamXucPhanTich"].ToString()] = Convert.ToInt32(reader["SoLan"]);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thống kê cảm xúc: {ex.Message}", ex);
            }
        }
    }
}
