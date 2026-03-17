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

namespace GUITV
{
    public partial class frmQuanLySach : Form
    {
        private bool isSelectingRow = false;
        SachController sachController = new SachController();
        LoaiSachController loaiSachController = new LoaiSachController();
        NhaXuatBanController nhaXuatBanController = new NhaXuatBanController();
        string fileAnh = "";
        string savePath = "";

        public frmQuanLySach()
        {
            InitializeComponent();
        }

        private void frmQuanLySach_Load(object sender, EventArgs e)
        {
            if (Auth.LoaiNguoiDung == "Bạn đọc")
            {
                btnChonAnh.Enabled = false;
                btnSua.Enabled = false;
                btnThem.Enabled = false;
                btnSua.Enabled = false;
            }
            string imageFolderPath = Path.Combine(Application.StartupPath, "Images");

            if (!Directory.Exists(imageFolderPath))
            {
                Directory.CreateDirectory(imageFolderPath);
            }
            LoadDanhMuc();
            LoadNhaXuatBan();
            LoadDanhSachSach();
            LoadTinhTrang();
            ResetForm();
        }

        public void LoadDanhSachSach()
        {
            var dsSach = sachController.GetAll();

            // Cập nhật trạng thái sách dựa vào số lượng 
            SachRepository sachRepo = new SachRepository();
            foreach (var sach in dsSach)
            {
                sachRepo.CapNhatTinhTrangSach(sach.MaSach);
            }

            var dsLoai = loaiSachController.GetAll();
            var dsNXB = nhaXuatBanController.GetAll();

            // Gán tên loại và tên NXB cho từng sách
            foreach (var sach in dsSach)
            {
                var loai = dsLoai.FirstOrDefault(l => l.MaLoai == sach.MaLoai);
                sach.TenLoai = loai != null ? loai.TenLoai : "";

                var nxb = dsNXB.FirstOrDefault(n => n.MaNXB == sach.MaNXB);
                sach.TenNXB = nxb != null ? nxb.TenNXB : "";
            }

            dgvSach.AutoGenerateColumns = false;
            dgvSach.DataSource = null;

            // Định dạng lại giá tiền khi hiển thị
            dgvSach.DataSource = dsSach.Select(s => new
            {
                s.MaSach,
                s.TenSach,
                s.TacGia,
                s.MaLoai,
                s.TenLoai,
                s.MaNXB,
                s.TenNXB,
                s.NamXuatBan,
                s.HinhAnh,
                s.SoLuong,
                s.SoTrang,
                GiaTien = s.GiaTien.ToString("#,##0 VNĐ"),
                s.TinhTrangSach,
                s.GioiThieu
            }).ToList();

            // Chỉ hiển thị các cột mong muốn (không hiển thị MaLoai, MaNXB)
            dgvSach.Columns.Clear();
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSach", HeaderText = "Mã Sách" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSach", HeaderText = "Tên Sách" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TacGia", HeaderText = "Tác Giả" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaLoai", HeaderText = "Mã Loại", Visible = false });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenLoai", HeaderText = "Thể Loại" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNXB", HeaderText = "Mã NXB", Visible = false });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenNXB", HeaderText = "Nhà Xuất Bản" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NamXuatBan", HeaderText = "Năm Xuất Bản" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HinhAnh", HeaderText = "Hình Ảnh" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "Số Lượng" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoTrang", HeaderText = "Số Trang" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GiaTien", HeaderText = "Giá Tiền" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TinhTrangSach", HeaderText = "Tình Trạng" });
            dgvSach.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GioiThieu", HeaderText = "Mô Tả" });
        }

        private void LoadDanhMuc()
        {
            // Lấy danh sách danh mục từ controller
            var danhMucDataTable = loaiSachController.GetAll().ToList();

            // Thiết lập DataSource cho ComboBox
            cbTheLoai.DataSource = danhMucDataTable;

            // Chỉ định thuộc tính hiển thị và giá trị
            cbTheLoai.DisplayMember = "TenLoai"; // Tên hiển thị
            cbTheLoai.ValueMember = "MaLoai"; // Giá trị thực tế
        }

        private void LoadNhaXuatBan()
        {
            // Lấy danh sách danh mục từ controller
            var nhaXuatDataTable = nhaXuatBanController.GetAll().ToList();

            // Thiết lập DataSource cho ComboBox
            cbNhaXuatBan.DataSource = nhaXuatDataTable;

            // Chỉ định thuộc tính hiển thị và giá trị
            cbNhaXuatBan.DisplayMember = "TenNXB"; // Tên hiển thị
            cbNhaXuatBan.ValueMember = "MaNXB"; // Giá trị thực tế
        }

        private void LoadTinhTrang()
        {
            cbTinhTrang.Items.Clear();
            cbTinhTrang.Items.Add("Còn");
            cbTinhTrang.Items.Add("Đã hết");
            cbTinhTrang.SelectedIndex = 0; // chọn mặc định là "Còn"

        }

        private void LoadHinhAnh()
        {
            if (pbHinhAnh.Tag != null)
            {
                string fileName = pbHinhAnh.Tag?.ToString() ?? "";
                string imagePath = Path.Combine(Application.StartupPath, "Images", fileName ?? "");

                if (File.Exists(imagePath))
                {
                    // Giải phóng ảnh cũ nếu có để tránh lỗi bộ nhớ
                    if (pbHinhAnh.Image != null)
                    {
                        pbHinhAnh.Image.Dispose();
                        pbHinhAnh.Image = null;
                    }

                    // Mở file ảnh bằng FileStream và MemoryStream để tránh khóa file
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            ms.Position = 0;
                            pbHinhAnh.Image = Image.FromStream(ms);
                        }
                    }
                }
                else
                {
                    pbHinhAnh.Image = null;
                }
            }
        }

        private void HideAllErrorLabels()
        {
            lblVuiLongNhapTenSach.Visible = false;
            lblVuiLongNhapTenTG.Visible = false;
            lblVuiLongNhapNamXB.Visible = false;
            lblVuiLongNhapSoLuong.Visible = false;
            lblVuiLongNhapSoTrang.Visible = false;
            lblVuiLongNhapGiaTien.Visible = false;
            lblVuiLongChonTinhTrang.Visible = false;
            lblVuiLongNhapMoTa.Visible = false;
            lblVuiLongChonTheLoai.Visible = false;
            lblVuiLongChonNXB.Visible = false;
            lblVuiLongChonHinhAnh.Visible = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        { 
            if (isSelectingRow)
            {
                MessageBox.Show("Bạn đang chọn một sách. Vui lòng nhấn 'Làm mới' trước khi thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HideAllErrorLabels();

            // Hiện thị tất cả các nhãn lỗi để người dùng có thể thấy
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
            {
                lblVuiLongNhapTenSach.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtTacGia.Text))
            {
                lblVuiLongNhapTenTG.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtNamXuatBan.Text))
            {
                lblVuiLongNhapNamXB.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                lblVuiLongNhapSoLuong.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtSoTrang.Text))
            {
                lblVuiLongNhapSoTrang.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtGiaTien.Text))
            {
                lblVuiLongNhapGiaTien.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtMoTa.Text))
            {
                lblVuiLongNhapMoTa.Visible = true;
                hasError = true;
            }
            if (cbTheLoai.SelectedIndex == -1)
            {
                lblVuiLongChonTheLoai.Visible = true;
                hasError = true;
            }
            if (cbNhaXuatBan.SelectedIndex == -1)
            {
                lblVuiLongChonNXB.Visible = true;
                hasError = true;
            }
            if (cbTinhTrang.SelectedIndex == -1)
            {
                lblVuiLongChonTinhTrang.Visible = true;
                hasError = true;
            }
            string hinhAnh = pbHinhAnh.Tag?.ToString() ?? "";
            if (string.IsNullOrEmpty(hinhAnh) || !File.Exists(fileAnh))
            {
                lblVuiLongChonHinhAnh.Visible = true;
                hasError = true;
            }
            if (hasError)
            {
                // Không thực hiện thêm sách nếu có lỗi
                return;
            }

            try
            {
                string tenSach = txtTenSach.Text.Trim();
                string tacGia = txtTacGia.Text.Trim();
                int maLoai = Int32.Parse(cbTheLoai.SelectedValue?.ToString() ?? "1");
                int maNhaXuatBan = Int32.Parse(cbNhaXuatBan.SelectedValue?.ToString() ?? "1");
                int namXuatBan = Int32.Parse(txtNamXuatBan.Text.Trim());
                // Kiểm tra năm xuất bản hợp lệ
                if (namXuatBan > DateTime.Now.Year)
                {
                    MessageBox.Show("Năm xuất bản không được lớn hơn năm hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (namXuatBan < 1900)
                {
                    MessageBox.Show("Năm xuất bản không hợp lệ (quá cũ)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuong = Int32.Parse(txtSoLuong.Text.Trim());

                // Kiểm tra nếu hình ảnh chưa được chọn
                savePath = Path.Combine(Application.StartupPath, "Images", hinhAnh);
                File.Copy(fileAnh, savePath, true);
                int soTrang = Int32.Parse(txtSoTrang.Text.Trim());
                int giaTien = Int32.Parse(txtGiaTien.Text.Trim());
                string tinhTrangSach = cbTinhTrang.SelectedItem?.ToString() ?? "";

                // Kiểm tra tình trạng sách
                if (string.IsNullOrEmpty(tinhTrangSach))
                {
                    MessageBox.Show("Vui lòng chọn tình trạng sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string gioiThieu = txtMoTa.Text.Trim();

                // Tạo đối tượng Sach với thứ tự các trường thông tin đã được điều chỉnh
                Sach s = new Sach
                {
                    TenSach = tenSach,
                    TacGia = tacGia,
                    MaLoai = maLoai,
                    MaNXB = maNhaXuatBan,
                    NamXuatBan = namXuatBan,
                    SoLuong = soLuong,
                    HinhAnh = hinhAnh,
                    SoTrang = soTrang,
                    GiaTien = giaTien,
                    TinhTrangSach = tinhTrangSach,
                    GioiThieu = gioiThieu
                };

                // Gọi phương thức Add mà không gán giá trị
                sachController.Add(s);

                // Kiểm tra xem sách có tồn tại trong danh sách hay không
                var danhSachSach = sachController.GetAll(); // Lấy danh sách sách hiện tại
                var sachDaThem = danhSachSach.FirstOrDefault(x => x.TenSach == tenSach && x.TacGia == tacGia);

                if (sachDaThem != null)
                {
                    MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSach(); // Cập nhật danh sách sách
                    ResetForm(); // Đặt lại form
                }
                else
                {
                    MessageBox.Show("Thêm sách thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Một số trường không được để trống. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException)
            {
                MessageBox.Show("Có lỗi xảy ra khi sao chép hình ảnh. Vui lòng kiểm tra lại!", "Lỗi IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi không xác định", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileAnh = openFileDialog.FileName;
                string fileName = Path.GetFileName(openFileDialog.FileName);

                if (!Directory.Exists(Path.Combine(Application.StartupPath, "Images")))
                {
                    Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Images"));
                }
                pbHinhAnh.Tag = fileName;

                fileName = Path.GetFileName(fileAnh); // Lấy tên file
                savePath = Path.Combine(Application.StartupPath, "Images", fileName);

                pbHinhAnh.Tag = fileName;

                // Thêm đoạn này để hiển thị ảnh lên PictureBox
                using (var fs = new FileStream(fileAnh, FileMode.Open, FileAccess.Read))
                {
                    pbHinhAnh.Image = Image.FromStream(fs);
                }
                pbHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage; // Đảm bảo ảnh vừa khung
            }
        }

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ẩn tất cả các nhãn lỗi trước khi hiển thị thông tin sách
            HideAllErrorLabels();

            if (e.RowIndex >= 0)
            {
                isSelectingRow = true; // Đánh dấu đang chọn dòng

                // Lấy dữ liệu từ dòng được chọn theo chỉ số cột
                txtMaSach.Text = dgvSach.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
                txtTenSach.Text = dgvSach.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";
                txtTacGia.Text = dgvSach.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                cbTheLoai.SelectedValue = dgvSach.Rows[e.RowIndex].Cells[3].Value ?? 1;
                cbNhaXuatBan.SelectedValue = dgvSach.Rows[e.RowIndex].Cells[5].Value ?? 1;
                txtNamXuatBan.Text = dgvSach.Rows[e.RowIndex].Cells[7].Value?.ToString() ?? "";
                pbHinhAnh.Tag = dgvSach.Rows[e.RowIndex].Cells[8].Value?.ToString() ?? "";
                txtSoLuong.Text = dgvSach.Rows[e.RowIndex].Cells[9].Value?.ToString() ?? "";
                txtSoTrang.Text = dgvSach.Rows[e.RowIndex].Cells[10].Value?.ToString() ?? "";
                txtGiaTien.Text = dgvSach.Rows[e.RowIndex].Cells[11].Value?.ToString() ?? "";
                int soLuong = 0;
                int.TryParse(txtSoLuong.Text, out soLuong);
                if (soLuong == 0)
                    cbTinhTrang.SelectedItem = "Đã hết";
                else
                    cbTinhTrang.SelectedItem = "Còn";
                txtMoTa.Text = dgvSach.Rows[e.RowIndex].Cells[13].Value?.ToString() ?? "";

                // Tải hình ảnh từ đường dẫn đã lưu trong Tag
                fileAnh = pbHinhAnh.Tag?.ToString() ?? "";
                savePath = Path.Combine(Application.StartupPath, "Images", pbHinhAnh.Tag?.ToString() ?? "");
                LoadHinhAnh();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem mã sách có hợp lệ không
            if (string.IsNullOrEmpty(txtMaSach.Text))
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin sách từ các ô nhập liệu
            int maSach;
            if (!int.TryParse(txtMaSach.Text, out maSach))
            {
                MessageBox.Show("Mã sách không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tenSach = txtTenSach.Text.Trim();
            string tacGia = txtTacGia.Text.Trim(); // Thêm tác giả
            int maLoai;
            if (!int.TryParse(cbTheLoai.SelectedValue?.ToString() ?? "", out maLoai))
            {
                MessageBox.Show("Mã thể loại không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int maNhaXuatBan;
            if (!int.TryParse(cbNhaXuatBan.SelectedValue?.ToString() ?? "", out maNhaXuatBan))
            {
                MessageBox.Show("Mã nhà xuất bản không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int namXuatBan;
            if (!int.TryParse(txtNamXuatBan.Text, out namXuatBan))
            {
                MessageBox.Show("Năm xuất bản không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int soLuongCon;
            if (!int.TryParse(txtSoLuong.Text, out soLuongCon))
            {
                MessageBox.Show("Số lượng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string hinhAnh = pbHinhAnh.Tag?.ToString() ?? "";
            int soTrang;
            if (!int.TryParse(txtSoTrang.Text, out soTrang))
            {
                MessageBox.Show("Số trang không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int giaTien;
            if (!int.TryParse(txtGiaTien.Text, out giaTien))
            {
                MessageBox.Show("Giá tiền không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tinhTrangSach = cbTinhTrang.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(tinhTrangSach))
            {
                MessageBox.Show("Vui lòng chọn tình trạng sách!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string moTa = txtMoTa.Text.Trim();

            // Tạo đối tượng Sach
            Sach s = new Sach
            {
                MaSach = maSach, // Đảm bảo mã sách được gán
                TenSach = tenSach,
                TacGia = tacGia, // Gán tác giả
                MaLoai = maLoai,
                MaNXB = maNhaXuatBan,
                NamXuatBan = namXuatBan,
                SoLuong = soLuongCon,
                HinhAnh = hinhAnh,
                SoTrang = soTrang,
                GiaTien = giaTien,
                TinhTrangSach = tinhTrangSach,
                GioiThieu = moTa
            };

            // Hỏi xác nhận trước khi sửa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn sửa sách này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return; // Nếu chọn No thì không làm gì cả
            }

            // Gọi phương thức sửa sách
            sachController.Update(s);

            // Kiểm tra xem sách đã được cập nhật hay chưa
            var danhSachSach = sachController.GetAll(); // Lấy danh sách sách hiện tại
            var sachDaCapNhat = danhSachSach.FirstOrDefault(x => x.MaSach == s.MaSach);

            if (sachDaCapNhat != null)
            {
                MessageBox.Show("Sửa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSach(); // Cập nhật danh sách sách
                ResetForm(); // Đặt lại form
            }
            else
            {
                MessageBox.Show("Sửa sách thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string fileName = "";
            if (string.IsNullOrEmpty(txtMaSach.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.");
                return;
            }

            int maSach = int.Parse(txtMaSach.Text);
            fileName = pbHinhAnh.Tag?.ToString() ?? "";
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sách này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Gọi phương thức xóa mà không kiểm tra giá trị trả về
                sachController.Delete(maSach);

                MessageBox.Show("Xóa sản phẩm thành công!");
                LoadDanhSachSach(); // Cập nhật danh sách sách
                ResetForm(); // Đặt lại form
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Lấy mã sách từ ô nhập liệu
            string maSachText = txtTimKiem.Text.Trim();

            // Kiểm tra xem mã sách có hợp lệ không
            if (string.IsNullOrEmpty(maSachText))
            {
                MessageBox.Show("Vui lòng nhập mã sách để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển đổi mã sách sang kiểu int
            if (!int.TryParse(maSachText, out int maSach))
            {
                MessageBox.Show("Mã sách không hợp lệ. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tìm sách theo mã
            var sach = sachController.GetById(maSach);

            // Xóa dữ liệu cũ trên DataGridView trước khi gán dữ liệu mới
            dgvSach.DataSource = null;

            if (sach != null)
            {
                // Tạo danh sách tạm chứa kết quả tìm được
                var danhSachSach = new List<Sach> { sach };

                // Gán danh sách vào DataGridView
                dgvSach.DataSource = danhSachSach;

                // Nếu có hàng, gọi sự kiện CellClick để hiển thị chi tiết (nếu cần)
                if (dgvSach.Rows.Count > 0)
                {
                    dgvSach_CellClick(this, new DataGridViewCellEventArgs(0, 0));
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sách với mã đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ResetForm()
        {
            isSelectingRow = false; // Reset trạng thái về thêm mới
            HideAllErrorLabels();
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTacGia.Clear();
            cbTheLoai.SelectedIndex = -1;
            cbNhaXuatBan.SelectedIndex = -1;
            txtNamXuatBan.Clear();
            txtSoLuong.Clear();
            pbHinhAnh.Image = null;
            pbHinhAnh.Tag = null;
            txtSoTrang.Clear();
            txtGiaTien.Clear();
            cbTinhTrang.SelectedIndex = -1;
            txtMoTa.Clear();
            txtTimKiem.Clear();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadDanhSachSach();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
