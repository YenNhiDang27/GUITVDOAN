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
    public class LoaiSachRepository
    {
        public LoaiSachRepository()
        {

        }

        public List<LoaiSach> GetAll()
        {
            var danhSachLoaiSach = new List<LoaiSach>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM loaisach";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSachLoaiSach.Add(new LoaiSach
                        {
                            MaLoai = reader.GetInt32(0),
                            TenLoai = reader.GetString(1)
                        });
                    }
                }
            }
            return danhSachLoaiSach;
        }

        public LoaiSach? GetById(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM loaisach WHERE maloai = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LoaiSach
                            {
                                MaLoai = reader.GetInt32(0),
                                TenLoai = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Add(LoaiSach loaiSach)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO loaisach (tenloai) VALUES (@tenloai)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tenloai", loaiSach.TenLoai);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(LoaiSach loaiSach)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE loaisach SET tenloai = @tenloai WHERE maloai = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tenloai", loaiSach.TenLoai);
                    cmd.Parameters.AddWithValue("@id", loaiSach.MaLoai);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM loaisach WHERE maloai = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

        
       

       


