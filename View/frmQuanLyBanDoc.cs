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
    public partial class frmQuanLyBanDoc : Form
    {
        private BanDocRepository banDocRepository = new BanDocRepository();
        private BanDocController _banDocController;
        private bool isSelectingRow = false;
        public frmQuanLyBanDoc()
        {
            InitializeComponent();
            _banDocController = new BanDocController(banDocRepository);
        }

        private void frmDocGia_Load(object sender, EventArgs e)
        {
            dtpNgayDangKy.Value = DateTime.Now;    // Gán ngày hiện tại
            dtpNgayDangKy.Enabled = false;         // Không cho chỉnh sửa
            LoadData();
        }
        private void LoadData()
        {
            dgvBanDoc.DataSource = _banDocController.GetAll();

            // Đổi tên cột sang tiếng Việt có dấu
            dgvBanDoc.Columns["MaBanDoc"].HeaderText = "Mã Bạn Đọc";
            dgvBanDoc.Columns["HoTen"].HeaderText = "Họ Và Tên";
            dgvBanDoc.Columns["GioiTinh"].HeaderText = "Giới Tính";
            dgvBanDoc.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
            dgvBanDoc.Columns["CCCD"].HeaderText = "CCCD";
            dgvBanDoc.Columns["SDT"].HeaderText = "Số Điện Thoại";
            dgvBanDoc.Columns["Email"].HeaderText = "Email";
            dgvBanDoc.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            dgvBanDoc.Columns["NgayDangKy"].HeaderText = "Ngày Đăng Ký";
            dgvBanDoc.Columns["HinhAnh"].HeaderText = "Hình Ảnh";
        }

        private void dgvBanDoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelectingRow = true;
                DataGridViewRow row = dgvBanDoc.Rows[e.RowIndex];

                txtMaBanDoc.Text = row.Cells["MaBanDoc"].Value != null ? row.Cells["MaBanDoc"].Value.ToString() : "";
                txtHoTen.Text = row.Cells["HoTen"].Value != null ? row.Cells["HoTen"].Value.ToString() : "";
                txtDiaChi.Text = row.Cells["DiaChi"].Value != null ? row.Cells["DiaChi"].Value.ToString() : "";
                txtEmail.Text = row.Cells["Email"].Value != null ? row.Cells["Email"].Value.ToString() : "";
                txtSoDienThoai.Text = row.Cells["SDT"].Value != null ? row.Cells["SDT"].Value.ToString() : "";
                txtSoCCCD.Text = row.Cells["CCCD"].Value != null ? row.Cells["CCCD"].Value.ToString() : "";

                if (row.Cells["NgaySinh"].Value != null)
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);

                if (row.Cells["NgayDangKy"].Value != null)
                    dtpNgayDangKy.Value = Convert.ToDateTime(row.Cells["NgayDangKy"].Value);


                bool gioiTinh = Convert.ToBoolean(row.Cells["GioiTinh"].Value);
                if (gioiTinh)
                    rdNam.Checked = true;
                else
                    rdNu.Checked = true;


                string hinhAnh = row.Cells["HinhAnh"]?.Value?.ToString() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(hinhAnh))
                {
                    string imagePath = Path.Combine("Images", hinhAnh);
                    if (File.Exists(imagePath))
                    {
                        pbHinhAnh.Image = Image.FromFile(imagePath);
                        pbHinhAnh.Tag = hinhAnh;
                    }
                    else
                    {
                        pbHinhAnh.Image = null;
                        pbHinhAnh.Tag = null;
                    }
                }
                else
                {
                    pbHinhAnh.Image = null;
                    pbHinhAnh.Tag = null;
                }
            }
        }

        private void dgvBanDoc_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string tenCot = dgvBanDoc.Columns[e.ColumnIndex].Name;
            if (tenCot == "NgaySinh" && e.Value is DateTime date)
            {
                e.Value = date.ToString("dd/MM/yyyy");
                e.FormattingApplied = true;
            }
            if (tenCot == "NgayDangKy" && e.Value is DateTime dateDK)
            {
                e.Value = dateDK.ToString("dd/MM/yyyy");
                e.FormattingApplied = true;
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Chọn hình ảnh";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourcePath);

                    // Đọc ảnh vào memory stream để tránh bị khóa file
                    using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            ms.Position = 0;
                            pbHinhAnh.Image = Image.FromStream(ms);
                        }
                    }
                    pbHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage; // Ảnh vừa khung
                    pbHinhAnh.Tag = sourcePath; // Lưu đường dẫn gốc để copy khi lưu
                }
            }
        }

        private void HideAllErrorLabels()
        {
            lblVuiLongNhapHoTen.Visible = false;
            lblVuiLongNhapDiaChi.Visible = false;
            lblVuiLongNhapEmail.Visible = false;
            lblVuiLongNhapSDT.Visible = false;
            lblVuiLongChonGioiTinh.Visible = false;
            lblVuiLongNhapSoCCCD.Visible = false;
            lblVuiLongChonHinhAnh.Visible = false;
            lblVuiLongChonNgaySinh.Visible = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Nếu đang chọn dòng thì không cho thêm mới
            if (isSelectingRow)
            {
                MessageBox.Show("Bạn đang chọn một bạn đọc. Vui lòng nhấn 'Làm mới' trước khi thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HideAllErrorLabels();

            // Hiện thị tất cả các nhãn lỗi để người dùng có thể thấy
            bool hasError = false;

            if (pbHinhAnh.Tag == null)
            {
                lblVuiLongChonHinhAnh.Text = "Vui lòng chọn hình ảnh";
                lblVuiLongChonHinhAnh.Visible = true;
            }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                lblVuiLongNhapHoTen.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                lblVuiLongNhapDiaChi.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblVuiLongNhapEmail.Text = "Vui lòng nhập email";
                lblVuiLongNhapEmail.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                lblVuiLongNhapSDT.Text = "Vui lòng nhập số điện thoại";
                lblVuiLongNhapSDT.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtSoCCCD.Text))
            {
                lblVuiLongNhapSoCCCD.Text = "Vui lòng nhập số CCCD";
                lblVuiLongNhapSoCCCD.Visible = true;
                hasError = true;
            }
            if (!rdNam.Checked && !rdNu.Checked)
            {
                lblVuiLongChonGioiTinh.Visible = true;
                hasError = true;
            }

            // Gom lỗi vào StringBuilder
            StringBuilder errorMsg = new StringBuilder();

            // Kiểm tra ngày sinh
            if (dtpNgaySinh.Value == dtpNgaySinh.MinDate)
            {
                errorMsg.AppendLine("Vui lòng chọn ngày sinh.");
                lblVuiLongChonNgaySinh.Text = "Vui lòng chọn ngày sinh";
                lblVuiLongChonNgaySinh.Visible = true;
                hasError = true;
            }
            else if (dtpNgaySinh.Value > DateTime.Now)
            {
                errorMsg.AppendLine("Ngày sinh không hợp lệ (không được lớn hơn ngày hiện tại).");
                lblVuiLongChonNgaySinh.Text = "Ngày sinh không hợp lệ";
                lblVuiLongChonNgaySinh.Visible = true;
                hasError = true;
            }
            else
            {
                int tuoi = DateTime.Now.Year - dtpNgaySinh.Value.Year;
                if (dtpNgaySinh.Value > DateTime.Now.AddYears(-tuoi)) tuoi--;
                if (tuoi < 7)
                {
                    errorMsg.AppendLine("Bạn chưa đủ tuổi mượn sách (Tối thiểu 7 tuổi).");
                    hasError = true;
                }
            }

            // Kiểm tra số điện thoại hợp lệ
            if (!string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                if (!txtSoDienThoai.Text.All(char.IsDigit) || txtSoDienThoai.Text.Length < 10 || txtSoDienThoai.Text.Length > 11)
                {
                    errorMsg.AppendLine("Số điện thoại phải có ít nhất 10 số và nhiều nhất là 11 số.");
                    hasError = true;
                }
            }

            // Kiểm tra số CCCD hợp lệ
            if (!string.IsNullOrWhiteSpace(txtSoCCCD.Text))
            {
                if (!txtSoCCCD.Text.All(char.IsDigit) || txtSoCCCD.Text.Length != 12)
                {
                    errorMsg.AppendLine("Số CCCD phải có 12 số.");
                    hasError = true;
                }
            }

            // Nếu có lỗi thì báo tất cả cùng lúc
            if (errorMsg.Length > 0)
            {
                MessageBox.Show(errorMsg.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra trùng số CCCD, số điện thoại và email
            var danhSach = banDocRepository.GetAll();

            // Kiểm tra trùng số CCCD
            bool cccdTrung = danhSach.Any(x => !string.IsNullOrWhiteSpace(x.CCCD) && x.CCCD.Trim() == txtSoCCCD.Text.Trim());
            if (cccdTrung)
            {
                lblVuiLongNhapSoCCCD.Text = "Số CCCD đã tồn tại!";
                lblVuiLongNhapSoCCCD.Visible = true;
                hasError = true;
            }
            else
            {
                lblVuiLongNhapSoCCCD.Text = "Vui lòng nhập số CCCD";
            }

            // Kiểm tra trùng số điện thoại
            bool sdtTrung = danhSach.Any(x => !string.IsNullOrWhiteSpace(x.SDT) && x.SDT.Trim() == txtSoDienThoai.Text.Trim());
            if (sdtTrung)
            {
                lblVuiLongNhapSDT.Text = "Số điện thoại bạn đăng ký đã tồn tại!";
                lblVuiLongNhapSDT.Visible = true;
                hasError = true;
            }
            else
            {
                lblVuiLongNhapSDT.Text = "Vui lòng nhập số điện thoại";
            }

            // Kiểm tra trùng email
            bool emailTrung = danhSach.Any(x => !string.IsNullOrWhiteSpace(x.Email) && x.Email.Trim().ToLower() == txtEmail.Text.Trim().ToLower());
            if (emailTrung)
            {
                lblVuiLongNhapEmail.Text = "Email bạn đăng ký đã tồn tại!";
                lblVuiLongNhapEmail.Visible = true;
                hasError = true;
            }
            else
            {
                lblVuiLongNhapEmail.Text = "Vui lòng nhập email";
            }

            // Nếu có lỗi thì return
            if (hasError)
                return;

            // Kiểm tra hình ảnh
            string fileName = "";
            if (pbHinhAnh.Tag != null)
            {
                string sourcePath = pbHinhAnh.Tag?.ToString() ?? "";
                if (!string.IsNullOrEmpty(sourcePath) && File.Exists(sourcePath))
                {
                    string? imagesFolder = Path.Combine(Application.StartupPath, "Images");
                    if (!string.IsNullOrEmpty(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    if (!string.IsNullOrEmpty(sourcePath))
                    {
                        fileName = Path.GetFileName(sourcePath);
                        if (!string.IsNullOrEmpty(imagesFolder) && !string.IsNullOrEmpty(fileName))
                        {
                            string destPath = "";
                            if (!string.IsNullOrEmpty(imagesFolder) && !string.IsNullOrEmpty(fileName))
                            {
                                destPath = Path.Combine(imagesFolder, fileName);
                            }

                            // Kiểm tra trùng tên, nếu có thì thêm số vào
                            int count = 1;
                            string newFileName = fileName;
                            while (File.Exists(destPath))
                            {
                                string nameOnly = Path.GetFileNameWithoutExtension(fileName);
                                string extension = Path.GetExtension(fileName);
                                newFileName = $"{nameOnly}_{count}{extension}";
                                destPath = Path.Combine(imagesFolder, newFileName);
                                count++;
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(sourcePath) && !string.IsNullOrEmpty(destPath) && File.Exists(sourcePath))
                                {
                                    File.Copy(sourcePath, destPath, true);
                                    fileName = newFileName;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi lưu hình: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }

            // Tạo đối tượng BanDoc và lưu vào database
            BanDoc banDoc = new BanDoc
            {
                HoTen = txtHoTen.Text?.Trim() ?? "",
                DiaChi = txtDiaChi.Text?.Trim() ?? "",
                Email = txtEmail.Text?.Trim() ?? "",
                SDT = txtSoDienThoai.Text?.Trim() ?? "",
                CCCD = txtSoCCCD.Text?.Trim() ?? "",
                NgaySinh = dtpNgaySinh.Value,
                NgayDangKy = DateTime.Now,
                GioiTinh = rdNam.Checked, // True nếu chọn Nam, False nếu chọn Nữ
                HinhAnh = fileName  // Chỉ lưu tên file
            };

            try
            {
                banDocRepository.Add(banDoc);
                MessageBox.Show("Thêm bạn đọc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData(); // Cập nhật danh sách sách
                ResetForm(); // Đặt lại form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu vào database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtMaBanDoc.Text))
            {
                MessageBox.Show("Vui lòng chọn bạn đọc cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maBanDoc = int.Parse(txtMaBanDoc.Text);

            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            // Lấy thông tin bạn đọc để xóa hình (nếu có)
            BanDoc? banDoc = banDocRepository.GetBanDocById(maBanDoc);
            if (banDoc == null)
            {
                MessageBox.Show("Không tìm thấy bạn đọc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xóa bản ghi khỏi database
            try
            {
                banDocRepository.Delete(maBanDoc);

                // Xóa hình trong thư mục Images nếu có
                if (!string.IsNullOrEmpty(banDoc.HinhAnh))
                {
                    string imagePath = Path.Combine(Application.StartupPath, "Images", banDoc.HinhAnh);
                    if (File.Exists(imagePath))
                    {
                        pbHinhAnh.Image = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(imagePath);
                    }
                }

                MessageBox.Show("Xóa bạn đọc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData(); // Cập nhật danh sách sách
                ResetForm(); // Đặt lại form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaBanDoc.Text))
            {
                MessageBox.Show("Vui lòng chọn bạn đọc cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMaBanDoc.Text, out int maBanDoc))
            {
                MessageBox.Show("Mã bạn đọc không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                errors.AppendLine("Không được để trống họ tên!");

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
                errors.AppendLine("Không được để trống địa chỉ!");

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                errors.AppendLine("Không được để trống email!");

            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
                errors.AppendLine("Không được để trống số điện thoại!");

            if (!rdNam.Checked && !rdNu.Checked)
                errors.AppendLine("Không được để trống giới tính!");

            int tuoi = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (dtpNgaySinh.Value > DateTime.Now.AddYears(-tuoi)) tuoi--;

            if (tuoi < 7)
                errors.AppendLine("Bạn chưa đủ tuổi để mượn sách (Tối thiểu 7 tuổi).");

            if (!txtSoDienThoai.Text.All(char.IsDigit) || txtSoDienThoai.Text.Length < 10)
                errors.AppendLine("Số điện thoại phải có ít nhất 10 chữ số và chỉ bao gồm số!");

            // Tránh kiểm tra trùng chính mình
            var danhSach = banDocRepository.GetAll();
            bool emailTrung = danhSach.Any(x => x.MaBanDoc != maBanDoc && !string.IsNullOrWhiteSpace(x.Email) && x.Email.Trim().ToLower() == txtEmail.Text.Trim().ToLower());
            if (emailTrung)
                errors.AppendLine("Email bạn đăng ký đã tồn tại!");

            bool sdtTrung = danhSach.Any(x => x.MaBanDoc != maBanDoc && !string.IsNullOrWhiteSpace(x.SDT) && x.SDT.Trim() == txtSoDienThoai.Text.Trim());
            if (sdtTrung)
                errors.AppendLine("Số điện thoại bạn đăng ký đã tồn tại!");

            // Nếu có lỗi thì hiện tất cả ra rồi return
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin cũ từ database
            BanDoc? banDocCu = banDocRepository.GetBanDocById(maBanDoc);
            if (banDocCu == null)
            {
                MessageBox.Show("Không tìm thấy bạn đọc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tạo đối tượng mới
            BanDoc banDocMoi = new BanDoc
            {
                MaBanDoc = maBanDoc,
                HoTen = txtHoTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                DiaChi = txtDiaChi.Text,
                NgayDangKy = dtpNgayDangKy.Value,
                GioiTinh = rdNam.Checked,
                Email = txtEmail.Text,
                SDT = txtSoDienThoai.Text,
                HinhAnh = banDocCu.HinhAnh,
                CCCD = txtSoCCCD.Text.Trim()
            };

            // Kiểm tra nếu có ảnh mới
            if (pbHinhAnh.Tag != null && pbHinhAnh.Tag.ToString() != banDocCu.HinhAnh)
            {
                string? newImagePath = pbHinhAnh.Tag.ToString();
                if (!string.IsNullOrEmpty(newImagePath))
                {
                    string fileName = Path.GetFileName(newImagePath);
                    string destPath = Path.Combine(Application.StartupPath, "Images", fileName);

                    try
                    {
                        string? destDir = Path.GetDirectoryName(destPath);
                        if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                            Directory.CreateDirectory(destDir);

                        if (!string.IsNullOrEmpty(newImagePath) && !string.IsNullOrEmpty(destPath) && File.Exists(newImagePath))
                        {
                            File.Copy(newImagePath, destPath, true);
                            banDocMoi.HinhAnh = fileName;
                        }

                        // Xóa ảnh cũ nếu khác
                        if (!string.IsNullOrEmpty(banDocCu.HinhAnh) && banDocCu.HinhAnh != fileName)
                        {
                            string oldImagePath = Path.Combine(Application.StartupPath, "Images", banDocCu.HinhAnh);
                            if (File.Exists(oldImagePath))
                                File.Delete(oldImagePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xử lý hình ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // Cập nhật dữ liệu trong database
            try
            {
                banDocRepository.Update(banDocMoi);
                MessageBox.Show("Cập nhật thông tin bạn đọc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData(); // Cập nhật danh sách sách
                ResetForm(); // Đặt lại form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maBanDocText = txtTimKiem.Text.Trim();

            if (!int.TryParse(maBanDocText, out int maBanDoc))
            {
                MessageBox.Show("Mã bạn đọc không hợp lệ. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tìm bạn đọc theo mã
            var banDoc = _banDocController.GetById(maBanDoc);

            // Xóa dữ liệu cũ trên DataGridView trước khi gán dữ liệu mới
            dgvBanDoc.DataSource = null;

            if (banDoc != null)
            {
                // Tạo danh sách tạm chứa kết quả tìm được
                var danhSachBanDoc = new List<BanDoc> { banDoc };

                // Gán danh sách vào DataGridView
                dgvBanDoc.DataSource = danhSachBanDoc;

                // Nếu có hàng, gọi sự kiện CellClick để hiển thị chi tiết
                if (dgvBanDoc.Rows.Count > 0)
                {
                    dgvBanDoc_CellClick(this, new DataGridViewCellEventArgs(0, 0));
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy bạn đọc với mã đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bthTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmQuanLyTaiKhoan frm = new frmQuanLyTaiKhoan();
            frm.ShowDialog();
            this.Show();
        }

        private void ResetForm()
        {
            isSelectingRow = false; // Đặt lại trạng thái về thêm mới
            HideAllErrorLabels();
            txtMaBanDoc.Clear();
            txtHoTen.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            txtSoCCCD.Clear();
            txtTimKiem.Clear();

            dtpNgaySinh.Value = DateTime.Now;
            dtpNgayDangKy.Value = DateTime.Now;

            rdNam.Checked = false;
            rdNu.Checked = false;

            pbHinhAnh.Image = null;
            pbHinhAnh.Tag = null;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadData();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuiOTP_Click(object sender, EventArgs e)
        {
            frmGuiOTP frm = new frmGuiOTP();
            frm.ShowDialog();
        }
    }
}

