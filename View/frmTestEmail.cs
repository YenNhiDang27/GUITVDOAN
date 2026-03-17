using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien.Utilies;

namespace ThuVien.View
{
    /// <summary>
    /// Form test gửi email để kiểm tra cấu hình
    /// </summary>
    public partial class frmTestEmail : Form
    {
        private TextBox txtEmailNhan;
        private TextBox txtTieuDe;
        private TextBox txtNoiDung;
        private Button btnGuiTest;
        private Button btnDong;
        private Label lblKetQua;

        public frmTestEmail()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Test Gửi Email";
            this.Size = new System.Drawing.Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Label và TextBox Email nhận
            Label lblEmail = new Label
            {
                Text = "Email người nhận:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(120, 20)
            };
            this.Controls.Add(lblEmail);

            txtEmailNhan = new TextBox
            {
                Location = new System.Drawing.Point(140, 20),
                Size = new System.Drawing.Size(320, 25),
                Text = "ngoctienvx27@gmail.com" // Email mặc định
            };
            this.Controls.Add(txtEmailNhan);

            // Label và TextBox Tiêu đề
            Label lblTieuDe = new Label
            {
                Text = "Tiêu đề:",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(120, 20)
            };
            this.Controls.Add(lblTieuDe);

            txtTieuDe = new TextBox
            {
                Location = new System.Drawing.Point(140, 60),
                Size = new System.Drawing.Size(320, 25),
                Text = "Test Email - Thư viện"
            };
            this.Controls.Add(txtTieuDe);

            // Label và TextBox Nội dung
            Label lblNoiDung = new Label
            {
                Text = "Nội dung:",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(120, 20)
            };
            this.Controls.Add(lblNoiDung);

            txtNoiDung = new TextBox
            {
                Location = new System.Drawing.Point(140, 100),
                Size = new System.Drawing.Size(320, 120),
                Multiline = true,
                Text = "Xin chào!\n\nĐây là email test từ hệ thống thư viện.\n\nNếu bạn nhận được email này, nghĩa là cấu hình email đã thành công! ✅\n\nTrân trọng,\nHệ thống thư viện"
            };
            this.Controls.Add(txtNoiDung);

            // Nút Gửi Test
            btnGuiTest = new Button
            {
                Text = "📧 Gửi Email Test",
                Location = new System.Drawing.Point(140, 240),
                Size = new System.Drawing.Size(150, 40),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuiTest.Click += BtnGuiTest_Click;
            this.Controls.Add(btnGuiTest);

            // Nút Đóng
            btnDong = new Button
            {
                Text = "❌ Đóng",
                Location = new System.Drawing.Point(310, 240),
                Size = new System.Drawing.Size(150, 40),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDong.Click += (s, e) => this.Close();
            this.Controls.Add(btnDong);

            // Label kết quả
            lblKetQua = new Label
            {
                Location = new System.Drawing.Point(20, 300),
                Size = new System.Drawing.Size(460, 80),
                Text = "💡 Nhập email của bạn và click 'Gửi Email Test'",
                ForeColor = System.Drawing.Color.Blue,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new Padding(10)
            };
            this.Controls.Add(lblKetQua);
        }

        private async void BtnGuiTest_Click(object? sender, EventArgs e)
        {
            string emailNhan = txtEmailNhan.Text.Trim();
            string tieuDe = txtTieuDe.Text.Trim();
            string noiDung = txtNoiDung.Text.Trim();

            // Kiểm tra input
            if (string.IsNullOrEmpty(emailNhan))
            {
                MessageBox.Show("Vui lòng nhập email người nhận!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!EmailService.IsValidEmail(emailNhan))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Disable nút và hiển thị loading
            btnGuiTest.Enabled = false;
            btnDong.Enabled = false;
            lblKetQua.Text = "⏳ Đang gửi email...";
            lblKetQua.ForeColor = System.Drawing.Color.Orange;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                EmailService emailService = new EmailService();
                bool success = await emailService.GuiEmailAsync(noiDung, emailNhan, tieuDe);

                if (success)
                {
                    lblKetQua.Text = $"✅ GỬI THÀNH CÔNG!\n\nEmail đã được gửi đến: {emailNhan}\n\nVui lòng kiểm tra hộp thư của bạn (có thể trong Spam).";
                    lblKetQua.ForeColor = System.Drawing.Color.Green;
                    MessageBox.Show($"Gửi email thành công!\n\nĐã gửi đến: {emailNhan}\n\nKiểm tra hộp thư của bạn.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblKetQua.Text = "❌ GỬI THẤT BẠI!\n\nVui lòng kiểm tra lại cấu hình.";
                    lblKetQua.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblKetQua.Text = $"❌ LỖI:\n\n{ex.Message}";
                lblKetQua.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Lỗi gửi email:\n\n{ex.Message}\n\nVui lòng kiểm tra:\n1. App Password đúng chưa?\n2. Đã bật 2FA cho Gmail chưa?\n3. Kết nối internet OK chưa?", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGuiTest.Enabled = true;
                btnDong.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
