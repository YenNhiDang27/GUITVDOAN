using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien.Utilies;

using ThuVien.Repository;

namespace ThuVien.View
{
    public partial class frmDoiMatKhau : Form
    {
        private bool isPasswordVisible = false; // Biến để theo dỗi trạng thái hiển thị mật khẩu
        private frmMain mainForm;
        public frmDoiMatKhau(frmMain mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void btnHienMatKhau1_Click(object? sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtMatKhauCu.UseSystemPasswordChar = !isPasswordVisible;
        }

        private void btnHienMatKhau2_Click(object? sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtMatKhauMoi.UseSystemPasswordChar = !isPasswordVisible;
        }

        private void btnHienMatKhau3_Click(object? sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtNhapLaiMatKhau.UseSystemPasswordChar = !isPasswordVisible;
        }

        private void btnXoaHet_Click(object sender, EventArgs e)
        {
            txtMatKhauCu.Clear();
            txtMatKhauMoi.Clear();
            txtNhapLaiMatKhau.Clear();
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            // Ẩn tất cả label cảnh báo trước
            lblVuiLongNhapMKC.Visible = false;
            lblVuiLongNhapMKM.Visible = false;
            lblVuiLongNhapLaiMKM.Visible = false;

            bool hasError = false;

            string matKhauCu = txtMatKhauCu.Text.Trim();
            string matKhauMoi = txtMatKhauMoi.Text.Trim();
            string matKhauNhapLai = txtNhapLaiMatKhau.Text.Trim();

            if (string.IsNullOrWhiteSpace(matKhauCu))
            {
                lblVuiLongNhapMKC.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(matKhauMoi))
            {
                lblVuiLongNhapMKM.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(matKhauNhapLai))
            {
                lblVuiLongNhapLaiMKM.Visible = true;
                hasError = true;
            }

            if (hasError)
                return;

            if (matKhauMoi != matKhauNhapLai)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu không khớp. Vui lòng thử lại.");
                return;
            }


            if (string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi) || string.IsNullOrEmpty(matKhauNhapLai))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }


            TaiKhoanRepository taiKhoanRepo = new TaiKhoanRepository();


            string tenDangNhap = Auth.TenDangNhap;

            bool isChanged = taiKhoanRepo.DoiMatKhau(tenDangNhap, matKhauCu, matKhauMoi);

            if (isChanged)
            {
                MessageBox.Show("Đổi mật khẩu thành công! Chương trình sẽ thoát ra để bạn đăng nhập lại");
                AppState.DongFormTuFormDoiMatKhau = true;

                this.Close();
                mainForm.Close();


            }
            else
            {
                MessageBox.Show("Mật khẩu cũ không đúng hoặc tài khoản không tồn tại.");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {

        }
    }
}
