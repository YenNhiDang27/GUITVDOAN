using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThuVien.Repository;
using ThuVien.Services;
using ThuVien.Models;

namespace GUITV
{
    public partial class frmGuiOTP : Form
    {
        private EmailService _emailService;
        private OTPService _otpService;
        private BanDocRepository _banDocRepository;
        private Dictionary<string, string> _otpDictionary; // Email -> OTP
        private string _senderEmail = "yourname@gmail.com"; // Email của bạn
        private string _senderPassword = "xxxx xxxx xxxx xxxx"; // Mật khẩu ứng dụng 16 ký tự
        private string senderEmail = "yourname@gmail.com"; // Email của bạn
        private string senderPassword = "xxxx xxxx xxxx xxxx"; // Mật khẩu ứng dụng 16 ký tự

        public frmGuiOTP()
        {
            InitializeComponent();
            _emailService = new EmailService();
            _otpService = new OTPService();
            _banDocRepository = new BanDocRepository();
            _otpDictionary = new Dictionary<string, string>();
        }

        private void frmGuiOTP_Load(object sender, EventArgs e)
        {
            LoadBanDocList();
        }

        private void LoadBanDocList()
        {
            var danhSachBanDoc = _banDocRepository.GetAll();
            
            // Hiển thị danh sách bạn đọc vào CheckedListBox
            clbBanDoc.Items.Clear();
            foreach (var banDoc in danhSachBanDoc)
            {
                if (!string.IsNullOrWhiteSpace(banDoc.Email))
                {
                    string displayText = $"{banDoc.HoTen} - {banDoc.Email}";
                    clbBanDoc.Items.Add(displayText);
                    clbBanDoc.Tag = danhSachBanDoc;
                }
            }
        }

        private async void btnGuiOTP_Click(object sender, EventArgs e)
        {
            // Lấy danh sách email được chọn
            List<string> selectedEmails = new List<string>();
            var danhSachBanDoc = clbBanDoc.Tag as List<BanDoc>;

            if (danhSachBanDoc == null || clbBanDoc.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một bạn đọc!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy email từ các mục được chọn
            foreach (int index in clbBanDoc.CheckedIndices)
            {
                var banDoc = danhSachBanDoc[index];
                if (!string.IsNullOrWhiteSpace(banDoc.Email))
                {
                    selectedEmails.Add(banDoc.Email);
                }
            }

            if (selectedEmails.Count == 0)
            {
                MessageBox.Show("Không có email hợp lệ được chọn!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Disable button để tránh gửi nhiều lần
            btnGuiOTP.Enabled = false;
            btnGuiOTP.Text = "Đang gửi...";

            try
            {
                _otpDictionary.Clear();
                int successCount = 0;
                int failCount = 0;
                StringBuilder resultMessage = new StringBuilder();

                foreach (string email in selectedEmails)
                {
                    // Tạo mã OTP cho từng email
                    string otp = _otpService.GenerateOTP();
                    _otpService.StoreOTP(email, otp);
                    _otpDictionary[email] = otp;

                    // Tạo nội dung email
                    string subject = "Mã OTP xác thực - Hệ thống Thư Viện";
                    string body = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif;'>
                            <div style='background-color: #f5f5f5; padding: 20px;'>
                                <div style='background-color: white; padding: 30px; border-radius: 10px; max-width: 600px; margin: 0 auto;'>
                                    <h2 style='color: #2196F3; text-align: center;'>Mã OTP Xác Thực</h2>
                                    <p>Xin chào,</p>
                                    <p>Bạn đã yêu cầu mã OTP để xác thực tài khoản trong hệ thống Thư Viện.</p>
                                    <div style='background-color: #e3f2fd; padding: 20px; text-align: center; margin: 20px 0; border-radius: 5px;'>
                                        <h1 style='color: #1976D2; letter-spacing: 5px; margin: 0;'>{otp}</h1>
                                    </div>
                                    <p><strong>Lưu ý:</strong></p>
                                    <ul>
                                        <li>Mã OTP có hiệu lực trong <strong>5 phút</strong></li>
                                        <li>Không chia sẻ mã này với bất kỳ ai</li>
                                        <li>Bạn có tối đa 3 lần nhập</li>
                                    </ul>
                                    <p style='color: #666; font-size: 12px; margin-top: 30px;'>
                                        Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email này.
                                    </p>
                                </div>
                            </div>
                        </body>
                        </html>";

                    // Gửi email
                    bool result = await _emailService.SendEmailAsync(email, subject, body);
                    
                    if (result)
                    {
                        successCount++;
                        resultMessage.AppendLine($"✓ Đã gửi đến: {email}");
                    }
                    else
                    {
                        failCount++;
                        resultMessage.AppendLine($"✗ Gửi thất bại: {email}");
                    }

                    // Delay nhỏ giữa các lần gửi để tránh spam
                    await Task.Delay(500);
                }

                // Hiển thị kết quả
                string summary = $"Tổng kết:\n- Thành công: {successCount}\n- Thất bại: {failCount}\n\n{resultMessage}";
                MessageBox.Show(summary, "Kết quả gửi OTP", MessageBoxButtons.OK, 
                    successCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (successCount > 0)
                {
                    // Chuyển sang tab xác thực
                    tabControl1.SelectedTab = tabXacThuc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi OTP: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGuiOTP.Enabled = true;
                btnGuiOTP.Text = "Gửi OTP";
            }
        }

        private void btnXacThuc_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string otp = txtOTP.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(otp))
            {
                MessageBox.Show("Vui lòng nhập mã OTP!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác thực OTP
            var (isValid, message) = _otpService.ValidateOTP(email, otp);

            if (isValid)
            {
                MessageBox.Show(message, "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Clear();
                txtOTP.Clear();
            }
            else
            {
                MessageBox.Show(message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChonTatCa_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbBanDoc.Items.Count; i++)
            {
                clbBanDoc.SetItemChecked(i, true);
            }
        }

        private void btnBoChonTatCa_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbBanDoc.Items.Count; i++)
            {
                clbBanDoc.SetItemChecked(i, false);
            }
        }
    }
}