using System;
using System.Windows.Forms;
using ThuVien.Utilies;

namespace ThuVien.View
{
    /// <summary>
    /// Form test gửi email NHANH - Chỉ cần chạy và nhấn nút
    /// </summary>
    public partial class frmTestEmailNhanh : Form
    {
        public frmTestEmailNhanh()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "🚀 TEST GỬI EMAIL NHANH";
            this.Size = new System.Drawing.Size(500, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            // Label hướng dẫn
            Label lblHuongDan = new Label
            {
                Text = "📧 NHẤN NÚT DƯỚI ĐỂ GỬI EMAIL TEST\n\nEmail sẽ được gửi đến: ngoctienvx27@gmail.com",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(440, 80),
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(0, 122, 204),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblHuongDan);

            // Nút gửi email
            Button btnGui = new Button
            {
                Text = "📧 GỬI EMAIL TEST NGAY",
                Location = new System.Drawing.Point(100, 120),
                Size = new System.Drawing.Size(300, 60),
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGui.Click += BtnGui_Click;
            this.Controls.Add(btnGui);

            // Label kết quả
            Label lblKetQua = new Label
            {
                Text = "",
                Location = new System.Drawing.Point(20, 200),
                Size = new System.Drawing.Size(440, 50),
                Font = new System.Drawing.Font("Segoe UI", 10),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            lblKetQua.Name = "lblKetQua";
            this.Controls.Add(lblKetQua);
        }

        private async void BtnGui_Click(object? sender, EventArgs e)
        {
            Label? lblKetQua = this.Controls["lblKetQua"] as Label;
            Button? btnGui = sender as Button;

            if (btnGui != null) btnGui.Enabled = false;
            if (lblKetQua != null)
            {
                lblKetQua.Text = "⏳ Đang gửi email...";
                lblKetQua.ForeColor = System.Drawing.Color.Orange;
            }

            this.Cursor = Cursors.WaitCursor;

            try
            {
                EmailService emailService = new EmailService();
                
                string emailNhan = "ngoctienvx27@gmail.com";
                string tieuDe = "✅ TEST EMAIL - Thư viện";
                string noiDung = @"Xin chào!

Đây là email TEST từ hệ thống thư viện.

Nếu bạn nhận được email này, nghĩa là:
✅ Cấu hình email THÀNH CÔNG
✅ App Password hoạt động tốt
✅ Hệ thống sẵn sàng gửi email thực tế

Bây giờ bạn có thể gửi email nhắc nhở quá hạn!

---
Thư Viện
" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                await emailService.GuiEmailAsync(noiDung, emailNhan, tieuDe);

                if (lblKetQua != null)
                {
                    lblKetQua.Text = $"✅ GỬI THÀNH CÔNG!\nKiểm tra email: {emailNhan}";
                    lblKetQua.ForeColor = System.Drawing.Color.Green;
                }

                MessageBox.Show(
                    $"✅ EMAIL ĐÃ ĐƯỢC GỬI THÀNH CÔNG!\n\n" +
                    $"📧 Đến: {emailNhan}\n" +
                    $"📝 Tiêu đề: {tieuDe}\n\n" +
                    $"Vui lòng kiểm tra hộp thư của bạn (có thể trong Spam).\n\n" +
                    $"Bây giờ bạn có thể gửi email nhắc nhở quá hạn!",
                    "🎉 Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                if (lblKetQua != null)
                {
                    lblKetQua.Text = "❌ GỬI THẤT BẠI!";
                    lblKetQua.ForeColor = System.Drawing.Color.Red;
                }

                string message = ex.Message;
                string goiY = "";

                if (message.Contains("authentication") || message.Contains("Authentication"))
                {
                    goiY = "\n\n💡 Gợi ý:\n" +
                           "1. Kiểm tra App Password trong EmailService.cs\n" +
                           "2. Đảm bảo đã bật 2FA cho Gmail\n" +
                           "3. Tạo lại App Password mới";
                }
                else if (message.Contains("timeout") || message.Contains("Timeout"))
                {
                    goiY = "\n\n💡 Gợi ý:\n" +
                           "1. Kiểm tra kết nối internet\n" +
                           "2. Tắt Firewall/Antivirus tạm thời\n" +
                           "3. Thử lại";
                }

                MessageBox.Show(
                    $"❌ LỖI GỬI EMAIL:\n\n{message}{goiY}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                if (btnGui != null) btnGui.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
