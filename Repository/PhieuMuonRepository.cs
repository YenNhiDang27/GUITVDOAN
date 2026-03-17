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
    public class PhieuMuonRepository
    {

        public PhieuMuonRepository()
        {
            
        }

        public List<PhieuMuon> GetTatCaPhieuMuon()
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public bool KiemTraPhieuMuonChuaTra(int maNguoiDoc)
        {
            string query = "SELECT COUNT(*) FROM PhieuMuon WHERE MaBanDoc = @MaBanDoc AND DaTra = 0";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maNguoiDoc);
                    int soLuongPhieu = Convert.ToInt32(cmd.ExecuteScalar());

                    return soLuongPhieu > 0; // Nếu có phiếu chưa trả, trả về true
                }
            }
        }
        public List<PhieuMuon> GetPhieuMuonDaTra()
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                // Chỉ lấy các trường cần thiết, bao gồm NguoiLapPhieu
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon WHERE DaTra = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public List<PhieuMuon> GetPhieuMuonChuaTra()
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon WHERE DaTra = 0";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public List<dynamic> GetPhieuMuonSapDenHan()
        {
            List<dynamic> danhSach = new List<dynamic>();
            string query = @"
        SELECT pm.MaPhieuMuon, pm.MaBanDoc, pm.NguoiLapPhieu, bd.Email, 
               pm.NgayMuon, pm.NgayHenTra, pm.DaTra
        FROM PhieuMuon pm
        JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
        WHERE pm.DaTra = 0 
          AND DATEDIFF(DAY, GETDATE(), pm.NgayHenTra) = 2";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSach.Add(new
                        {
                            MaPhieuMuon = reader.GetInt32(0),
                            MaBanDoc = reader.GetInt32(1),
                            NguoiLapPhieu = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            NgayMuon = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                            NgayHenTra = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5),
                            DaTra = reader.IsDBNull(6) ? false : reader.GetBoolean(6)
                        });
                    }
                }
            }
            return danhSach;
        }

        public List<dynamic> GetPhieuMuonQuaHan()
        {
            List<dynamic> danhSach = new List<dynamic>();

            string query = @"
        SELECT pm.MaPhieuMuon, pm.MaBanDoc, pm.NguoiLapPhieu, bd.Email, 
               pm.NgayMuon, pm.NgayHenTra, pm.DaTra
        FROM PhieuMuon pm
        JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
        WHERE pm.DaTra = 0 and pm.NgayHenTra < GETDATE()";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NguoiLapPhieu = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                NgayMuon = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                                NgayHenTra = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5),
                                DaTra = reader.IsDBNull(6) ? false : reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }

            return danhSach;
        }

        public List<dynamic> GetChiTietPhieuMuon(int maPhieuMuon)
        {
            List<dynamic> danhSach = new List<dynamic>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"
                SELECT ct.MaChiTiet, bd.HoTen, bd.GioiTinh, bd.NgaySinh, bd.Email, bd.CCCD, bd.SDT, bd.DiaChi,
                       s.MaSach, s.TenSach, ct.SoLuong, ct.NgayTra, 
                       CASE 
                           WHEN ct.NgayTra IS NOT NULL AND ct.NgayTra > pm.NgayHenTra THEN DATEDIFF(DAY, pm.NgayHenTra, ct.NgayTra) * 5000
                           WHEN ct.NgayTra IS NULL AND GETDATE() > pm.NgayHenTra THEN DATEDIFF(DAY, pm.NgayHenTra, GETDATE()) * 5000
                           ELSE 0
                       END AS PhiPhat
                FROM ChiTietPhieuMuon ct
                INNER JOIN PhieuMuon pm ON ct.MaPhieuMuon = pm.MaPhieuMuon
                INNER JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
                INNER JOIN Sach s ON ct.MaSach = s.MaSach
                WHERE ct.MaPhieuMuon = @MaPhieuMuon";

                // Lấy trạng thái đã trả của phiếu mượn
                bool daTra = false;
                using (SqlCommand cmdCheck = new SqlCommand("SELECT DaTra FROM PhieuMuon WHERE MaPhieuMuon = @MaPhieuMuon", conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                    daTra = Convert.ToBoolean(cmdCheck.ExecuteScalar());
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new
                            {
                                MaChiTiet = reader.GetInt32(0),
                                HoTen = reader.GetString(1),
                                GioiTinh = reader.IsDBNull(2) ? false : reader.GetBoolean(2),
                                NgaySinh = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                                Email = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                CCCD = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                SDT = reader.GetString(6),
                                DiaChi = reader.GetString(7),
                                MaSach = reader.GetInt32(8),
                                TenSach = reader.GetString(9),
                                SoLuongSachMuon = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                NgayTra = daTra ? (reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11)) : null,
                                PhiPhat = reader.IsDBNull(12) ? 0 : Convert.ToDecimal(reader.GetValue(12))
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public List<PhieuMuon> GetPhieuMuonBySDT_AndTrangThai(string sdt, int? daTra, bool quaHan = false)
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                var query = new StringBuilder(@"
            SELECT pm.MaPhieuMuon, pm.MaBanDoc, pm.NgayMuon, pm.NgayHenTra, pm.DaTra, pm.NguoiLapPhieu
            FROM PhieuMuon pm
            JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
            WHERE bd.SDT = @SDT
        ");
                if (daTra.HasValue)
                    query.Append(" AND pm.DaTra = @DaTra");
                if (quaHan)
                    query.Append(" AND pm.DaTra = 0 AND pm.NgayHenTra < GETDATE()");

                SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                if (daTra.HasValue)
                    cmd.Parameters.AddWithValue("@DaTra", daTra.Value);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    danhSach.Add(new PhieuMuon
                    {
                        MaPhieuMuon = reader.GetInt32(0),
                        MaBanDoc = reader.GetInt32(1),
                        NgayMuon = reader.GetDateTime(2),
                        NgayHenTra = reader.GetDateTime(3),
                        DaTra = reader.GetBoolean(4),
                        NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                    });
                }
            }
            return danhSach;
        }

        public List<PhieuMuon> GetAll()
        {
            List<PhieuMuon> list = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PhieuMuon
                        {
                            MaPhieuMuon = reader.GetInt32(0),
                            MaBanDoc = reader.GetInt32(1),
                            NgayMuon = reader.GetDateTime(2),
                            NgayHenTra = reader.GetDateTime(3),
                            DaTra = reader.GetBoolean(4),
                            NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                        });
                    }
                }
            }
            return list;
        }

        public List<PhieuMuon> GetPhieuMuonByMaBanDocAndDateRange(int maBanDoc, DateTime tuNgay, DateTime denNgay)
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu " +
                               "FROM PhieuMuon " +
                               "WHERE MaBanDoc = @MaBanDoc AND NgayMuon BETWEEN @TuNgay AND @DenNgay";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public PhieuMuon? GetById(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM phieumuon WHERE maphieumuon = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int ThemPhieuMuon(int maBanDoc, DateTime ngayMuon, DateTime ngayHenTra, string nguoiLapPhieu)
        {
            string query = "INSERT INTO PhieuMuon (MaBanDoc, NgayMuon, NgayHenTra, NguoiLapPhieu) OUTPUT INSERTED.MaPhieuMuon VALUES (@MaBanDoc, @NgayMuon, @NgayHenTra, @NguoiLapPhieu)";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                        cmd.Parameters.AddWithValue("@NgayMuon", ngayMuon);
                        cmd.Parameters.AddWithValue("@NgayHenTra", ngayHenTra);
                        cmd.Parameters.AddWithValue("@NguoiLapPhieu", nguoiLapPhieu);

                        return (int)cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi lập phiếu mượn: " + ex.Message);
                    return -1;
                }
            }
        }

        public PhieuMuon? GetPhieuMuonBySDT(string sdt)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
        SELECT pm.MaPhieuMuon, pm.MaBanDoc, pm.NgayMuon, pm.NgayHenTra, pm.NguoiLapPhieu
        FROM PhieuMuon pm
        JOIN BanDoc bd ON pm.MaBanDoc = bd.MaBanDoc
        WHERE bd.SDT = @SDT AND pm.DaTra = 0";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new PhieuMuon
                    {
                        MaPhieuMuon = (int)reader["MaPhieuMuon"],
                        MaBanDoc = (int)reader["MaBanDoc"],
                        NgayMuon = (DateTime)reader["NgayMuon"],
                        NgayHenTra = (DateTime)reader["NgayHenTra"],
                        NguoiLapPhieu = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    };
                }
            }
            return null; // Không tìm thấy phiếu mượn
        }

        public void Update(PhieuMuon phieuMuon)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE PhieuMuon SET MaBanDoc = @MaBanDoc, NgayMuon = @NgayMuon, NgayHenTra = @NgayHenTra, DaTra = @DaTra WHERE MaPhieuMuon = @MaPhieuMuon";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", phieuMuon.MaBanDoc);
                    cmd.Parameters.AddWithValue("@NgayMuon", phieuMuon.NgayMuon);
                    cmd.Parameters.AddWithValue("@NgayHenTra", phieuMuon.NgayHenTra);
                    cmd.Parameters.AddWithValue("@DaTra", phieuMuon.DaTra);
                    cmd.Parameters.AddWithValue("@MaPhieuMuon", phieuMuon.MaPhieuMuon);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CapNhatNguoiLapPhieu(string tenDangNhapCu, string tenDangNhapMoi)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE PhieuMuon SET NguoiLapPhieu = @TenDangNhapMoi WHERE NguoiLapPhieu = @TenDangNhapCu";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDangNhapMoi", tenDangNhapMoi);
                    cmd.Parameters.AddWithValue("@TenDangNhapCu", tenDangNhapCu);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int maPhieuMuon)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM PhieuMuon WHERE MaPhieuMuon = @MaPhieuMuon";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Lấy tất cả phiếu mượn của một bạn đọc
        public List<PhieuMuon> GetPhieuMuonByMaBanDoc(int maBanDoc)
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon WHERE MaBanDoc = @MaBanDoc";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        // Lấy phiếu mượn của bạn đọc theo trạng thái đã trả/chưa trả
        public List<PhieuMuon> GetPhieuMuonByMaBanDoc_AndTrangThai(int maBanDoc, int daTra)
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon WHERE MaBanDoc = @MaBanDoc AND DaTra = @DaTra";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    cmd.Parameters.AddWithValue("@DaTra", daTra);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        // Lấy phiếu mượn quá hạn của bạn đọc
        public List<PhieuMuon> GetPhieuMuonQuaHanByMaBanDoc(int maBanDoc)
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon WHERE MaBanDoc = @MaBanDoc AND DaTra = 0 AND NgayHenTra < GETDATE()";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        // Lấy phiếu mượn của bạn đọc theo khoảng ngày
        public List<PhieuMuon> GetPhieuMuonByMaBanDoc_AndNgay(int maBanDoc, DateTime tuNgay, DateTime denNgay)
        {
            List<PhieuMuon> danhSach = new List<PhieuMuon>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaPhieuMuon, MaBanDoc, NgayMuon, NgayHenTra, DaTra, NguoiLapPhieu FROM PhieuMuon WHERE MaBanDoc = @MaBanDoc AND NgayMuon BETWEEN @TuNgay AND @DenNgay";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBanDoc", maBanDoc);
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new PhieuMuon
                            {
                                MaPhieuMuon = reader.GetInt32(0),
                                MaBanDoc = reader.GetInt32(1),
                                NgayMuon = reader.GetDateTime(2),
                                NgayHenTra = reader.GetDateTime(3),
                                DaTra = reader.GetBoolean(4),
                                NguoiLapPhieu = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public void TraSach(int maPhieuMuon)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Lấy ngày hẹn trả
                    string queryNgayHenTra = @"
                SELECT NgayHenTra FROM PhieuMuon WHERE MaPhieuMuon = @MaPhieuMuon";
                    SqlCommand cmdNgayHenTra = new SqlCommand(queryNgayHenTra, conn, transaction);
                    cmdNgayHenTra.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                    DateTime ngayHenTra = (DateTime)cmdNgayHenTra.ExecuteScalar();
                    DateTime ngayTra = DateTime.Now;

                    // Tính số ngày trễ
                    int soNgayTre = (ngayTra - ngayHenTra).Days;
                    decimal tienPhat = soNgayTre > 0 ? soNgayTre * 5000 : 0;

                    // Cập nhật NgayTra và PhiPhat trong ChiTietPhieuMuon
                    string queryCapNhatChiTiet = @"
                UPDATE ChiTietPhieuMuon
                SET NgayTra = @NgayTra, PhiPhat = @PhiPhat
                WHERE MaPhieuMuon = @MaPhieuMuon";
                    SqlCommand cmdCapNhatChiTiet = new SqlCommand(queryCapNhatChiTiet, conn, transaction);
                    cmdCapNhatChiTiet.Parameters.AddWithValue("@NgayTra", ngayTra);
                    cmdCapNhatChiTiet.Parameters.AddWithValue("@PhiPhat", tienPhat);
                    cmdCapNhatChiTiet.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                    cmdCapNhatChiTiet.ExecuteNonQuery();

                    // Cập nhật trạng thái DaTra = 1 trong PhieuMuon
                    string queryCapNhatPhieuMuon = @"
                UPDATE PhieuMuon
                SET DaTra = 1
                WHERE MaPhieuMuon = @MaPhieuMuon";
                    SqlCommand cmdCapNhatPhieuMuon = new SqlCommand(queryCapNhatPhieuMuon, conn, transaction);
                    cmdCapNhatPhieuMuon.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);
                    cmdCapNhatPhieuMuon.ExecuteNonQuery();

                    // LẤY DANH SÁCH SÁCH ĐÃ MƯỢN VÀ CẬP NHẬT LẠI SỐ LƯỢNG
                    string queryChiTiet = @"
                SELECT MaSach, SoLuong FROM ChiTietPhieuMuon WHERE MaPhieuMuon = @MaPhieuMuon";
                    SqlCommand cmdChiTiet = new SqlCommand(queryChiTiet, conn, transaction);
                    cmdChiTiet.Parameters.AddWithValue("@MaPhieuMuon", maPhieuMuon);

                    // Đọc ra danh sách tạm
                    var sachList = new List<(int MaSach, int SoLuong)>();
                    using (var reader = cmdChiTiet.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sachList.Add((reader.GetInt32(0), reader.GetInt32(1)));
                        }
                    }

                    // Đã đóng reader, giờ mới update số lượng sách
                    foreach (var item in sachList)
                    {
                        string queryUpdateSach = @"
                    UPDATE Sach SET SoLuong = SoLuong + @SoLuong WHERE MaSach = @MaSach";
                        using (var cmdUpdateSach = new SqlCommand(queryUpdateSach, conn, transaction))
                        {
                            cmdUpdateSach.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                            cmdUpdateSach.Parameters.AddWithValue("@MaSach", item.MaSach);
                            cmdUpdateSach.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}




