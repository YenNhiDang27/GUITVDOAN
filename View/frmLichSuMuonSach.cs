using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien
{
    public partial class frmLichSuMuonSach : Form
    {
        // Lấy mã bạn đọc từ tài khoản đăng nhập hiện tại
        private int? MaBanDoc => TaiKhoan.DangNhapHienTai?.MaBanDoc;

        public frmLichSuMuonSach()
        {
            InitializeComponent();
        }

        private void frmLichSuMuonSach_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Today;
            dtpDenNgay.Value = DateTime.Today;
            rdDaTra.Checked = false;
            rdChuaTra.Checked = false;
            rdQuaHan.Checked = false;
            LoadPhieuMuonCuaBanDoc();
        }

        private void LoadPhieuMuonCuaBanDoc()
        {
            if (MaBanDoc == null)
            {
                MessageBox.Show("Không tìm thấy thông tin bạn đọc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDaTraChuaTra.DataSource = null;
                dgvChiTietPhieuMuon.DataSource = null;
                return;
            }
            PhieuMuonRepository repo = new PhieuMuonRepository();
            var data = repo.GetPhieuMuonByMaBanDoc(MaBanDoc.Value);
            dgvDaTraChuaTra.DataSource = data;
            DoiTenCotDgvDaTraChuaTra();
            dgvChiTietPhieuMuon.DataSource = null;
        }

        private void DoiTenCotDgvDaTraChuaTra()
        {
            if (dgvDaTraChuaTra.Columns.Contains("MaPhieuMuon"))
                dgvDaTraChuaTra.Columns["MaPhieuMuon"].HeaderText = "Mã Phiếu Mượn";
            if (dgvDaTraChuaTra.Columns.Contains("MaBanDoc"))
                dgvDaTraChuaTra.Columns["MaBanDoc"].HeaderText = "Mã Bạn Đọc";
            if (dgvDaTraChuaTra.Columns.Contains("NgayMuon"))
                dgvDaTraChuaTra.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
            if (dgvDaTraChuaTra.Columns.Contains("NgayHenTra"))
                dgvDaTraChuaTra.Columns["NgayHenTra"].HeaderText = "Ngày Hẹn Trả";
            if (dgvDaTraChuaTra.Columns.Contains("DaTra"))
                dgvDaTraChuaTra.Columns["DaTra"].HeaderText = "Đã Trả";
            if (dgvDaTraChuaTra.Columns.Contains("NgayLapPhieu"))
                dgvDaTraChuaTra.Columns["NgayLapPhieu"].HeaderText = "Ngày Lập Phiếu";
        }

        private void DoiTenCotDgvChiTietPhieuMuon()
        {
            if (dgvChiTietPhieuMuon.Columns.Contains("MaChiTiet"))
                dgvChiTietPhieuMuon.Columns["MaChiTiet"].HeaderText = "Mã Chi Tiết";
            if (dgvChiTietPhieuMuon.Columns.Contains("HoTen"))
                dgvChiTietPhieuMuon.Columns["HoTen"].HeaderText = "Họ Và Tên";
            if (dgvChiTietPhieuMuon.Columns.Contains("GioiTinh"))
                dgvChiTietPhieuMuon.Columns["GioiTinh"].HeaderText = "Giới Tính";
            if (dgvChiTietPhieuMuon.Columns.Contains("NgaySinh"))
                dgvChiTietPhieuMuon.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
            if (dgvChiTietPhieuMuon.Columns.Contains("Email"))
                dgvChiTietPhieuMuon.Columns["Email"].HeaderText = "Email";
            if (dgvChiTietPhieuMuon.Columns.Contains("CCCD"))
                dgvChiTietPhieuMuon.Columns["CCCD"].HeaderText = "CCCD";
            if (dgvChiTietPhieuMuon.Columns.Contains("SDT"))
                dgvChiTietPhieuMuon.Columns["SDT"].HeaderText = "Số Điện Thoại";
            if (dgvChiTietPhieuMuon.Columns.Contains("DiaChi"))
                dgvChiTietPhieuMuon.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            if (dgvChiTietPhieuMuon.Columns.Contains("MaSach"))
                dgvChiTietPhieuMuon.Columns["MaSach"].HeaderText = "Mã Sách";
            if (dgvChiTietPhieuMuon.Columns.Contains("TenSach"))
                dgvChiTietPhieuMuon.Columns["TenSach"].HeaderText = "Tên Sách";
            if (dgvChiTietPhieuMuon.Columns.Contains("SoLuongSachMuon"))
                dgvChiTietPhieuMuon.Columns["SoLuongSachMuon"].HeaderText = "Số Lượng";
            if (dgvChiTietPhieuMuon.Columns.Contains("NgayTra"))
                dgvChiTietPhieuMuon.Columns["NgayTra"].HeaderText = "Ngày Trả";
            if (dgvChiTietPhieuMuon.Columns.Contains("PhiPhat"))
                dgvChiTietPhieuMuon.Columns["PhiPhat"].HeaderText = "Phí Phạt";
        }

        private void rdDaTra_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDaTra.Checked && MaBanDoc != null)
            {
                rdChuaTra.Checked = false;
                rdQuaHan.Checked = false;
                PhieuMuonRepository repo = new PhieuMuonRepository();
                dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonByMaBanDoc_AndTrangThai(MaBanDoc.Value, 1);
                DoiTenCotDgvDaTraChuaTra();
                dgvChiTietPhieuMuon.DataSource = null;
            }
        }

        private void rdChuaTra_CheckedChanged(object sender, EventArgs e)
        {
            if (rdChuaTra.Checked && MaBanDoc != null)
            {
                rdDaTra.Checked = false;
                rdQuaHan.Checked = false;
                PhieuMuonRepository repo = new PhieuMuonRepository();
                dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonByMaBanDoc_AndTrangThai(MaBanDoc.Value, 0);
                DoiTenCotDgvDaTraChuaTra();
                dgvChiTietPhieuMuon.DataSource = null;
            }
        }

        private void rdQuaHan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdQuaHan.Checked && MaBanDoc != null)
            {
                rdDaTra.Checked = false;
                rdChuaTra.Checked = false;
                PhieuMuonRepository repo = new PhieuMuonRepository();
                dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonQuaHanByMaBanDoc(MaBanDoc.Value);
                DoiTenCotDgvDaTraChuaTra();
                dgvChiTietPhieuMuon.DataSource = null;
            }
        }

        private void dgvDaTraChuaTra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int maPhieuMuon = Convert.ToInt32(dgvDaTraChuaTra.Rows[e.RowIndex].Cells["MaPhieuMuon"].Value);
                PhieuMuonRepository repo = new PhieuMuonRepository();
                dgvChiTietPhieuMuon.DataSource = repo.GetChiTietPhieuMuon(maPhieuMuon);
                DoiTenCotDgvChiTietPhieuMuon();
            }
        }

        private void dgvDaTraChuaTra_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string tenCot = dgvDaTraChuaTra.Columns[e.ColumnIndex].Name;
            if ((tenCot == "NgayMuon" || tenCot == "NgayHenTra" || tenCot == "NgayLapPhieu") && e.Value is DateTime date)
            {
                e.Value = date.ToString("dd/MM/yyyy");
                e.FormattingApplied = true;
            }
        }

        private void dgvChiTietPhieuMuon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string tenCot = dgvChiTietPhieuMuon.Columns[e.ColumnIndex].Name;
            if (tenCot == "NgayTra" && e.Value is DateTime dateTra)
            {
                e.Value = dateTra.ToString("dd/MM/yyyy");
                e.FormattingApplied = true;
            }
            if (tenCot == "NgaySinh" && e.Value is DateTime dateSinh)
            {
                e.Value = dateSinh.ToString("dd/MM/yyyy");
                e.FormattingApplied = true;
            }
            if (tenCot == "PhiPhat" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal phiPhat))
                {
                    e.Value = phiPhat.ToString("#,##0") + " VNĐ";
                    e.FormattingApplied = true;
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (MaBanDoc == null)
            {
                MessageBox.Show("Không tìm thấy thông tin bạn đọc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvDaTraChuaTra.DataSource = null;
                dgvChiTietPhieuMuon.DataSource = null;
                return;
            }

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;
            DateTime today = DateTime.Today;

            // Kiểm tra nếu cả hai ngày đều lớn hơn ngày hiện tại
            if (tuNgay > today && denNgay > today)
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc không được lớn hơn ngày hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PhieuMuonRepository repo = new PhieuMuonRepository();
            var data = repo.GetPhieuMuonByMaBanDocAndDateRange(MaBanDoc.Value, tuNgay, denNgay);
            dgvDaTraChuaTra.DataSource = data;
            DoiTenCotDgvDaTraChuaTra();
            dgvChiTietPhieuMuon.DataSource = null;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dtpTuNgay.Value = DateTime.Today;
            dtpDenNgay.Value = DateTime.Today;
            rdDaTra.Checked = false;
            rdChuaTra.Checked = false;
            rdQuaHan.Checked = false;
            LoadPhieuMuonCuaBanDoc();
            dgvChiTietPhieuMuon.DataSource = null;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}