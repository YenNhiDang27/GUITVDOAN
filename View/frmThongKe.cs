using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien.Models;
using ThuVien.Repository;
using ThuVien.Utilies;
using QRCoder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ThuVien.View
{
    public partial class frmThongKe : Form
    {
        private PrintDocument printDocumentPhieuPhat = new PrintDocument();
        private DataGridViewRow? selectedPhieuMuonRow;
        private Bitmap? qrBitmapForPrint = null;

        private void LoadTatCaPhieuMuon()
        {
            PhieuMuonRepository repo = new PhieuMuonRepository();
            dgvDaTraChuaTra.DataSource = repo.GetTatCaPhieuMuon();
            DoiTenCotDgvDaTraChuaTra();
        }

        public frmThongKe()
        {
            InitializeComponent();
            printDocumentPhieuPhat.PrintPage += printDocumentPhieuPhat_PrintPage;
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            // Bỏ chọn tất cả radio button
            rdDaTra.Checked = false;
            rdChuaTra.Checked = false;
            rdQuaHan.Checked = false;
            LoadTatCaPhieuMuon();
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
            if (dgvDaTraChuaTra.Columns.Contains("NguoiLapPhieu"))
                dgvDaTraChuaTra.Columns["NguoiLapPhieu"].HeaderText = "Người Lập Phiếu";
            if (dgvDaTraChuaTra.Columns.Contains("Email"))
                dgvDaTraChuaTra.Columns["Email"].HeaderText = "Email";
        }

        private void DoiTenCotDgvChiTietPhieuMuon()
        {
            if (dgvChiTietPhieuMuon.Columns.Contains("MaChiTiet"))
                dgvChiTietPhieuMuon.Columns["MaChiTiet"].HeaderText = "Mã Chi Tiết";
            if (dgvChiTietPhieuMuon.Columns.Contains("HoTen"))
                dgvChiTietPhieuMuon.Columns["HoTen"].HeaderText = "Họ Tên";
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

        private void dgvDaTraChuaTra_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            string tenCot = dgvDaTraChuaTra.Columns[e.ColumnIndex].Name;
            if ((tenCot == "NgayMuon" || tenCot == "NgayHenTra" || tenCot == "NgayTra") && e.Value is DateTime date)
            {
                e.Value = date.ToString("dd/MM/yyyy");
                e.FormattingApplied = true;
            }
        }

        private void dgvChiTietPhieuMuon_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
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

        private void rdDaTra_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDaTra.Checked)
            {
                dgvChiTietPhieuMuon.DataSource = null;
                rdChuaTra.Checked = false;
                rdQuaHan.Checked = false;
                btnGuiMail.Enabled = false;
                PhieuMuonRepository repo = new PhieuMuonRepository();

                string sdt = txtTimKiem.Text.Trim();
                if (!string.IsNullOrEmpty(sdt))
                    dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonBySDT_AndTrangThai(sdt, 1);
                else
                    dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonDaTra();

                DoiTenCotDgvDaTraChuaTra();
                dgvDaTraChuaTra.CellFormatting += dgvDaTraChuaTra_CellFormatting;
            }
        }

        private void rdChuaTra_CheckedChanged(object sender, EventArgs e)
        {
            if (rdChuaTra.Checked)
            {
                dgvChiTietPhieuMuon.DataSource = null;
                rdDaTra.Checked = false;
                rdQuaHan.Checked = false;
                btnGuiMail.Enabled = false;
                PhieuMuonRepository repo = new PhieuMuonRepository();

                string sdt = txtTimKiem.Text.Trim();
                if (!string.IsNullOrEmpty(sdt))
                    dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonBySDT_AndTrangThai(sdt, 0);
                else
                    dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonChuaTra();

                DoiTenCotDgvDaTraChuaTra();
                dgvDaTraChuaTra.CellFormatting += dgvDaTraChuaTra_CellFormatting;
            }
        }

        private void rdQuaHan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdQuaHan.Checked)
            {
                dgvChiTietPhieuMuon.DataSource = null;
                rdDaTra.Checked = false;
                rdChuaTra.Checked = false;
                btnGuiMail.Enabled = true;
                PhieuMuonRepository repo = new PhieuMuonRepository();

                string sdt = txtTimKiem.Text.Trim();
                if (!string.IsNullOrEmpty(sdt))
                    dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonBySDT_AndTrangThai(sdt, null, true);
                else
                    dgvDaTraChuaTra.DataSource = repo.GetPhieuMuonQuaHan();

                DoiTenCotDgvDaTraChuaTra();
                dgvDaTraChuaTra.CellFormatting += dgvDaTraChuaTra_CellFormatting;
            }
        }

        private void dgvDaTraChuaTra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedPhieuMuonRow = dgvDaTraChuaTra.Rows[e.RowIndex];
                int maPhieuMuon = Convert.ToInt32(dgvDaTraChuaTra.Rows[e.RowIndex].Cells["MaPhieuMuon"].Value);
                PhieuMuonRepository repo = new PhieuMuonRepository();
                dgvChiTietPhieuMuon.DataSource = repo.GetChiTietPhieuMuon(maPhieuMuon);
                DoiTenCotDgvChiTietPhieuMuon();
                dgvChiTietPhieuMuon.CellFormatting += dgvChiTietPhieuMuon_CellFormatting;
            }
        }

        private async void btnGuiMail_Click(object sender, EventArgs e)
        {
            // Vô hiệu hóa nút gửi để tránh click nhiều lần
            btnGuiMail.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            int emailCount = 0;
            int errorCount = 0;
            StringBuilder errorMessages = new StringBuilder();

            try
            {
                BanDocRepository banDocRepo = new BanDocRepository();

                foreach (DataGridViewRow row in dgvDaTraChuaTra.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Cells["MaPhieuMuon"].Value != null && row.Cells["MaBanDoc"].Value != null)
                    {
                        int maPhieuMuon = Convert.ToInt32(row.Cells["MaPhieuMuon"].Value);
                        int maBanDoc = Convert.ToInt32(row.Cells["MaBanDoc"].Value);

                        // Lấy email từ database thông qua MaBanDoc
                        var banDoc = banDocRepo.GetBanDocById(maBanDoc);
                        if (banDoc == null || string.IsNullOrWhiteSpace(banDoc.Email))
                        {
                            errorCount++;
                            errorMessages.AppendLine($"- Không tìm thấy email cho bạn đọc mã {maBanDoc}");
                            continue;
                        }

                        string emailNguoiNhan = banDoc.Email.Trim();

                        // Kiểm tra email hợp lệ
                        if (!EmailService.IsValidEmail(emailNguoiNhan))
                        {
                            errorCount++;
                            errorMessages.AppendLine($"- Email không hợp lệ: {emailNguoiNhan}");
                            continue;
                        }

                        // Lấy chi tiết phiếu mượn cho email này
                        PhieuMuonRepository repo = new PhieuMuonRepository();
                        var chiTietList = repo.GetChiTietPhieuMuon(maPhieuMuon);

                        if (chiTietList == null || chiTietList.Count == 0)
                        {
                            errorCount++;
                            errorMessages.AppendLine($"- Không có chi tiết cho phiếu mượn {maPhieuMuon}");
                            continue;
                        }

                        // Lấy họ tên từ bạn đọc
                        string hoTen = banDoc.HoTen ?? "";

                        DateTime ngayHenTra = DateTime.MinValue;
                        if (row.Cells["NgayHenTra"].Value != null && DateTime.TryParse(row.Cells["NgayHenTra"].Value.ToString(), out DateTime temp))
                        {
                            ngayHenTra = temp;
                        }

                        // Tạo nội dung email với chi tiết sách
                        string noiDungEmail = TaoNoiDungEmailChiTiet(hoTen, maPhieuMuon, ngayHenTra, chiTietList);

                        // Gửi email bất đồng bộ
                        EmailService emailService = new EmailService();
                        try
                        {
                            await emailService.GuiEmailAsync(noiDungEmail, emailNguoiNhan, "Thư viện - Thông báo sách quá hạn");
                            emailCount++;
                        }
                        catch (Exception emailEx)
                        {
                            errorCount++;
                            errorMessages.AppendLine($"- Lỗi gửi đến {emailNguoiNhan}: {emailEx.Message}");
                        }
                    }
                }

                // Hiển thị kết quả
                StringBuilder resultMessage = new StringBuilder();
                if (emailCount > 0)
                {
                    resultMessage.AppendLine($"✅ Đã gửi thành công {emailCount} email!");
                }
                if (errorCount > 0)
                {
                    resultMessage.AppendLine($"\n❌ Có {errorCount} email gặp lỗi:");
                    resultMessage.AppendLine(errorMessages.ToString());
                }

                if (emailCount == 0 && errorCount == 0)
                {
                    MessageBox.Show("Không có email nào được gửi!\nVui lòng kiểm tra lại dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(resultMessage.ToString(), "Kết quả gửi email", 
                        errorCount > 0 ? MessageBoxButtons.OK : MessageBoxButtons.OK, 
                        errorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không mong đợi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Kích hoạt lại nút và đổi con trỏ về bình thường
                btnGuiMail.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Tạo nội dung email chi tiết với thông tin sách từ database
        /// </summary>
        private string TaoNoiDungEmailChiTiet(string hoTen, int maPhieuMuon, DateTime ngayHenTra, dynamic chiTietList)
        {
            string ngayHenTraStr = ngayHenTra != DateTime.MinValue ? ngayHenTra.ToString("dd/MM/yyyy") : "Chưa xác định";
            StringBuilder noiDung = new StringBuilder();

            noiDung.AppendLine($"Xin chào {(string.IsNullOrWhiteSpace(hoTen) ? "bạn" : hoTen)}!");
            noiDung.AppendLine();
            noiDung.AppendLine($"Bạn có phiếu mượn sách số {maPhieuMuon} đã quá hạn trả.");
            noiDung.AppendLine("Vui lòng hoàn trả sách và đóng phí phạt theo quy định của thư viện.");
            noiDung.AppendLine();
            noiDung.AppendLine("DANH SÁCH SÁCH CHƯA TRẢ:");
            noiDung.AppendLine("".PadRight(60, '='));

            decimal tongPhiPhat = 0;
            int stt = 1;

            foreach (var item in chiTietList)
            {
                var type = item.GetType();
                string tenSach = type.GetProperty("TenSach")?.GetValue(item)?.ToString() ?? "";
                int soLuong = Convert.ToInt32(type.GetProperty("SoLuongSachMuon")?.GetValue(item) ?? 0);
                decimal phiPhat = Convert.ToDecimal(type.GetProperty("PhiPhat")?.GetValue(item) ?? 0);

                noiDung.AppendLine($"{stt}. {tenSach}");
                noiDung.AppendLine($"   - Số lượng: {soLuong}");
                noiDung.AppendLine($"   - Ngày hẹn trả: {ngayHenTraStr}");
                noiDung.AppendLine($"   - Phí phạt: {phiPhat.ToString("#,##0", new System.Globalization.CultureInfo("vi-VN"))} VNĐ");
                noiDung.AppendLine();

                tongPhiPhat += phiPhat;
                stt++;
            }

            noiDung.AppendLine("".PadRight(60, '='));
            noiDung.AppendLine($"TỔNG PHÍ PHẠT: {tongPhiPhat.ToString("#,##0", new System.Globalization.CultureInfo("vi-VN"))} VNĐ");
            noiDung.AppendLine("".PadRight(60, '='));
            noiDung.AppendLine();
            noiDung.AppendLine("Vui lòng liên hệ thư viện để hoàn tất thủ tục trả sách.");
            noiDung.AppendLine("Cảm ơn bạn đã sử dụng dịch vụ của thư viện!");
            noiDung.AppendLine();
            noiDung.AppendLine("--");
            noiDung.AppendLine("Thư Viện");

            return noiDung.ToString();
        }

        // Sửa lại hàm này để nhận thêm ngày hẹn trả
        private string TaoNoiDungEmail(string hoTen, int maPhieuMuon, DateTime ngayHenTra)
        {
            string ngayHenTraStr = ngayHenTra != DateTime.MinValue ? ngayHenTra.ToString("dd/MM/yyyy") : "Chưa xác định";
            string noiDung = $"Xin chào {(string.IsNullOrWhiteSpace(hoTen) ? "!" : hoTen + " !")}\n\n";
            noiDung += $"Bạn có phiếu mượn sách số {maPhieuMuon} đã quá hạn trả. Vui lòng hoàn trả sách và đóng phạt theo quy định của thư viện.\n\n";
            noiDung += "Danh sách sách chưa trả:\n";
            noiDung += "--------------------------------------------------\n";

            foreach (DataGridViewRow row in dgvChiTietPhieuMuon.Rows)
            {
                if (row.Cells["TenSach"].Value != null)
                {
                    string tenSach = row.Cells["TenSach"].Value?.ToString() ?? "";
                    // Lấy ngày hẹn trả cho từng sách nếu có, nếu không thì dùng ngàyHenTra chung
                    string ngayHenTraSach = "Chưa xác định";
                    if (row.Cells["NgayTra"].Value != null && DateTime.TryParse(row.Cells["NgayTra"].Value.ToString(), out DateTime ngayTraSach))
                    {
                        ngayHenTraSach = ngayTraSach.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ngayHenTraSach = ngayHenTraStr;
                    }
                    decimal phiPhat = row.Cells["PhiPhat"].Value != null ? Convert.ToDecimal(row.Cells["PhiPhat"].Value) : 0m;

                    noiDung += $"📖 {tenSach}\n";
                    noiDung += $"  - Ngày hẹn trả: {ngayHenTraSach}\n";
                    noiDung += $"  - Phí phạt đến hôm nay: {phiPhat.ToString("#,##0", new System.Globalization.CultureInfo("vi-VN"))} VNĐ\n\n";
                }
            }

            noiDung += "--------------------------------------------------\n";
            noiDung += "Vui lòng liên hệ thư viện để hoàn tất thủ tục trả sách.\nCảm ơn bạn!";

            return noiDung;
        }

        private async void GuiEmailSapDenHan()
        {
            try
            {
                PhieuMuonRepository repo = new PhieuMuonRepository();
                var danhSach = repo.GetPhieuMuonSapDenHan();

                if (danhSach == null || danhSach.Count == 0)
                    return; // Không có phiếu mượn sắp đến hạn

                EmailService emailService = new EmailService();
                int sentCount = 0;
                int errorCount = 0;

                foreach (var pm in danhSach)
                {
                    string emailNguoiNhan = pm.Email;

                    // Kiểm tra email hợp lệ
                    if (!EmailService.IsValidEmail(emailNguoiNhan))
                    {
                        errorCount++;
                        continue;
                    }

                    string hoTen = pm.NguoiLapPhieu;
                    DateTime ngayHenTra = pm.NgayHenTra;

                    string noiDung = $"Xin chào {hoTen},\n\n";
                    noiDung += $"Bạn có phiếu mượn sách số {pm.MaPhieuMuon} sẽ đến hạn trả vào ngày {ngayHenTra:dd/MM/yyyy}.\n";
                    noiDung += "Vui lòng trả sách đúng hạn để tránh bị phạt.\n\n";
                    noiDung += "Cảm ơn bạn đã sử dụng dịch vụ của thư viện!\n\n";
                    noiDung += "--\n";
                    noiDung += "Thư Viện";

                    try
                    {
                        await emailService.GuiEmailAsync(noiDung, emailNguoiNhan, "Thư viện - Nhắc nhở sắp đến hạn trả sách");
                        sentCount++;
                    }
                    catch
                    {
                        errorCount++;
                    }
                }

                if (sentCount > 0)
                {
                    string message = $"Đã gửi {sentCount} email nhắc nhở sắp đến hạn trả sách!";
                    if (errorCount > 0)
                        message += $"\nCó {errorCount} email gặp lỗi.";

                    MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi email nhắc nhở: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sdt = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại bạn đọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PhieuMuonRepository repo = new PhieuMuonRepository();
            List<PhieuMuon> danhSach;

            if (rdDaTra.Checked)
                danhSach = repo.GetPhieuMuonBySDT_AndTrangThai(sdt, 1);
            else if (rdChuaTra.Checked)
                danhSach = repo.GetPhieuMuonBySDT_AndTrangThai(sdt, 0);
            else if (rdQuaHan.Checked)
                danhSach = repo.GetPhieuMuonBySDT_AndTrangThai(sdt, null, true);
            else
                danhSach = repo.GetPhieuMuonBySDT_AndTrangThai(sdt, null);

            // Kiểm tra không có phiếu mượn
            if (danhSach == null || danhSach.Count == 0)
            {
                MessageBox.Show("Không có phiếu mượn tương ứng với số điện thoại này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvDaTraChuaTra.DataSource = null;
                return;
            }

            dgvDaTraChuaTra.DataSource = danhSach;
            DoiTenCotDgvDaTraChuaTra();
            dgvDaTraChuaTra.CellFormatting += dgvDaTraChuaTra_CellFormatting;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            rdDaTra.Checked = false;
            rdChuaTra.Checked = false;
            rdQuaHan.Checked = false;
            btnGuiMail.Enabled = false;
            LoadTatCaPhieuMuon();
            dgvChiTietPhieuMuon.DataSource = null; // Xóa chi tiết nếu muốn
        }

        private async Task<string?> GetVietQRRawDataAsync(string bankBin, string accountNo, string accountName, decimal amount, string addInfo)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.Add("x-client-id", "00d27563-5d28-40c3-b751-48f034e113e6");
                client.DefaultRequestHeaders.Add("x-api-key", "ba4a94fd-1db9-47f2-bee2-73d4dba25b88");

                var url = "https://api.vietqr.io/v2/generate";
                var payload = new
                {
                    accountNo = accountNo,
                    accountName = accountName,
                    acqId = bankBin,
                    amount = (int)amount,
                    addInfo = addInfo,
                    format = "text"
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    var response = await client.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var obj = JObject.Parse(responseString);
                    if (obj == null || obj["code"] == null || obj["code"]?.ToString() != "00")
                        return null;
                    return obj["data"]?["qrCode"]?.ToString();
                }
                catch
                {
                    return null;
                }
            }
        }

        private Bitmap? GenerateQRCodeBitmap(string qrContent, int pixelPerModule = 10)
        {
            if (string.IsNullOrWhiteSpace(qrContent))
                return null;

            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    // Clone bitmap để tránh dispose sớm
                    return new Bitmap(qrCode.GetGraphic(pixelPerModule));
                }
            }
            catch
            {
                return null;
            }
        }

        private async void btnInPhieuPhat_Click(object sender, EventArgs e)
        {
            if (selectedPhieuMuonRow == null)
            {
                MessageBox.Show("Vui lòng chọn một phiếu mượn để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tính tổng phí phạt
            decimal tongPhiPhat = 0;
            foreach (DataGridViewRow row in dgvChiTietPhieuMuon.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["PhiPhat"].Value != null)
                    tongPhiPhat += Convert.ToDecimal(row.Cells["PhiPhat"].Value);
            }

            // Tạo nội dung QR
            string bankBin = "970407";
            string accountNo = "150212300424";
            string accountName = "VO NGUYEN";
            decimal amount = tongPhiPhat;
            string addInfo = "Thanh toan phi phat thu vien";
            string? emvcoQR = await GetVietQRRawDataAsync(bankBin, accountNo, accountName, amount, addInfo);

            if (string.IsNullOrWhiteSpace(emvcoQR))
            {
                qrBitmapForPrint = null;
            }
            else
            {
                qrBitmapForPrint = GenerateQRCodeBitmap(emvcoQR, 10);
            }

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDocumentPhieuPhat;
            preview.ShowDialog();

            // Giải phóng sau khi in
            qrBitmapForPrint?.Dispose();
            qrBitmapForPrint = null;
        }

        private void printDocumentPhieuPhat_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (selectedPhieuMuonRow == null)
                return;

            // Lấy thông tin phiếu mượn
            string hoTen = "";
            string diaChi = "";
            string email = "";
            if (dgvChiTietPhieuMuon.Rows.Count > 0)
            {
                var row = dgvChiTietPhieuMuon.Rows[0];
                hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
                diaChi = row.Cells["DiaChi"].Value?.ToString() ?? "";
                if (dgvChiTietPhieuMuon.Columns.Contains("Email"))
                    email = row.Cells["Email"].Value?.ToString() ?? "";
            }
            string ngayHenTra = selectedPhieuMuonRow.Cells["NgayHenTra"].Value != null
                ? Convert.ToDateTime(selectedPhieuMuonRow.Cells["NgayHenTra"].Value).ToString("dd/MM/yyyy")
                : "";
            string ngayMuon = selectedPhieuMuonRow.Cells["NgayMuon"].Value != null
                ? Convert.ToDateTime(selectedPhieuMuonRow.Cells["NgayMuon"].Value).ToString("dd/MM/yyyy")
                : "";

            Graphics g = e.Graphics!;
            int x = 50, y = 50, pageWidth = e.PageBounds.Width;

            Font fontTitle = new Font("Times New Roman", 16, FontStyle.Bold);
            Font fontBold = new Font("Times New Roman", 12, FontStyle.Bold);
            Font fontNormal = new Font("Times New Roman", 12);
            Font fontTable = new Font("Times New Roman", 11, FontStyle.Bold);
            Font fontTableNormal = new Font("Times New Roman", 11);

            // Tiêu đề căn giữa
            string title = "PHIẾU PHẠT TRẢ SÁCH";
            SizeF titleSize = g.MeasureString(title, fontTitle);
            float titleX = (pageWidth - titleSize.Width) / 2;
            g.DrawString(title, fontTitle, Brushes.Black, titleX, y);

            // Xuống dòng 2 ô (60px)
            y += 60;

            // In thông tin
            int infoSpacing = 30;
            int labelMaxWidth = Math.Max(Math.Max((int)g.MeasureString("Họ và tên:", fontBold).Width, (int)g.MeasureString("Địa chỉ:", fontBold).Width), (int)g.MeasureString("Email:", fontBold).Width) + 10;

            g.DrawString("Họ và tên:", fontBold, Brushes.Black, x, y);
            g.DrawString(hoTen, fontNormal, Brushes.Black, x + labelMaxWidth, y);
            y += infoSpacing;

            g.DrawString("Địa chỉ:", fontBold, Brushes.Black, x, y);
            g.DrawString(diaChi, fontNormal, Brushes.Black, x + labelMaxWidth, y);
            y += infoSpacing;

            g.DrawString("Email:", fontBold, Brushes.Black, x, y);
            g.DrawString(email, fontNormal, Brushes.Black, x + labelMaxWidth, y);
            y += infoSpacing;

            // Vẽ bảng
            y += 10;
            int[] colWidths = { 40, 80, 200, 100, 100, 100, 150 }; // Thêm cột Ngày trả
            string[] headers = { "STT", "Mã sách", "Tên sách", "Số lượng", "Ngày trả", "Số ngày trễ", "Phí phạt" };

            int tableX = x;
            int tableY = y;
            int rowHeight = 30;
            int numRows = dgvChiTietPhieuMuon.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);

            // Vẽ tiêu đề cột in đậm, căn giữa
            for (int i = 0; i < headers.Length; i++)
            {
                g.DrawRectangle(Pens.Black, tableX, tableY, colWidths[i], rowHeight);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(headers[i], fontTable, Brushes.Black, new RectangleF(tableX, tableY, colWidths[i], rowHeight), sf);
                tableX += colWidths[i];
            }
            tableY += rowHeight;

            // Tính tổng phí phạt
            decimal tongPhiPhat = 0;
            foreach (DataGridViewRow row in dgvChiTietPhieuMuon.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["PhiPhat"].Value != null)
                    tongPhiPhat += Convert.ToDecimal(row.Cells["PhiPhat"].Value);
            }

            // Vẽ các dòng dữ liệu, cột Phí phạt gộp lại thành 1 ô
            int stt = 1;
            int dataStartY = tableY;
            foreach (DataGridViewRow row in dgvChiTietPhieuMuon.Rows)
            {
                if (row.IsNewRow) continue;
                tableX = x;
                string maSach = row.Cells["MaSach"].Value?.ToString() ?? "";
                string tenSach = row.Cells["TenSach"].Value?.ToString() ?? "";
                string soLuong = row.Cells["SoLuongSachMuon"].Value?.ToString() ?? "";
                string ngayTraStr = row.Cells["NgayTra"].Value != null
                    ? Convert.ToDateTime(row.Cells["NgayTra"].Value).ToString("dd/MM/yyyy")
                    : "";
                int soNgayTre = 0;
                DateTime ngayTra = DateTime.MinValue;
                DateTime henTra = DateTime.MinValue;
                if (row.Cells["NgayTra"].Value != null && DateTime.TryParse(row.Cells["NgayTra"].Value.ToString(), out ngayTra)
                    && selectedPhieuMuonRow.Cells["NgayHenTra"].Value != null && DateTime.TryParse(selectedPhieuMuonRow.Cells["NgayHenTra"].Value.ToString(), out henTra))
                {
                    if (ngayTra > henTra)
                        soNgayTre = (ngayTra - henTra).Days;
                }

                string[] values = { stt.ToString(), maSach, tenSach, soLuong, ngayTraStr, soNgayTre.ToString() };
                for (int i = 0; i < values.Length; i++)
                {
                    g.DrawRectangle(Pens.Black, tableX, tableY, colWidths[i], rowHeight);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = (i == 2) ? StringAlignment.Near : StringAlignment.Center; // Tên sách căn trái
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(values[i], fontTableNormal, Brushes.Black, new RectangleF(tableX, tableY, colWidths[i], rowHeight), sf);
                    tableX += colWidths[i];
                }
                // Không vẽ cột phí phạt ở đây
                tableY += rowHeight;
                stt++;
            }

            // Vẽ ô phí phạt gộp, nhưng chỉ lấy giá trị của dòng đầu tiên
            int phiPhatX = x + colWidths.Take(6).Sum();
            int phiPhatY = dataStartY;
            int phiPhatHeight = rowHeight * numRows;
            g.DrawRectangle(Pens.Black, phiPhatX, phiPhatY, colWidths[6], phiPhatHeight);
            StringFormat sfPhiPhat = new StringFormat();
            sfPhiPhat.Alignment = StringAlignment.Center;
            sfPhiPhat.LineAlignment = StringAlignment.Center;

            // Lấy phí phạt của dòng đầu tiên (nếu có)
            decimal phiPhatValue = 0;
            foreach (DataGridViewRow row in dgvChiTietPhieuMuon.Rows)
            {
                if (!row.IsNewRow && row.Cells["PhiPhat"].Value != null)
                {
                    phiPhatValue = Convert.ToDecimal(row.Cells["PhiPhat"].Value);
                    break; // Chỉ lấy dòng đầu tiên
                }
            }
            string phiPhatStr = phiPhatValue.ToString("#,##0 VNĐ");
            g.DrawString(phiPhatStr, fontTableNormal, Brushes.Black, new RectangleF(phiPhatX, phiPhatY, colWidths[6], phiPhatHeight), sfPhiPhat);

            // Ngày mượn, ngày hẹn trả in đậm, căn phải sát lề phải
            y = tableY + 20;
            int infoRightX = pageWidth - x - 150;
            g.DrawString($"Ngày mượn: {ngayMuon}", fontBold, Brushes.Black, infoRightX, y); y += 25;
            g.DrawString($"Ngày hẹn trả: {ngayHenTra}", fontBold, Brushes.Black, infoRightX, y); y += 25;

            // Tạo khoảng trống phía dưới
            y += 40;

            // Thông tin QR
            int qrSize = 180;
            int qrX = (pageWidth - qrSize) / 2; // Căn giữa QR theo chiều ngang

            if (qrBitmapForPrint == null || qrBitmapForPrint.Width == 0 || qrBitmapForPrint.Height == 0)
            {
                g.DrawString("Không lấy được dữ liệu QR!", fontNormal, Brushes.Red, qrX, y);
                return;
            }

            // Vẽ QR code căn giữa
            g.DrawImage(qrBitmapForPrint, qrX, y, qrSize, qrSize);

            // Ghi chú dưới QR
            using (Font fontNote = new Font("Times New Roman", 10, FontStyle.Italic))
            {
                g.DrawString("Quét mã QR để thanh toán phí phạt", fontNote, Brushes.Black, qrX - 15, y + qrSize + 5);
            }

            y += 20;

            // Thông tin ngân hàng dưới QR, căn giữa
            string bankInfo1 = "NGÂN HÀNG: TECHCOMBANK";
            string bankInfo2 = "SỐ TÀI KHOẢN: 150212300424";
            string bankInfo3 = "CHỦ TÀI KHOẢN: VO NGUYEN";

            int infoY = y + qrSize + 30;
            using (Font fontBank = new Font("Times New Roman", 11, FontStyle.Bold))
            {
                SizeF size1 = g.MeasureString(bankInfo1, fontBank);
                SizeF size2 = g.MeasureString(bankInfo2, fontBank);
                SizeF size3 = g.MeasureString(bankInfo3, fontBank);

                int infoX1 = (pageWidth - (int)size1.Width) / 2;
                int infoX2 = (pageWidth - (int)size2.Width) / 2;
                int infoX3 = (pageWidth - (int)size3.Width) / 2;

                g.DrawString(bankInfo1, fontBank, Brushes.Black, infoX1, infoY);
                g.DrawString(bankInfo2, fontBank, Brushes.Black, infoX2, infoY + 22);
                g.DrawString(bankInfo3, fontBank, Brushes.Black, infoX3, infoY + 44);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        // ===============================================
        // 🔍 PHƯƠNG THỨC DEBUG - XÓA SAU KHI SỬA XONG
        // ===============================================
        private void TestGetPhieuMuonQuaHan()
        {
            try
            {
                StringBuilder debug = new StringBuilder();
                debug.AppendLine("🔍 DEBUG: Test GetPhieuMuonQuaHan()\n");

                PhieuMuonRepository repo = new PhieuMuonRepository();
                var danhSach = repo.GetPhieuMuonQuaHan();

                debug.AppendLine($"✅ Số lượng: {danhSach.Count}\n");

                if (danhSach.Count == 0)
                {
                    debug.AppendLine("⚠️ KHÔNG CÓ PHIẾU MƯỢN QUÁ HẠN!");
                    debug.AppendLine("\nChạy script trong TestEmailDebug.sql để tạo dữ liệu test.");
                }
                else
                {
                    var firstItem = danhSach[0];
                    var type = firstItem.GetType();

                    debug.AppendLine("📋 Các thuộc tính:");
                    foreach (var prop in type.GetProperties())
                    {
                        var value = prop.GetValue(firstItem);
                        debug.AppendLine($"  • {prop.Name} = {value}");
                    }

                    // Kiểm tra Email
                    var emailProp = type.GetProperty("Email");
                    if (emailProp != null)
                    {
                        debug.AppendLine($"\n✅ CÓ CỘT EMAIL!");
                        debug.AppendLine($"   Giá trị: {emailProp.GetValue(firstItem)}");
                    }
                    else
                    {
                        debug.AppendLine("\n❌ KHÔNG CÓ CỘT EMAIL!");
                        debug.AppendLine("\n⚠️ BẠN CHƯA REBUILD SOLUTION!");
                        debug.AppendLine("Hãy làm:");
                        debug.AppendLine("1. Build → Clean Solution");
                        debug.AppendLine("2. Build → Rebuild Solution");
                    }
                }

                MessageBox.Show(debug.ToString(), "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi debug: {ex.Message}\n\n{ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

