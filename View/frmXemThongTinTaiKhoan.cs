using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GUITV;
using ThuVien.Models;
using ThuVien.Repository;
using ThuVien.Utilies;

namespace ThuVien.View
{
    public partial class frmXemThongTinTaiKhoan : Form
    {
        public frmXemThongTinTaiKhoan()
        {
            InitializeComponent();
        }

        private void frmXemThongTinTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadThongTinBanDoc();
        }

        private void LoadThongTinBanDoc()
        {
            if (Auth.LoaiNguoiDung == "Bạn đọc")
            {
                txtSoDienThoai.ReadOnly = true;
            }
            if (Auth.MaBanDoc == null)
            {
                MessageBox.Show("Tài khoản không liên kết với bạn đọc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BanDocRepository repo = new BanDocRepository();
            BanDoc banDoc = repo.GetBanDocById(Auth.MaBanDoc.Value)!;
            string tinhTrang = repo.GetTinhTrangMuon(Auth.MaBanDoc.Value);

            if (banDoc != null)
            {
                txtMaBanDoc.Text = banDoc.MaBanDoc.ToString();
                txtHoTen.Text = banDoc.HoTen;
                txtNgaySinh.Text = banDoc.NgaySinh.ToString("dd/MM/yyyy");
                txtDiaChi.Text = banDoc.DiaChi;
                txtNgayDangKy.Text = banDoc.NgayDangKy.ToString("dd/MM/yyyy");
                txtGioiTinh.Text = banDoc.GioiTinh ? "Nam" : "Nữ";
                txtEmail.Text = banDoc.Email;
                txtSoDienThoai.Text = banDoc.SDT;
                txtSoCCCD.Text = banDoc.CCCD;

                // Hiển thị hình ảnh nếu có
                if (!string.IsNullOrEmpty(banDoc.HinhAnh))
                {
                    string imagePath = Path.Combine(Application.StartupPath, "Images", banDoc.HinhAnh);
                    if (File.Exists(imagePath))
                    {
                        pbHinhAnh.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        pbHinhAnh.Image = null;
                    }
                }
                else
                {
                    pbHinhAnh.Image = null;
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin bạn đọc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadThongTinBanDoc();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
