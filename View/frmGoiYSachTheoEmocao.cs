using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien.Models;
using ThuVien.Repository;
using ThuVien.Services;

namespace ThuVien.View
{
    public partial class frmGoiYSachTheoEmocao : Form
    {
        private readonly EmotionBasedRecommendationService _recommendationService;
        private readonly LichSuCamXucRepository _lichSuRepo;
        private readonly int _maBanDoc;
        private DanhSachGoiYTheoEmocao _ketQuaHienTai;

        public frmGoiYSachTheoEmocao(int maBanDoc)
        {
            InitializeComponent();
            _recommendationService = new EmotionBasedRecommendationService();
            _lichSuRepo = new LichSuCamXucRepository();
            _maBanDoc = maBanDoc;

            // Set focus vào textbox
            this.Load += (s, e) => txtTrangThai.Focus();
        }

        /// <summary>
        /// Xử lý khi nhấn nút Phân Tích
        /// </summary>
        private async void btnPhanTich_Click(object sender, EventArgs e)
        {
            string trangThai = txtTrangThai.Text.Trim();

            if (string.IsNullOrEmpty(trangThai))
            {
                MessageBox.Show("Vui lòng nhập trạng thái hoặc cảm xúc của bạn!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTrangThai.Focus();
                return;
            }

            await PhanTichVaGoiY(trangThai);
        }

        /// <summary>
        /// Phân tích cảm xúc và gợi ý sách
        /// </summary>
        private async Task PhanTichVaGoiY(string trangThai)
        {
            try
            {
                // Hiển thị trạng thái đang xử lý
                HienThiTrangThaiXuLy(true);

                // Gọi service phân tích và gợi ý
                _ketQuaHienTai = await _recommendationService.GoiYSachTheoEmotionAsync(trangThai, _maBanDoc, 10);

                // Ẩn trạng thái xử lý
                HienThiTrangThaiXuLy(false);

                // Hiển thị kết quả
                HienThiKetQua(_ketQuaHienTai);

                // Lưu lịch sử
                LuuLichSu(_ketQuaHienTai);
            }
            catch (Exception ex)
            {
                HienThiTrangThaiXuLy(false);
                MessageBox.Show($"Lỗi khi phân tích: {ex.Message}\n\n" +
                    "Vui lòng kiểm tra:\n" +
                    "- Kết nối internet (nếu dùng AI)\n" +
                    "- Cấu hình AI trong file AIService.cs\n" +
                    "- Dữ liệu sách trong database",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hiển thị/ẩn trạng thái đang xử lý
        /// </summary>
        private void HienThiTrangThaiXuLy(bool hienThi)
        {
            lblDangXuLy.Visible = hienThi;
            progressBar.Visible = hienThi;
            btnPhanTich.Enabled = !hienThi;
            pnlVidu.Visible = !hienThi;

            if (hienThi)
            {
                pnlKetQua.Visible = false;
                dgvGoiY.Visible = false;
                btnXemChiTiet.Visible = false;
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Hiển thị kết quả phân tích và gợi ý
        /// </summary>
        private void HienThiKetQua(DanhSachGoiYTheoEmocao ketQua)
        {
            // Hiển thị thông tin cảm xúc
            pnlKetQua.Visible = true;
            
            string emoji = LayEmojiCamXuc(ketQua.CamXucPhanTich.CamXucChinh);
            lblCamXuc.Text = $"{emoji} Cảm xúc: {char.ToUpper(ketQua.CamXucPhanTich.CamXucChinh[0])}{ketQua.CamXucPhanTich.CamXucChinh.Substring(1)}";
            lblDoTinCay.Text = $"Độ tin cậy: {(ketQua.CamXucPhanTich.DoTinCay * 100):F0}%";
            lblLyDo.Text = $"💭 {ketQua.CamXucPhanTich.LyDo}";
            lblDongVien.Text = $"💪 {ketQua.CauDongVien}";

            // Hiển thị danh sách sách gợi ý
            if (ketQua.DanhSachSach != null && ketQua.DanhSachSach.Any())
            {
                dgvGoiY.Visible = true;
                btnXemChiTiet.Visible = true;

                var dataSource = ketQua.DanhSachSach.Select(x => new
                {
                    MaSach = x.Sach.MaSach,
                    TenSach = x.Sach.TenSach,
                    TacGia = x.Sach.TacGia,
                    TheLoai = x.Sach.TenLoai,
                    PhuHop = $"{(x.DiemPhuHop * 100):F0}%",
                    LyDo = x.LyDoGoiY,
                    SoLuong = x.Sach.SoLuong,
                    TinhTrang = x.Sach.TinhTrangSach
                }).ToList();

                dgvGoiY.DataSource = dataSource;

                // Định dạng cột
                if (dgvGoiY.Columns.Count > 0)
                {
                    dgvGoiY.Columns["MaSach"].HeaderText = "Mã";
                    dgvGoiY.Columns["MaSach"].Width = 60;
                    dgvGoiY.Columns["TenSach"].HeaderText = "Tên Sách";
                    dgvGoiY.Columns["TenSach"].Width = 250;
                    dgvGoiY.Columns["TacGia"].HeaderText = "Tác Giả";
                    dgvGoiY.Columns["TacGia"].Width = 150;
                    dgvGoiY.Columns["TheLoai"].HeaderText = "Thể Loại";
                    dgvGoiY.Columns["TheLoai"].Width = 120;
                    dgvGoiY.Columns["PhuHop"].HeaderText = "Phù Hợp";
                    dgvGoiY.Columns["PhuHop"].Width = 80;
                    dgvGoiY.Columns["LyDo"].HeaderText = "Lý Do Gợi Ý";
                    dgvGoiY.Columns["LyDo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvGoiY.Columns["SoLuong"].HeaderText = "SL";
                    dgvGoiY.Columns["SoLuong"].Width = 50;
                    dgvGoiY.Columns["TinhTrang"].HeaderText = "Tình Trạng";
                    dgvGoiY.Columns["TinhTrang"].Width = 100;

                    // Tô màu dòng theo độ phù hợp
                    foreach (DataGridViewRow row in dgvGoiY.Rows)
                    {
                        string phuHop = row.Cells["PhuHop"].Value?.ToString() ?? "0%";
                        int diemPhuHop = int.Parse(phuHop.Replace("%", ""));

                        if (diemPhuHop >= 80)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        else if (diemPhuHop >= 60)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sách phù hợp với cảm xúc của bạn.\n" +
                    "Vui lòng thử lại hoặc kiểm tra dữ liệu sách trong hệ thống.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Lấy emoji phù hợp với cảm xúc
        /// </summary>
        private string LayEmojiCamXuc(string camXuc)
        {
            return camXuc.ToLower() switch
            {
                "buồn" => "😢",
                "vui" => "😊",
                "lo lắng" => "😰",
                "tức giận" => "😠",
                "hạnh phúc" => "😄",
                "mệt mỏi" => "😴",
                "hồi hộp" => "😮",
                _ => "😐"
            };
        }

        /// <summary>
        /// Lưu lịch sử cảm xúc
        /// </summary>
        private void LuuLichSu(DanhSachGoiYTheoEmocao ketQua)
        {
            try
            {
                var sachIds = string.Join(",", ketQua.DanhSachSach.Select(x => x.Sach.MaSach));

                var lichSu = new LichSuCamXuc
                {
                    MaBanDoc = _maBanDoc,
                    TrangThai = ketQua.TrangThaiNguoiDung,
                    CamXucPhanTich = ketQua.CamXucPhanTich.CamXucChinh,
                    DoTinCay = ketQua.CamXucPhanTich.DoTinCay,
                    ThoiGian = DateTime.Now,
                    GoiYSachIds = sachIds
                };

                _lichSuRepo.Luu(lichSu);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lưu lịch sử: {ex.Message}");
                // Không hiển thị lỗi cho người dùng vì đây không phải chức năng chính
            }
        }

        /// <summary>
        /// Xem chi tiết sách được chọn
        /// </summary>
        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvGoiY.SelectedRows.Count > 0)
            {
                int maSach = Convert.ToInt32(dgvGoiY.SelectedRows[0].Cells["MaSach"].Value);
                MessageBox.Show($"Chức năng xem chi tiết sách {maSach} sẽ được phát triển tiếp.\n\n" +
                    "Bạn có thể tích hợp với form chi tiết sách hiện có của dự án.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // TODO: Mở form chi tiết sách
                // var frmChiTiet = new frmChiTietSach(maSach);
                // frmChiTiet.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Double click vào sách để xem chi tiết
        /// </summary>
        private void dgvGoiY_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnXemChiTiet_Click(sender, e);
            }
        }

        /// <summary>
        /// Xem lịch sử cảm xúc
        /// </summary>
        private void btnLichSu_Click(object sender, EventArgs e)
        {
            try
            {
                var lichSu = _lichSuRepo.LayTheoMaBanDoc(_maBanDoc, 20);
                
                if (!lichSu.Any())
                {
                    MessageBox.Show("Bạn chưa có lịch sử cảm xúc nào!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị trong một form mới hoặc MessageBox
                string noiDung = "📊 LỊCH SỬ CẢM XÚC CỦA BẠN\n\n";
                foreach (var item in lichSu.Take(10))
                {
                    string emoji = LayEmojiCamXuc(item.CamXucPhanTich);
                    noiDung += $"{emoji} {item.ThoiGian:dd/MM/yyyy HH:mm} - {item.CamXucPhanTich}\n";
                    noiDung += $"   \"{item.TrangThai}\"\n\n";
                }

                // Thống kê
                var thongKe = _lichSuRepo.ThongKeCamXuc(_maBanDoc);
                if (thongKe.Any())
                {
                    noiDung += "\n📈 THỐNG KÊ:\n";
                    foreach (var item in thongKe.OrderByDescending(x => x.Value).Take(5))
                    {
                        string emoji = LayEmojiCamXuc(item.Key);
                        noiDung += $"{emoji} {item.Key}: {item.Value} lần\n";
                    }
                }

                MessageBox.Show(noiDung, "Lịch Sử Cảm Xúc",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem lịch sử: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Các nút ví dụ
        private async void btnViDu1_Click(object sender, EventArgs e)
        {
            txtTrangThai.Text = "Hôm nay tôi buồn quá, cảm thấy mệt mỏi và cô đơn. Muốn tìm sách để vơi đi nỗi buồn này.";
            await PhanTichVaGoiY(txtTrangThai.Text);
        }

        private async void btnViDu2_Click(object sender, EventArgs e)
        {
            txtTrangThai.Text = "Tôi đang rất vui vẻ và hạnh phúc! Muốn đọc gì đó tích cực để giữ vững tinh thần này.";
            await PhanTichVaGoiY(txtTrangThai.Text);
        }

        private async void btnViDu3_Click(object sender, EventArgs e)
        {
            txtTrangThai.Text = "Đang lo lắng về tương lai, không biết con đường phía trước sẽ như thế nào. Cần tìm sách để bình tĩnh hơn.";
            await PhanTichVaGoiY(txtTrangThai.Text);
        }
    }
}
