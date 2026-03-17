using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien.Controller;
using ThuVien.Models;
using ThuVien.Utilies;

namespace ThuVien
{
    public partial class frmQuenMatKhau : Form
    {
        private string currentOTP = "";
        private TaiKhoanController taiKhoanController = new TaiKhoanController();
        private bool isPasswordVisible = false; // Biến để theo dõi trạng thái hiển thị mật khẩu

        public frmQuenMatKhau()
        {
            InitializeComponent();
        }

        // Gửi mã OTP về email
        private void btnOTP_Click(object sender, EventArgs e)
        {
            lblVuiLongNhapEmail.Visible = false;

            string email = txtNhapEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                lblVuiLongNhapEmail.Visible = true;
                return;
            }

            // Tìm tài khoản theo email
            TaiKhoan tk = taiKhoanController.TimTaiKhoanTheoEmail(email);
            if (tk == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản với email này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tạo OTP
            Random rand = new Random();
            currentOTP = rand.Next(100000, 999999).ToString();

            // Gửi OTP qua email
            try
            {
                EmailService.Send(email, "Mã xác thực OTP", $"Mã xác thực của bạn là: {currentOTP}");
                MessageBox.Show("Mã xác thực đã được gửi về email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không gửi được email. Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xác nhận đổi mật khẩu
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            // Ẩn tất cả label cảnh báo trước
            lblVuiLongNhapEmail.Visible = false;
            VuiLongNhapMaOTP.Visible = false;
            lblVuiLongNhapMKM.Visible = false;
            lblVuiLongXacNhanLaiMK.Visible = false;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtNhapEmail.Text))
            {
                lblVuiLongNhapEmail.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtNhapOTP.Text))
            {
                VuiLongNhapMaOTP.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtNhapMatKhauMoi.Text))
            {
                lblVuiLongNhapMKM.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtXacNhanMatKhau.Text))
            {
                lblVuiLongXacNhanLaiMK.Visible = true;
                hasError = true;
            }

            if (hasError)
                return;

            string email = txtNhapEmail.Text.Trim();
            string otp = txtNhapOTP.Text.Trim();
            string matKhauMoi = txtNhapMatKhauMoi.Text.Trim();
            string xacNhanMatKhau = txtXacNhanMatKhau.Text.Trim();

            if (otp != currentOTP)
            {
                MessageBox.Show("Mã xác thực không đúng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(matKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (matKhauMoi != xacNhanMatKhau)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tìm tài khoản theo email
            TaiKhoan tk = taiKhoanController.TimTaiKhoanTheoEmail(email);
            if (tk == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản với email này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Đổi mật khẩu
            bool doiThanhCong = taiKhoanController.DoiMatKhau(tk.MaTaiKhoan, matKhauMoi);
            if (doiThanhCong)
            {
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Đổi mật khẩu thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnHienMatKhau1_Click(object? sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtNhapMatKhauMoi.UseSystemPasswordChar = !isPasswordVisible;
        }

        private void btnHienMatKhau2_Click(object? sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtXacNhanMatKhau.UseSystemPasswordChar = !isPasswordVisible;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmQuenMatKhau_Load(object sender, EventArgs e)
        {

        }
    }
}