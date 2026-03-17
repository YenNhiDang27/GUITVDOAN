using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ThuVien.Controller;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.View
{
    public partial class frmGoiYSach : Form
    {
        private GoiYSachController _controller;
        private SachRepository _sachRepo;
        private int _maBanDoc;

        public frmGoiYSach(int maBanDoc)
        {
            InitializeComponent();
            _controller = new GoiYSachController();
            _sachRepo = new SachRepository();
            _maBanDoc = maBanDoc;
        }

        private void frmGoiYSach_Load(object sender, EventArgs e)
        {
            HienThiGoiY();
        }

        private void HienThiGoiY()
        {
            try
            {
                MessageBox.Show($"Bắt đầu lấy gợi ý cho MaBanDoc: {_maBanDoc}", "DEBUG");

                // Lấy danh sách gợi ý
                var goiYList = _controller.LayDanhSachGoiY(_maBanDoc);

                MessageBox.Show($"Số gợi ý từ database: {goiYList.Count}", "DEBUG");

                if (!goiYList.Any())
                {
                    // Nếu chưa có gợi ý, tạo mới
                    lblThongBao.Text = "Đang tạo gợi ý cho bạn...";
                    lblThongBao.Visible = true;
                    Application.DoEvents();

                    MessageBox.Show("Đang tạo gợi ý mới...", "DEBUG");

                    var goiYMoi = _controller.TaoGoiYMoi(_maBanDoc, 15);

                    MessageBox.Show($"Đã tạo {goiYMoi.Count} gợi ý mới!", "DEBUG");

                    goiYList = _controller.LayDanhSachGoiY(_maBanDoc);

                    MessageBox.Show($"Sau khi tạo mới, số gợi ý: {goiYList.Count}", "DEBUG");

                    lblThongBao.Visible = false;
                }

                // Hiển thị lên DataGridView
                dgvGoiY.DataSource = null;
                dgvGoiY.DataSource = goiYList;

                // Định dạng cột
                if (dgvGoiY.Columns.Count > 0)
                {
                    dgvGoiY.Columns["MaGoiY"].Visible = false;
                    dgvGoiY.Columns["MaSach"].HeaderText = "Mã Sách";
                    dgvGoiY.Columns["MaSach"].Width = 80;
                    dgvGoiY.Columns["TenSach"].HeaderText = "Tên Sách";
                    dgvGoiY.Columns["TenSach"].Width = 250;
                    dgvGoiY.Columns["TacGia"].HeaderText = "Tác Giả";
                    dgvGoiY.Columns["TacGia"].Width = 150;
                    dgvGoiY.Columns["TenLoai"].HeaderText = "Thể Loại";
                    dgvGoiY.Columns["TenLoai"].Width = 120;
                    dgvGoiY.Columns["PhanTramPhuHop"].HeaderText = "Độ Phù Hợp (%)";
                    dgvGoiY.Columns["PhanTramPhuHop"].Width = 120;
                    dgvGoiY.Columns["LyDoGoiY"].HeaderText = "Lý Do Gợi Ý";
                    dgvGoiY.Columns["LyDoGoiY"].Width = 300;
                    dgvGoiY.Columns["TinhTrangSach"].HeaderText = "Tình Trạng";
                    dgvGoiY.Columns["TinhTrangSach"].Width = 100;

                    dgvGoiY.Columns["HinhAnh"].Visible = false;
                    dgvGoiY.Columns["DiemGoiY"].Visible = false;
                    dgvGoiY.Columns["NgayGoiY"].Visible = false;
                    dgvGoiY.Columns["DaXem"].Visible = false;
                    dgvGoiY.Columns["DaMuon"].Visible = false;
                    dgvGoiY.Columns["TenNXB"].Visible = false;
                    dgvGoiY.Columns["SoLuong"].Visible = false;
                }

                lblTongSo.Text = $"Tổng số gợi ý: {goiYList.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị gợi ý: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaoGoiYMoi_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có muốn tạo lại gợi ý sách dựa trên lịch sử mượn mới nhất?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    lblThongBao.Text = "Đang phân tích và tạo gợi ý...";
                    lblThongBao.Visible = true;
                    Application.DoEvents();

                    var goiYMoi = _controller.TaoGoiYMoi(_maBanDoc, 15);
                    
                    lblThongBao.Visible = false;
                    
                    MessageBox.Show($"Đã tạo {goiYMoi.Count} gợi ý mới!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    HienThiGoiY();
                }
            }
            catch (Exception ex)
            {
                lblThongBao.Visible = false;
                MessageBox.Show($"Lỗi khi tạo gợi ý: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGoiY_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int maSach = Convert.ToInt32(dgvGoiY.Rows[e.RowIndex].Cells["MaSach"].Value);
                    int maGoiY = Convert.ToInt32(dgvGoiY.Rows[e.RowIndex].Cells["MaGoiY"].Value);

                    // Đánh dấu đã xem
                    _controller.DanhDauDaXem(maGoiY);

                    // Hiển thị thông tin chi tiết sách
                    var sach = _sachRepo.GetById(maSach);
                    if (sach != null)
                    {
                        string thongTin = $"Tên sách: {sach.TenSach}\n" +
                                        $"Tác giả: {sach.TacGia}\n" +
                                        $"Thể loại: {sach.TenLoai}\n" +
                                        $"Nhà xuất bản: {sach.TenNXB}\n" +
                                        $"Năm xuất bản: {sach.NamXuatBan}\n" +
                                        $"Số lượng: {sach.SoLuong}\n" +
                                        $"Tình trạng: {sach.TinhTrangSach}\n" +
                                        $"Giới thiệu: {sach.GioiThieu}";

                        MessageBox.Show(thongTin, "Thông Tin Sách", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvGoiY_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Tô màu dựa trên độ phù hợp
            if (dgvGoiY.Columns[e.ColumnIndex].Name == "PhanTramPhuHop" && e.Value != null)
            {
                int phanTram = Convert.ToInt32(e.Value);
                
                if (phanTram >= 80)
                    e.CellStyle.BackColor = Color.LightGreen;
                else if (phanTram >= 60)
                    e.CellStyle.BackColor = Color.LightYellow;
                else if (phanTram >= 40)
                    e.CellStyle.BackColor = Color.LightBlue;
            }
        }
    }
}
