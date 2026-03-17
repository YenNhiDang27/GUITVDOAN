using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ThuVien.Utilies
{
    public class DatabaseHelper
    {
        private static string connectionString = "Data Source=LAPTOP-USTARUQU;Initial Catalog=quanlythuvien;Integrated Security=True;TrustServerCertificate=True";


        public static SqlConnection GetConnection()
        {
            

            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                return conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message);
                return null; 
            }
        }
    }
}
