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
using ThuVien.Repository;
using ThuVien.Utilies;

namespace ThuVien.View
{
    public partial class frmDangNhap : Form
    {
        private TaiKhoanController taiKhoanController;
        private bool isPasswordVisible = false; // Biến để theo dõi trạng thái hiển thị mật khẩu

        public frmDangNhap()
        {
            InitializeComponent();
            taiKhoanController = new TaiKhoanController();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void btnHienMatKhau_Click(object? sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtMatKhau.UseSystemPasswordChar = !isPasswordVisible;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Ẩn label cảnh báo trước khi người dùng nhấn nút đăng nhập
            lblVuiLongNhapTDN.Visible = false;
            lblVuiLongNhapMatKhau.Visible = false;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                lblVuiLongNhapTDN.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                lblVuiLongNhapMatKhau.Visible = true;
                hasError = true;
            }

            // Nếu có lỗi thì không thực hiện đăng nhập
            if (hasError)
                return;

            // Chỉ gọi đăng nhập khi đã nhập đủ
            TaiKhoanController controller = new TaiKhoanController();
            TaiKhoan? taiKhoan = controller.DangNhap(txtTenDangNhap.Text, txtMatKhau.Text);

            if (taiKhoan != null)
            {
                TaiKhoan.DangNhapHienTai = taiKhoan;
                Auth.DangNhap(taiKhoan);

                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenDangNhap.Clear();
                txtMatKhau.Clear();

                frmMain mainForm = new frmMain(taiKhoan.LoaiNguoiDung);
                this.Hide();
                mainForm.ShowDialog();
                    
                // Kiểm tra form có bị dispose chưa trước khi show
                if (!this.IsDisposed)
                {
                    this.Show();
                }
                else
                {
                    Application.Exit(); // Thoát ứng dụng nếu form đã bị dispose
                }
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblQuenMatKhau_Click(object sender, EventArgs e)
        {
            frmQuenMatKhau frm = new frmQuenMatKhau();
            frm.ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            // 1. Mã hóa mật khẩu người dùng vừa nhập
            string matKhauDaMaHoa = BaoMat.MaHoaMatKhau(txtMatKhau.Text);

            // 2. So sánh chuỗi đã mã hóa với chuỗi trong Database
            string sql = "SELECT * FROM TaiKhoan WHERE TenDangNhap='" + txtTenDangNhap.Text + "' AND MatKhau='" + matKhauDaMaHoa + "'";

            // ... thực thi câu lệnh SQL như bình thường ...
        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        { }

       
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AppState.DongFormTuFormDoiMatKhau)
            {
                Auth.DangXuat();
            }
            else
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    Auth.DangXuat();
                    // KHÔNG gọi Application.Exit() ở đây
                    // this.Close(); // Chỉ đóng form này, không dispose các form khác
                }
            }
        }
    }   }
