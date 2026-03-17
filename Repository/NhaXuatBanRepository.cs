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
    public class NhaXuatBanRepository
    {
       

        public NhaXuatBanRepository()
        {
            
        }

        public List<NhaXuatBan> GetAll()
        {
            List<NhaXuatBan> list = new List<NhaXuatBan>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT MaNXB, TenNXB FROM NhaXuatBan";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NhaXuatBan
                        {
                            MaNXB = reader.GetInt32(0),
                            TenNXB = reader.GetString(1)
                        });
                    }
                }
            }
            return list;
        }
        public NhaXuatBan GetById(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM nhaxuatban WHERE manxb = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new NhaXuatBan
                            {
                                MaNXB = reader.GetInt32(0),
                                TenNXB = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }
        public void Add(NhaXuatBan nhaXuatBan)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO NhaXuatBan (TenNXB) VALUES (@TenNXB)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenNXB", nhaXuatBan.TenNXB);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(NhaXuatBan nhaXuatBan)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE NhaXuatBan SET TenNXB = @TenNXB WHERE MaNXB = @MaNXB";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenNXB", nhaXuatBan.TenNXB);
                    cmd.Parameters.AddWithValue("@MaNXB", nhaXuatBan.MaNXB);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int maNXB)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM NhaXuatBan WHERE MaNXB = @MaNXB";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
