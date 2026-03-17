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

namespace GUITV
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        TaiKhoanController taiKhoanController = new TaiKhoanController();
        public frmQuanLyTaiKhoan(string email = "")
        {
            InitializeComponent();
            txtEmail.Text = email; // Gán trực tiếp vào textbox Email
        }

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            dgvTaiKhoan.CellFormatting += dgvTaiKhoan_CellFormatting;
            LoadDanhSachTaiKhoan();
        }

        private void dgvTaiKhoan_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            // Đảm bảo e không null và ColumnIndex hợp lệ
            if (e == null || dgvTaiKhoan.Columns == null || e.ColumnIndex < 0 || e.ColumnIndex >= dgvTaiKhoan.Columns.Count)
                return;

            var column = dgvTaiKhoan.Columns[e.ColumnIndex];
            if (column != null && column.Name == "MatKhau" && e.Value != null)
            {
                int length = e.Value.ToString()!.Length;
                e.Value = new string('*', length > 0 ? length : 6); // Ẩn bằng ký tự *, số lượng tương ứng
            }
        }

        public void LoadDanhSachTaiKhoan()
        {
            dgvTaiKhoan.DataSource = taiKhoanController.LayDanhSachTaiKhoan();

            // Đổi tên cột sang tiếng Việt có dấu
            dgvTaiKhoan.Columns["MaTaiKhoan"].HeaderText = "Mã Tài Khoản";
            dgvTaiKhoan.Columns["TenDangNhap"].HeaderText = "Tên Đăng Nhập";
            dgvTaiKhoan.Columns["MatKhau"].HeaderText = "Mật Khẩu";
            dgvTaiKhoan.Columns["LoaiNguoiDung"].HeaderText = "Loại Người Dùng";
            dgvTaiKhoan.Columns["MaBanDoc"].HeaderText = "Mã Bạn Đọc";
            dgvTaiKhoan.Columns["Email"].HeaderText = "Email";
            dgvTaiKhoan.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
        }

        private void ResetForm()
        {
            txtMaTaiKhoan.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtEmail.Clear();
            txtMaBanDoc.Clear();
            txtTimKiem.Clear();
            cbLoaiNguoiDung.SelectedIndex = -1;

        }

        private void btnTimKiemSDT_Click(object sender, EventArgs e)
        {
            string sdt = txtTimKiem.Text.Trim();
            var banDocRepo = new BanDocRepository();
            string email = banDocRepo.GetEmailBySoDienThoai(sdt);
            txtEmail.Text = email ?? "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Ẩn label cảnh báo trước khi người dùng nhấn nút thêm tài khoản
            lblVuiLongNhapTDN.Visible = false;
            lblVuiLongChonLoaiNguoiDung.Visible = false;
            lblVuiLongNhapEmail.Visible = false;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                lblVuiLongNhapTDN.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(cbLoaiNguoiDung.Text))
            {
                lblVuiLongChonLoaiNguoiDung.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblVuiLongNhapEmail.Visible = true;
                hasError = true;
            }

            // Nếu có lỗi thì không thực hiện thêm tài khoản
            if (hasError)
                return;

            // Kiểm tra định dạng email
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin từ các điều khiển trên giao diện
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = "1111"; // Mật khẩu mặc định
            string loaiNguoiDung = cbLoaiNguoiDung.Text.Trim();

            try
            {
                int? maBanDoc = string.IsNullOrEmpty(txtMaBanDoc.Text.Trim()) ? (int?)null : Int32.Parse(txtMaBanDoc.Text.Trim());

                // Tạo đối tượng TaiKhoan
                TaiKhoan taiKhoan = new TaiKhoan
                {
                    TenDangNhap = tenDangNhap,
                    MatKhau = matKhau,
                    LoaiNguoiDung = loaiNguoiDung,
                    MaBanDoc = maBanDoc,
                    Email = txtEmail.Text.Trim(),
                    SoDienThoai = txtTimKiem.Text.Trim()
                };

                // Kiểm tra xem tài khoản có tồn tại trong danh sách hay không
                bool result = taiKhoanController.ThemTaiKhoan(taiKhoan);
                if (result)
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachTaiKhoan();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại!" + ex.Message, "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Một số trường không được để trống. Vui lòng kiểm tra lại!" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi không xác định", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem mã tài khoản có hợp lệ không
            if (string.IsNullOrEmpty(txtMaTaiKhoan.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin tài khoản từ các ô nhập liệu
            int maTaiKhoan;
            if (!int.TryParse(txtMaTaiKhoan.Text, out maTaiKhoan))
            {
                MessageBox.Show("Mã tài khoản không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();
            string loaiNguoiDung = cbLoaiNguoiDung.Text.Trim();
            int? maBanDoc = string.IsNullOrEmpty(txtMaBanDoc.Text.Trim()) ? (int?)null : Int32.TryParse(txtMaBanDoc.Text.Trim(), out int temp) ? temp : (int?)null;
            string email = txtEmail.Text.Trim();

            // Lưu lại tên đăng nhập cũ
            string tenDangNhapCu = "";
            if (dgvTaiKhoan.CurrentRow != null)
            {
                tenDangNhapCu = dgvTaiKhoan.CurrentRow.Cells["TenDangNhap"].Value?.ToString() ?? "";
            }
            else
            {
                tenDangNhapCu = txtTenDangNhap.Text.Trim();
            }

            // Tạo đối tượng TaiKhoan
            TaiKhoan taiKhoan = new TaiKhoan
            {
                MaTaiKhoan = maTaiKhoan,
                TenDangNhap = tenDangNhap,
                MatKhau = matKhau,
                LoaiNguoiDung = loaiNguoiDung,
                MaBanDoc = maBanDoc,
                Email = email,
                SoDienThoai = txtTimKiem.Text.Trim()
            };

            // Gọi phương thức sửa tài khoản
            bool result = taiKhoanController.CapNhatTaiKhoan(taiKhoan);
            if (result)
            {
                // Chỉ cập nhật các phiếu mượn có tên người lập phiếu đúng bằng tên đăng nhập cũ
                if (!string.IsNullOrEmpty(tenDangNhapCu) && tenDangNhapCu != taiKhoan.TenDangNhap)
                {
                    PhieuMuonRepository repo = new PhieuMuonRepository();
                    repo.CapNhatNguoiLapPhieu(tenDangNhapCu, taiKhoan.TenDangNhap);
                }

                MessageBox.Show("Sửa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachTaiKhoan();
                txtTenDangNhap.Text = taiKhoan.TenDangNhap;
                txtMatKhau.Text = taiKhoan.MatKhau;
                cbLoaiNguoiDung.Text = taiKhoan.LoaiNguoiDung;
                txtMaBanDoc.Text = taiKhoan.MaBanDoc?.ToString();
                txtEmail.Text = taiKhoan.Email;
            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTaiKhoan.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTaiKhoan;
            if (!int.TryParse(txtMaTaiKhoan.Text, out maTaiKhoan))
            {
                MessageBox.Show("Mã tài khoản không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult resultDialog = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultDialog == DialogResult.Yes)
            {
                // Gọi phương thức xóa tài khoản
                bool result = taiKhoanController.XoaTaiKhoan(maTaiKhoan);
                if (result)
                {
                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachTaiKhoan();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaTaiKhoan.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[0].Value?.ToString();
                txtTenDangNhap.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[1].Value?.ToString();
                txtMatKhau.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[2].Value?.ToString();
                cbLoaiNguoiDung.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[3].Value?.ToString();
                txtMaBanDoc.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[4].Value?.ToString();
                txtEmail.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[5].Value?.ToString();
                txtTimKiem.Text = dgvTaiKhoan.Rows[e.RowIndex].Cells[6].Value?.ToString();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string soDienThoai = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(soDienThoai))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? maBanDoc = taiKhoanController.TimMaBanDocTheoSoDienThoai(soDienThoai);

            if (maBanDoc.HasValue)
            {
                txtMaBanDoc.Text = maBanDoc.Value.ToString();

                // Lấy email và hiển thị
                string? email = taiKhoanController.TimEmailTheoSoDienThoai(soDienThoai);
                txtEmail.Text = email ?? "";

                // Tìm tài khoản đã có của bạn đọc này
                var danhSachTaiKhoan = taiKhoanController.LayDanhSachTaiKhoan();
                var taiKhoan = danhSachTaiKhoan.FirstOrDefault(x => x.MaBanDoc == maBanDoc.Value);

                if (taiKhoan != null)
                {
                    // Nếu đã có tài khoản, hiển thị thông tin thực tế
                    txtMaTaiKhoan.Text = taiKhoan.MaTaiKhoan.ToString();
                    txtMatKhau.Text = taiKhoan.MatKhau;
                    txtTenDangNhap.Text = taiKhoan.TenDangNhap;
                    cbLoaiNguoiDung.Text = taiKhoan.LoaiNguoiDung;
                }
                else
                {
                    // Nếu chưa có tài khoản, sinh mã mới và mật khẩu mặc định
                    int nextMaTaiKhoan = 1;
                    if (danhSachTaiKhoan.Any())
                    {
                        nextMaTaiKhoan = danhSachTaiKhoan.Max(x => x.MaTaiKhoan) + 1;
                    }
                    txtMaTaiKhoan.Text = nextMaTaiKhoan.ToString();
                    txtMatKhau.Text = "1111";
                    txtTenDangNhap.Clear();
                    cbLoaiNguoiDung.SelectedIndex = -1;
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy bạn đọc với số điện thoại đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Text = "";
                txtMaTaiKhoan.Clear();
                txtMatKhau.Clear();
                txtMaBanDoc.Clear();
                txtTenDangNhap.Clear();
                cbLoaiNguoiDung.SelectedIndex = -1;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}