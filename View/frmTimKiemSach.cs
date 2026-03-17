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
    public partial class frmTimKiemSach : Form
    {
        SachController sachController = new SachController();
        LoaiSachController loaiSachController = new LoaiSachController();
        NhaXuatBanController nhaXuatBanController = new NhaXuatBanController();

        public frmTimKiemSach()
        {
            InitializeComponent();
            LoadTatCaSach();
        }

        private void frmTimKiemSach_Load(object sender, EventArgs e)
        {
            // Nạp thể loại
            var dsLoai = loaiSachController.GetAll();
            cbTheLoai.DataSource = dsLoai;
            cbTheLoai.DisplayMember = "TenLoai";
            cbTheLoai.ValueMember = "MaLoai";
            cbTheLoai.SelectedIndex = -1;

            // Nạp nhà xuất bản
            var dsNXB = nhaXuatBanController.GetAll();
            cbNhaXuatBan.DataSource = dsNXB;
            cbNhaXuatBan.DisplayMember = "TenNXB";
            cbNhaXuatBan.ValueMember = "MaNXB";
            cbNhaXuatBan.SelectedIndex = -1;

            cbTinhTrang.Items.Clear();
            cbTinhTrang.Items.Add("Còn");
            cbTinhTrang.Items.Add("Hết");
            cbTinhTrang.SelectedIndex = -1;
        }

        private void LoadTatCaSach()
        {
            var danhSach = sachController.GetAll();

            // Lấy danh sách loại sách và NXB
            var dsLoai = loaiSachController.GetAll();
            var dsNXB = nhaXuatBanController.GetAll();

            // Gán tên loại và tên NXB cho từng sách
            foreach (var sach in danhSach)
            {
                var loai = dsLoai.FirstOrDefault(l => l.MaLoai == sach.MaLoai);
                sach.TenLoai = loai != null ? loai.TenLoai : "";
                var nxb = dsNXB.FirstOrDefault(n => n.MaNXB == sach.MaNXB);
                sach.TenNXB = nxb != null ? nxb.TenNXB : "";
            }

            dgvSach.DataSource = danhSach.Select(s => new
            {
                s.MaSach,
                s.TenSach,
                s.TacGia,
                MaLoai = s.MaLoai,
                TheLoai = s.TenLoai,
                MaNXB = s.MaNXB,
                NhaXuatBan = s.TenNXB,
                s.NamXuatBan,
                s.SoLuong,
                s.SoTrang,
                GiaTien = s.GiaTien.ToString("#,##0 VNĐ"),
                TinhTrang = s.SoLuong > 0 ? "Còn" : "Hết",
                MoTa = s.GioiThieu,
                HinhAnh = s.HinhAnh
            }).ToList();

            // Đổi tên cột trực tiếp tại đây
            dgvSach.Columns["MaSach"].HeaderText = "Mã Sách";
            dgvSach.Columns["TenSach"].HeaderText = "Tên Sách";
            dgvSach.Columns["TacGia"].HeaderText = "Tác Giả";
            dgvSach.Columns["TheLoai"].HeaderText = "Thể Loại";
            dgvSach.Columns["NhaXuatBan"].HeaderText = "Nhà Xuất Bản";
            dgvSach.Columns["NamXuatBan"].HeaderText = "Năm Xuất Bản";
            dgvSach.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvSach.Columns["SoTrang"].HeaderText = "Số Trang";
            dgvSach.Columns["GiaTien"].HeaderText = "Giá Tiền";
            dgvSach.Columns["TinhTrang"].HeaderText = "Tình Trạng";
            dgvSach.Columns["MoTa"].HeaderText = "Mô Tả";
            dgvSach.Columns["HinhAnh"].HeaderText = "Hình Ảnh";

            // Ẩn cột mã loại và mã NXB nếu cần
            dgvSach.Columns["MaLoai"].Visible = false;
            dgvSach.Columns["MaNXB"].Visible = false;
        }

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvSach.Rows[e.RowIndex];
                txtMaSach.Text = row.Cells["MaSach"].Value?.ToString() ?? "";
                txtTenSach.Text = row.Cells["TenSach"].Value?.ToString() ?? "";
                txtTacGia.Text = row.Cells["TacGia"].Value?.ToString() ?? "";
                cbTheLoai.SelectedValue = row.Cells["MaLoai"].Value;
                cbNhaXuatBan.SelectedValue = row.Cells["MaNXB"].Value;
                txtNamXuatBan.Text = row.Cells["NamXuatBan"].Value?.ToString() ?? "";
                txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString() ?? "";
                txtSoTrang.Text = row.Cells["SoTrang"].Value?.ToString() ?? "";
                txtGiaTien.Text = row.Cells["GiaTien"].Value?.ToString() ?? "";
                cbTinhTrang.SelectedItem = row.Cells["TinhTrang"].Value?.ToString() ?? "";
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString() ?? "";

                string hinhAnh = row.Cells["HinhAnh"].Value?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(hinhAnh))
                {
                    string imagePath = Path.Combine(Application.StartupPath, "Images", hinhAnh);
                    if (File.Exists(imagePath))
                        pbHinhAnh.Image = Image.FromFile(imagePath);
                    else
                        pbHinhAnh.Image = null;
                }
                else
                {
                    pbHinhAnh.Image = null;
                }
            }
        }

        private void ResetForm()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            cbTheLoai.SelectedIndex = -1;
            cbTheLoai.Text = "";
            txtTacGia.Clear();
            cbNhaXuatBan.SelectedIndex = -1;
            cbNhaXuatBan.Text = "";
            txtSoLuong.Clear();
            txtSoTrang.Clear();
            txtGiaTien.Clear();
            cbTinhTrang.SelectedIndex = -1;
            cbTinhTrang.Text = "";
            txtMoTa.Clear();
            pbHinhAnh.Image = null;
            LoadTatCaSach();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiemSachTheoNhieuTruong();
        }

        private void TimKiemSachTheoNhieuTruong()
        {
            var danhSach = sachController.GetAll();

            // Lấy danh sách loại sách và NXB
            var dsLoai = loaiSachController.GetAll();
            var dsNXB = nhaXuatBanController.GetAll();

            // Gán tên loại và tên NXB cho từng sách
            foreach (var sach in danhSach)
            {
                var loai = dsLoai.FirstOrDefault(l => l.MaLoai == sach.MaLoai);
                sach.TenLoai = loai != null ? loai.TenLoai : "";
                var nxb = dsNXB.FirstOrDefault(n => n.MaNXB == sach.MaNXB);
                sach.TenNXB = nxb != null ? nxb.TenNXB : "";
            }

            // Lấy dữ liệu từ các ô nhập
            string maSach = txtMaSach.Text.Trim();
            string tenSach = txtTenSach.Text.Trim();
            string tacGia = txtTacGia.Text.Trim();
            string theLoai = cbTheLoai.Text.Trim();
            string nhaXuatBan = cbNhaXuatBan.Text.Trim();
            string namXuatBan = txtNamXuatBan.Text.Trim();
            string soLuong = txtSoLuong.Text.Trim();
            string soTrang = txtSoTrang.Text.Trim();
            string giaTien = txtGiaTien.Text.Trim();
            string tinhTrang = cbTinhTrang.Text.Trim();
            string moTa = txtMoTa.Text.Trim();

            // Lọc danh sách theo các trường đã nhập
            var ketQua = danhSach.Where(s =>
                (string.IsNullOrEmpty(maSach) || s.MaSach.ToString().Contains(maSach, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(tenSach) || (s.TenSach != null && s.TenSach.Contains(tenSach, StringComparison.OrdinalIgnoreCase))) &&
                (string.IsNullOrEmpty(tacGia) || (s.TacGia != null && s.TacGia.Contains(tacGia, StringComparison.OrdinalIgnoreCase))) &&
                (string.IsNullOrEmpty(theLoai) || (s.TenLoai != null && s.TenLoai.Contains(theLoai, StringComparison.OrdinalIgnoreCase))) &&
                (string.IsNullOrEmpty(nhaXuatBan) || (s.TenNXB != null && s.TenNXB.Contains(nhaXuatBan, StringComparison.OrdinalIgnoreCase))) &&
                (string.IsNullOrEmpty(namXuatBan) || s.NamXuatBan.ToString().Contains(namXuatBan, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(soLuong) || s.SoLuong.ToString().Contains(soLuong, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(soTrang) || s.SoTrang.ToString().Contains(soTrang, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(giaTien) || s.GiaTien.ToString().Contains(giaTien, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(tinhTrang) || (s.TinhTrangSach != null && s.TinhTrangSach.Contains(tinhTrang, StringComparison.OrdinalIgnoreCase))) &&
                (string.IsNullOrEmpty(moTa) || (s.GioiThieu != null && s.GioiThieu.Contains(moTa, StringComparison.OrdinalIgnoreCase)))
            ).ToList();

            var ketQuaList = ketQua.Select(s => new
            {
                s.MaSach,
                s.TenSach,
                s.TacGia,
                MaLoai = s.MaLoai,
                TheLoai = s.TenLoai,
                MaNXB = s.MaNXB,
                NhaXuatBan = s.TenNXB,
                s.NamXuatBan,
                s.SoLuong,
                s.SoTrang,
                GiaTien = s.GiaTien.ToString("#,##0 VNĐ"),
                TinhTrang = s.SoLuong > 0 ? "Còn" : "Hết",
                MoTa = s.GioiThieu,
                HinhAnh = s.HinhAnh
            }).ToList();

            if (ketQuaList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvSach.DataSource = ketQuaList;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadTatCaSach();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}