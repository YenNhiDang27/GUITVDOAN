using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using ThuVien.Controller;
using ThuVien.Models;
using ThuVien.Repository;
using ThuVien.View;

namespace GUITV
{
    public partial class frmQuanLyPhieuMuon : Form
    {
        private BanDoc banDoc = new BanDoc();
        private PhieuMuonRepository phieuMuonRepository;
        private PhieuMuonController phieuMuonController;
        private bool kt = false;
        private ChiTietPhieuMuonRepository chiTietPhieuMuonRepository;
        private bool daDongYLapPhieuMoi = false;
        private BindingList<dynamic> danhSachSachMuon = new BindingList<dynamic>();

        public frmQuanLyPhieuMuon()
        {
            InitializeComponent();
            phieuMuonRepository = new PhieuMuonRepository();
            phieuMuonController = new PhieuMuonController(phieuMuonRepository);
            chiTietPhieuMuonRepository = new ChiTietPhieuMuonRepository();
            TaiKhoan tk = new TaiKhoan
            {
                MaTaiKhoan = 1,
                TenDangNhap = "admin",
                MatKhau = "admin",
                LoaiNguoiDung = "Admin",
                Email = "admin@yourdomain.com",
                SoDienThoai = "0123456789"
            };

            // Sử dụng biến kt để tránh cảnh báo
            var _ = kt;
        }

        private void frmPhieuMuon_Load(object sender, EventArgs e)
        {
            dgvDanhSachMuon.Columns.Clear();

            dtpNgayLapPhieu.Value = DateTime.Now; // Gán ngày hiện tại
            dtpNgayLapPhieu.Enabled = false;      // Vô hiệu hóa chỉnh sửa

            // Nạp danh sách admin vào ComboBox
            cbNguoiLapPhieu.Items.Clear();
            var taiKhoanRepo = new TaiKhoanRepository();
            var danhSachAdmin = taiKhoanRepo.GetAll().Where(tk => tk.LoaiNguoiDung == "Admin").ToList();
            foreach (var admin in danhSachAdmin)
            {
                cbNguoiLapPhieu.Items.Add(admin.TenDangNhap);
            }

        }

        private void LoadDanhSachMuon(int maPhieuMuon)
        {
            var danhSach = chiTietPhieuMuonRepository.GetChiTietPhieuMuonChiTiet(maPhieuMuon);
            dgvDanhSachMuon.DataSource = danhSach;
            DoiTenCotDanhSachMuon();

            // Định dạng cột Giá Tiền
            if (dgvDanhSachMuon.Columns["GiaTien"] != null)
            {
                dgvDanhSachMuon.Columns["GiaTien"].DefaultCellStyle.Format = "#,##0 'VNĐ'";
            }
        }

        private void DoiTenCotDanhSachMuon()
        {
            if (dgvDanhSachMuon.Columns["MaSach"] != null)
                dgvDanhSachMuon.Columns["MaSach"].HeaderText = "Mã Sách";
            if (dgvDanhSachMuon.Columns["TenSach"] != null)
                dgvDanhSachMuon.Columns["TenSach"].HeaderText = "Tên Sách";
            if (dgvDanhSachMuon.Columns["TacGia"] != null)
                dgvDanhSachMuon.Columns["TacGia"].HeaderText = "Tác Giả";
            if (dgvDanhSachMuon.Columns["TenLoai"] != null)
                dgvDanhSachMuon.Columns["TenLoai"].HeaderText = "Thể loại";
            if (dgvDanhSachMuon.Columns["TenNXB"] != null)
                dgvDanhSachMuon.Columns["TenNXB"].HeaderText = "Nhà xuất bản";
            if (dgvDanhSachMuon.Columns["NamXuatBan"] != null)
                dgvDanhSachMuon.Columns["NamXuatBan"].HeaderText = "Năm Xuất Bản";
            if (dgvDanhSachMuon.Columns["SoLuong"] != null)
                dgvDanhSachMuon.Columns["SoLuong"].HeaderText = "Số Lượng";
            if (dgvDanhSachMuon.Columns["HinhAnh"] != null)
                dgvDanhSachMuon.Columns["HinhAnh"].HeaderText = "Hình Ảnh";
            if (dgvDanhSachMuon.Columns["SoTrang"] != null)
                dgvDanhSachMuon.Columns["SoTrang"].HeaderText = "Số Trang";
            if (dgvDanhSachMuon.Columns["GiaTien"] != null)
                dgvDanhSachMuon.Columns["GiaTien"].HeaderText = "Giá Tiền";
            if (dgvDanhSachMuon.Columns["TinhTrangSach"] != null)
                dgvDanhSachMuon.Columns["TinhTrangSach"].HeaderText = "Tình Trạng";
            if (dgvDanhSachMuon.Columns["NgayMuon"] != null)
                dgvDanhSachMuon.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
            if (dgvDanhSachMuon.Columns["NgayHenTra"] != null)
                dgvDanhSachMuon.Columns["NgayHenTra"].HeaderText = "Ngày Hẹn Trả";
        }

        private void btnTimBanDoc_Click(object sender, EventArgs e)
        {
            string sdt = txtSoDienThoai.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BanDocRepository repo = new BanDocRepository();
            banDoc = repo.GetBanDocBySDT(sdt)!;

            if (banDoc != null)
            {
                txtHoVaTen.Text = banDoc?.HoTen ?? "";
                txtDiaChi.Text = banDoc?.DiaChi ?? "";
                txtEmail.Text = banDoc?.Email ?? "";
                txtSoCCCD.Text = banDoc?.CCCD ?? "";
                dtpNgaySinh.Value = (banDoc != null && banDoc.NgaySinh >= dtpNgaySinh.MinDate && banDoc.NgaySinh <= dtpNgaySinh.MaxDate) ? banDoc.NgaySinh : DateTime.Now;

                // Gán giới tính
                rbNam.Checked = banDoc != null && banDoc.GioiTinh;
                rbNu.Checked = banDoc != null && !banDoc.GioiTinh;

                // Hiển thị mã bạn đọc
                txtSTTPhieuMuon.Text = banDoc != null ? banDoc.MaBanDoc.ToString() : "";

                // Hiển thị ngày đăng ký
                dtpNgayLapPhieu.Value = (banDoc != null && banDoc.NgayDangKy > dtpNgayLapPhieu.MinDate && banDoc.NgayDangKy < dtpNgayLapPhieu.MaxDate) ? banDoc.NgayDangKy : DateTime.Now;

                // --- Hiển thị thông tin phiếu mượn chưa trả ---
                var phieuChuaTraMoiNhat = (banDoc != null) ? phieuMuonRepository.GetPhieuMuonChuaTra().Where(pm => pm.MaBanDoc == banDoc.MaBanDoc).OrderByDescending(pm => pm.NgayMuon).FirstOrDefault() : null;
                if (phieuChuaTraMoiNhat != null)
                {
                    txtSTTPhieuMuon.Text = phieuChuaTraMoiNhat.MaPhieuMuon.ToString();

                    // Hiển thị người lập phiếu
                    var nguoiLapPhieu = phieuChuaTraMoiNhat.NguoiLapPhieu;
                    cbNguoiLapPhieu.SelectedIndex = -1; // Xóa chọn trước

                    if (!string.IsNullOrEmpty(nguoiLapPhieu))
                    {
                        int index = -1;
                        for (int i = 0; i < cbNguoiLapPhieu.Items.Count; i++)
                        {
                            var item = cbNguoiLapPhieu.Items[i];
                            if ((item?.ToString() ?? "").Trim().Equals(nguoiLapPhieu?.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                index = i;
                                break;
                            }
                        }

                        if (index == -1)
                        {
                            if (!string.IsNullOrEmpty(nguoiLapPhieu))
                            {
                                cbNguoiLapPhieu.Items.Add(nguoiLapPhieu);
                                index = cbNguoiLapPhieu.Items.Count - 1;
                            }

                            index = cbNguoiLapPhieu.Items.Count - 1;
                        }
                        cbNguoiLapPhieu.SelectedIndex = index;
                    }
                    else
                    {
                        cbNguoiLapPhieu.SelectedIndex = -1;
                    }

                    dtpNgayLapPhieu.Value = phieuChuaTraMoiNhat.NgayMuon;

                    // Hiển thị danh sách sách đã mượn lên DataGridView
                    var danhSach = chiTietPhieuMuonRepository.GetChiTietPhieuMuonChiTiet(phieuChuaTraMoiNhat.MaPhieuMuon);
                    dgvDanhSachMuon.Columns.Clear();
                    dgvDanhSachMuon.AutoGenerateColumns = true;
                    dgvDanhSachMuon.DataSource = danhSach;
                    DoiTenCotDanhSachMuon();
                    kt = true;
                }
                else
                {
                    // Không có phiếu mượn chưa trả, xóa trắng thông tin phiếu mượn
                    txtSTTPhieuMuon.Clear();
                    cbNguoiLapPhieu.SelectedIndex = -1;
                    dtpNgayLapPhieu.Value = DateTime.Now;
                    dgvDanhSachMuon.DataSource = null;
                    dgvDanhSachMuon.Columns.Clear();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy bạn đọc với số điện thoại này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHoVaTen.Clear();
                txtDiaChi.Clear();
                txtEmail.Clear();
                dtpNgaySinh.Value = DateTime.Now;
                rbNam.Checked = false;
                rbNu.Checked = false;

                // Xóa trắng thông tin phiếu mượn
                txtSTTPhieuMuon.Clear();
                cbNguoiLapPhieu.SelectedIndex = -1;
                dtpNgayLapPhieu.Value = DateTime.Now;
                dgvDanhSachMuon.DataSource = null;
                dgvDanhSachMuon.Columns.Clear();
            }

            // Kiểm tra cảnh báo phiếu mượn chưa trả
            if (banDoc != null && phieuMuonRepository.KiemTraPhieuMuonChuaTra(banDoc.MaBanDoc))
            {
                var result = MessageBox.Show(
                    "Bạn đọc có phiếu mượn chưa hoàn trả, bạn có muốn cho bạn đọc này mượn sách tiếp không?",
                    "Cảnh báo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    // Xóa thông tin để tạo phiếu mới
                    txtSTTPhieuMuon.Clear();
                    cbNguoiLapPhieu.SelectedIndex = -1;
                    dtpNgayLapPhieu.Value = DateTime.Now;
                    txtMaSach.Clear();
                    txtSoLuong.Clear();
                    dgvDanhSachMuon.DataSource = null;
                    dgvDanhSachMuon.Rows.Clear();
                    dgvDanhSachMuon.Columns.Clear();
                    daDongYLapPhieuMoi = true; // Đã đồng ý lập phiếu mới
                }
                else if (result == DialogResult.No || result == DialogResult.Cancel)
                {
                    daDongYLapPhieuMoi = false;
                    return;
                }
            }
            else
            {
                daDongYLapPhieuMoi = false;
            }

            // Sửa trong btnThemSach_Click:
            if (banDoc == null)
            {
                MessageBox.Show("Chưa có thông tin bạn đọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNguoiDoc = banDoc.MaBanDoc;
            if (phieuMuonRepository.KiemTraPhieuMuonChuaTra(maNguoiDoc) && !daDongYLapPhieuMoi)
            {
                var result = MessageBox.Show(
                    "Bạn đọc có phiếu mượn chưa hoàn trả, bạn có muốn cho bạn đọc này mượn sách tiếp không?\n\n" +
                    "Nhấn 'Yes' để tiếp tục mượn.\nNhấn 'No' để hủy.\nNhấn 'Cancel' để thoát.",
                    "Cảnh báo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No || result == DialogResult.Cancel)
                {
                    return;
                }
                // Nếu Yes thì cho phép mượn tiếp
            }
        }

        private void btnThemSach_Click(object sender, EventArgs e)
        {
            // Kiểm tra người lập phiếu đã được chọn chưa
            if (cbNguoiLapPhieu.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn người lập phiếu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            int maNguoiDoc = banDoc.MaBanDoc;

            // Tính tổng số lượng sách đã chọn
            int tongSoLuong = danhSachSachMuon.Sum(s => (int)s.SoLuong);

            // Cộng thêm số lượng sắp thêm
            int soLuongMoi;
            if (!int.TryParse(txtSoLuong.Text, out soLuongMoi) || soLuongMoi <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            if (tongSoLuong + soLuongMoi > 5)
            {
                MessageBox.Show("Tổng số lượng sách mượn không được vượt quá 5!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maSach;
            int soLuong;
            if (!int.TryParse(txtMaSach.Text.Trim(), out maSach))
            {
                MessageBox.Show("Mã sách không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!int.TryParse(txtSoLuong.Text, out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            // Lấy thông tin sách
            SachRepository sachRepository = new SachRepository();
            Sach? sach = sachRepository.GetById(maSach);

            // Kiểm tra null trước
            if (sach == null)
            {
                MessageBox.Show("Không tìm thấy sách!");
                return;
            }

            // Kiểm tra hết sách
            if (sach.SoLuong == 0)
            {
                MessageBox.Show("Sách này đã hết, không thể mượn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sach == null)
            {
                MessageBox.Show("Không tìm thấy sách!");
                return;
            }
            // Nếu chưa xác nhận lập phiếu mới thì mới kiểm tra trùng sách
            if (!daDongYLapPhieuMoi && danhSachSachMuon.Any(s => s.MaSach == sach.MaSach))
            {
                MessageBox.Show("Sách này đã có trong danh sách mượn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSach.Clear();
                txtSoLuong.Clear();
                return;
            }

            DateTime ngayMuon = DateTime.Now;
            DateTime ngayHenTra = ngayMuon.AddDays(5);
            if (ngayHenTra.DayOfWeek == DayOfWeek.Sunday)
            {
                ngayHenTra = ngayHenTra.AddDays(1);
            }

            // Thêm sách vào danh sách BindingList
            danhSachSachMuon.Add(new
            {
                MaSach = sach.MaSach,
                TenSach = sach.TenSach,
                TacGia = sach.TacGia,
                TenLoai = sach.TenLoai,
                TenNXB = sach.TenNXB,
                NamXuatBan = sach.NamXuatBan,
                SoLuong = soLuong,
                HinhAnh = sach.HinhAnh,
                SoTrang = sach.SoTrang,
                GiaTien = sach.GiaTien,
                TinhTrangSach = sach.TinhTrangSach,
                NgayMuon = ngayMuon.ToString("dd/MM/yyyy"),
                NgayHenTra = ngayHenTra.ToString("dd/MM/yyyy")
            });

            // Gán lại DataSource nếu chưa gán
            if (dgvDanhSachMuon.DataSource == null)
            {
                dgvDanhSachMuon.DataSource = danhSachSachMuon;
            }

            txtMaSach.Clear();
            txtSoLuong.Clear();
           
        }

        private void btnXoaSach_Click(object sender, EventArgs e)
        {
            if (dgvDanhSachMuon.CurrentRow != null)
            {
                int index = dgvDanhSachMuon.CurrentRow.Index;
                if (index >= 0 && index < danhSachSachMuon.Count)
                {
                    danhSachSachMuon.RemoveAt(index);
                    txtMaSach.Clear();
                    txtSoLuong.Clear();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLuuPhieu_Click(object sender, EventArgs e)
        {
            // Kiểm tra phiếu mượn trống
            if (dgvDanhSachMuon.Rows.Count == 0)
            {
                MessageBox.Show("Phiếu mượn trống! Vui lòng thêm sách vào phiếu mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maBanDoc = banDoc.MaBanDoc;
            DateTime ngayMuon = DateTime.Now;
            DateTime ngayHenTra = ngayMuon.AddDays(7);

            if (ngayHenTra.DayOfWeek == DayOfWeek.Sunday)
            {
                ngayHenTra = ngayHenTra.AddDays(1);
            }

            // Lấy người lập phiếu từ ComboBox
            string nguoiLapPhieu = cbNguoiLapPhieu.SelectedItem?.ToString() ?? "";

            // Truyền thêm tham số người lập phiếu
            int maPhieuMuon = phieuMuonRepository.ThemPhieuMuon(maBanDoc, ngayMuon, ngayHenTra, nguoiLapPhieu);

            // Kiểm tra xem đã lưu phiếu mượn thành công chưa
            ChiTietPhieuMuonRepository chiTietPhieuMuonRepository = new ChiTietPhieuMuonRepository();
            foreach (DataGridViewRow row in dgvDanhSachMuon.Rows)
            {
                if (row.Cells["MaSach"].Value != null && row.Cells["SoLuong"].Value != null)
                {
                    int maSach = Convert.ToInt32(row.Cells["MaSach"].Value);
                    int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    chiTietPhieuMuonRepository.ThemChiTietPhieuMuon(maPhieuMuon, maSach, soLuong);

                    // Cập nhật số lượng sách trong kho
                    SachRepository sachRepository = new SachRepository();
                    var sach = sachRepository.GetById(maSach);
                    if (sach != null)
                    {
                        sach.SoLuong -= soLuong;
                        if (sach.SoLuong < 0) sach.SoLuong = 0;
                        sachRepository.Update(sach);
                        sachRepository.CapNhatTinhTrangSach(maSach);
                    }
                }
            }

            // Thêm đoạn này để nạp lại chi tiết phiếu mượn lên DataGridView
            var danhSachChiTiet = chiTietPhieuMuonRepository.GetChiTietPhieuMuonChiTiet(maPhieuMuon);
            dgvDanhSachMuon.DataSource = danhSachChiTiet;
            DoiTenCotDanhSachMuon();

            MessageBox.Show("Lưu phiếu mượn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            if (dgvDanhSachMuon.Rows.Count == 0)
            {
                MessageBox.Show("Không có sách nào để trả.");
                return;
            }

            int maPhieuMuon;
            if (!int.TryParse(txtSTTPhieuMuon.Text.Trim(), out maPhieuMuon))
            {
                MessageBox.Show("Mã phiếu mượn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Cập nhật dữ liệu trong database
                phieuMuonRepository.TraSach(maPhieuMuon);

                SachRepository sachRepository = new SachRepository();
                var danhSachChiTiet = chiTietPhieuMuonRepository.GetChiTietPhieuMuon(maPhieuMuon);
                foreach (var chiTiet in danhSachChiTiet)
                {
                    var sach = sachRepository.GetById(chiTiet.MaSach);
                    if (sach != null)
                    {
                        sach.SoLuong += chiTiet.SoLuong;
                        sachRepository.Update(sach);
                        sachRepository.CapNhatTinhTrangSach(chiTiet.MaSach);
                    }
                }

                // Load lại dữ liệu lên DataGridView
                List<ChiTietPhieuMuon> danhSach = chiTietPhieuMuonRepository.GetChiTietPhieuMuon(maPhieuMuon);
                dgvDanhSachMuon.DataSource = danhSach;

                MessageBox.Show("Trả sách thành công!");
                dgvDanhSachMuon.DataSource = null;
                dgvDanhSachMuon.Columns.Clear();
                kt = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi trả sách: " + ex.Message);
            }
        }

        private void btnThemBanDoc_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmQuanLyBanDoc frm = new frmQuanLyBanDoc();
            frm.ShowDialog();
            this.Show();
        }

        private void SetThongTinBanDocEnabled(bool enabled)
        {
            txtHoVaTen.Enabled = enabled;
            dtpNgaySinh.Enabled = enabled;
            rbNam.Enabled = enabled;
            rbNu.Enabled = enabled;
            txtDiaChi.Enabled = enabled;
            txtEmail.Enabled = enabled;
        }

        private void dgvDanhSachMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra chỉ số dòng hợp lệ
            if (e.RowIndex >= 0 && dgvDanhSachMuon.Rows[e.RowIndex].Cells["MaSach"].Value != null)
            {
                txtMaSach.Text = dgvDanhSachMuon.Rows[e.RowIndex].Cells["MaSach"].Value.ToString();
                // Thêm dòng này để hiện số lượng
                txtSoLuong.Text = dgvDanhSachMuon.Rows[e.RowIndex].Cells["SoLuong"].Value?.ToString();
            }
        }

        private void btnKiemTraPhieuMuon_Click(object sender, EventArgs e)
        {
            frmThongKe frm = new frmThongKe();
            frm.ShowDialog();
        }

        private void ResetForm()
        {
            // Xóa danh sách tạm và DataGridView
            danhSachSachMuon.Clear();
            dgvDanhSachMuon.DataSource = null;

            // Xóa dữ liệu các textbox
            txtSTTPhieuMuon.Clear();
            cbNguoiLapPhieu.SelectedIndex = -1; // Giả sử bạn có một ComboBox cho người lập phiếu   
            txtMaSach.Clear();
            txtSoLuong.Clear();
            txtSoDienThoai.Clear();
            txtHoVaTen.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            rbNam.Checked = false;
            rbNu.Checked = false;
            txtDiaChi.Clear();
            txtSoCCCD.Clear();
            txtEmail.Clear();

            // Đặt lại ngày lập phiếu về ngày hiện tại
            dtpNgayLapPhieu.Value = DateTime.Now;

            // Xóa dữ liệu DataGridView
            dgvDanhSachMuon.DataSource = null;
            dgvDanhSachMuon.Rows.Clear();
            dgvDanhSachMuon.Columns.Clear();

            // Đặt lại trạng thái biến nếu cần
            kt = false;
            banDoc = new BanDoc();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnInPhieu_Click(object sender, EventArgs e)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += PrintPhieuMuon_PrintPage;
            PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = printDoc,
                Width = 800,
                Height = 600
            };
            preview.ShowDialog();
        }

        private void PrintPhieuMuon_PrintPage(object sender, PrintPageEventArgs e)
        {
            int pageWidth = e.PageBounds.Width;
            int x = 50, y = 50;
            int lineHeight = 30;

            // Font Times New Roman
            Font titleFont = new Font("Times New Roman", 16, FontStyle.Bold);
            Font labelFont = new Font("Times New Roman", 12, FontStyle.Bold);
            Font normalFont = new Font("Times New Roman", 12);

            // Tiêu đề của phiếu mượn
            string title = "PHIẾU MƯỢN SÁCH";
            SizeF titleSize = e.Graphics!.MeasureString(title, titleFont);
            float titleX = (pageWidth - titleSize.Width) / 2;
            e.Graphics.DrawString(title, titleFont, Brushes.Black, titleX, y);
            y += lineHeight + 10;

            // Mã phiếu mượn và người lập phiếu
            string maPhieu = "Mã phiếu mượn: " + txtSTTPhieuMuon.Text;
            string nguoiLapPhieu = "Người lập phiếu: " + (cbNguoiLapPhieu.SelectedItem?.ToString() ?? "");
            int infoX = x;

            e.Graphics.DrawString(maPhieu, labelFont, Brushes.Black, infoX, y);
            y += lineHeight;
            e.Graphics.DrawString(nguoiLapPhieu, labelFont, Brushes.Black, infoX, y);

            // Thông tin cá nhân
            y += lineHeight;
            e.Graphics.DrawString("Họ và tên: ", labelFont, Brushes.Black, x, y);
            e.Graphics.DrawString(txtHoVaTen.Text, normalFont, Brushes.Black, x + 110, y);
            y += lineHeight;
            e.Graphics.DrawString("Địa chỉ: ", labelFont, Brushes.Black, x, y);
            e.Graphics.DrawString(txtDiaChi.Text, normalFont, Brushes.Black, x + 110, y);
            y += lineHeight;
            e.Graphics.DrawString("Email: ", labelFont, Brushes.Black, x, y);
            e.Graphics.DrawString(txtEmail.Text, normalFont, Brushes.Black, x + 110, y);
            y += lineHeight;

            // Cách 2 dòng trước khi vẽ ngày mượn/ngày trả
            y += lineHeight * 2;

            // Tính vị trí bảng
            int tableX = x, tableY = y;
            int[] colWidths = { 40, 80, 200, 80, 100, 150, 110 };
            string[] headers = { "STT", "Mã sách", "Tên sách", "Số lượng", "Thể loại", "Nhà xuất bản", "Ghi chú" };

            // Ngày mượn và ngày hẹn trả
            int rightTableX = tableX + colWidths.Sum();
            int infoY = tableY - lineHeight * 2;

            string ngayMuon = "Ngày mượn: " + dtpNgayLapPhieu.Value.ToString("dd/MM/yyyy");
            string ngayHenTra = "Ngày hẹn trả: " + dtpNgayLapPhieu.Value.AddDays(7).ToString("dd/MM/yyyy");

            SizeF ngayMuonSize = e.Graphics.MeasureString(ngayMuon, labelFont);
            SizeF ngayHenTraSize = e.Graphics.MeasureString(ngayHenTra, labelFont);

            e.Graphics.DrawString(ngayMuon, labelFont, Brushes.Black, rightTableX - ngayMuonSize.Width, infoY);
            e.Graphics.DrawString(ngayHenTra, labelFont, Brushes.Black, rightTableX - ngayHenTraSize.Width, infoY + lineHeight);

            // Header của bảng
            int colX = tableX;
            for (int i = 0; i < headers.Length; i++)
            {
                e.Graphics.DrawRectangle(Pens.Black, colX, tableY, colWidths[i], lineHeight);
                SizeF headerSize = e.Graphics.MeasureString(headers[i], labelFont);
                float headerCenterX = colX + (colWidths[i] - headerSize.Width) / 2;
                e.Graphics.DrawString(headers[i], labelFont, Brushes.Black, headerCenterX, tableY + 5);
                colX += colWidths[i];
            }
            tableY += lineHeight;

            // Dữ liệu sách
            for (int i = 0; i < dgvDanhSachMuon.Rows.Count; i++)
            {
                var row = dgvDanhSachMuon.Rows[i];
                if (row.IsNewRow) continue;

                colX = tableX;
                string[] cellValues = {
                (i + 1).ToString(),
                row.Cells["MaSach"]?.Value?.ToString() ?? "",
                row.Cells["TenSach"]?.Value?.ToString() ?? "",
                row.Cells["SoLuong"]?.Value?.ToString() ?? "",
                row.Cells["TenLoai"]?.Value?.ToString() ?? "",
                row.Cells["TenNXB"]?.Value?.ToString() ?? "",
                "" // Ghi chú
                };

                for (int j = 0; j < cellValues.Length; j++)
                {
                    e.Graphics.DrawRectangle(Pens.Black, colX, tableY, colWidths[j], lineHeight);
                    string cellText = cellValues[j] ?? "";
                    SizeF cellSize = e.Graphics.MeasureString(cellText, normalFont);

                    float cellY = tableY + (lineHeight - cellSize.Height) / 2;
                    float cellXDraw;
                    if (j == 2) // Tên sách căn trái
                    {
                        cellXDraw = colX + 4;
                    }
                    else // Các cột khác căn giữa
                    {
                        cellXDraw = colX + (colWidths[j] - cellSize.Width) / 2;
                    }
                    e.Graphics.DrawString(cellText, normalFont, Brushes.Black, cellXDraw, cellY);
                    colX += colWidths[j];
                }
                tableY += lineHeight;
            }
        }

        private void btnTimPhieu_Click(object sender, EventArgs e)
        {
            // Lấy mã phiếu mượn từ textbox
            if (!int.TryParse(txtSTTPhieuMuon.Text.Trim(), out int maPhieuMuon))
            {
                MessageBox.Show("Vui lòng nhập mã phiếu mượn hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin phiếu mượn
            var phieu = phieuMuonRepository.GetById(maPhieuMuon);
            if (phieu == null)
            {
                MessageBox.Show("Không tìm thấy phiếu mượn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hiển thị thông tin phiếu mượn
            // Người lập phiếu
            cbNguoiLapPhieu.SelectedIndex = -1;
            for (int i = 0; i < cbNguoiLapPhieu.Items.Count; i++)
            {
                if ((cbNguoiLapPhieu.Items[i]?.ToString() ?? "").Trim().Equals(phieu.NguoiLapPhieu?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    cbNguoiLapPhieu.SelectedIndex = i;
                    break;
                }
            }
            // Nếu không có trong danh sách thì thêm vào
            if (cbNguoiLapPhieu.SelectedIndex == -1 && !string.IsNullOrEmpty(phieu.NguoiLapPhieu))
            {
                cbNguoiLapPhieu.Items.Add(phieu.NguoiLapPhieu);
                cbNguoiLapPhieu.SelectedIndex = cbNguoiLapPhieu.Items.Count - 1;
            }
            dtpNgayLapPhieu.Value = phieu.NgayMuon;

            // Lấy thông tin bạn đọc
            BanDocRepository repo = new BanDocRepository();
            var banDoc = repo.GetBanDocById(phieu.MaBanDoc);
            if (banDoc != null)
            {
                txtSoDienThoai.Text = banDoc.SDT;
                txtHoVaTen.Text = banDoc.HoTen;
                dtpNgaySinh.Value = (banDoc.NgaySinh >= dtpNgaySinh.MinDate && banDoc.NgaySinh <= dtpNgaySinh.MaxDate) ? banDoc.NgaySinh : DateTime.Now;
                rbNam.Checked = banDoc.GioiTinh;
                rbNu.Checked = !banDoc.GioiTinh;
                txtDiaChi.Text = banDoc.DiaChi;
                txtEmail.Text = banDoc.Email;
                txtSoCCCD.Text = banDoc.CCCD;
            }
            else
            {
                MessageBox.Show("Không tìm thấy bạn đọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Hiển thị danh sách sách đã mượn
            var danhSach = chiTietPhieuMuonRepository.GetChiTietPhieuMuonChiTiet(maPhieuMuon);
            dgvDanhSachMuon.DataSource = danhSach;
            DoiTenCotDanhSachMuon();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}