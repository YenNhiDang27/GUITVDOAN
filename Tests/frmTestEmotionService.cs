using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien.Services;
using System.Collections.Generic;

namespace ThuVien.Tests
{
    /// <summary>
    /// Form test nhanh các service cảm xúc
    /// Chạy để kiểm tra xem service hoạt động đúng không
    /// </summary>
    public partial class frmTestEmotionService : Form
    {
        private EmotionAnalysisService _emotionService;
        private EmotionBasedRecommendationService _recommendService;
        private static string connectionString = "Data Source=LAPTOP-USTARUQU;...";
        private const string OPENAI_API_KEY = "https://platform.openai.com/api-keys";
        private string _senderEmail = "your-email@gmail.com";
        private string _senderPassword = "your-app-password";

        public frmTestEmotionService()
        {
            InitializeComponent();
            _emotionService = new EmotionAnalysisService();
            _recommendService = new EmotionBasedRecommendationService();
        }

        private void InitializeComponent()
        {
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(20, 60);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(550, 80);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = "Hôm nay tôi buồn quá, cảm thấy mệt mỏi và cô đơn";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(20, 150);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(150, 40);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test Phân Tích";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(20, 200);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(550, 300);
            this.txtResult.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 25);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "🧪 Test Emotion Analysis Service";
            // 
            // frmTestEmotionService
            // 
            this.ClientSize = new System.Drawing.Size(594, 521);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtInput);
            this.Name = "frmTestEmotionService";
            this.Text = "Test Emotion Service";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblTitle;

        private async void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Text = "⏳ Đang phân tích...\r\n";
                Application.DoEvents();

                string input = txtInput.Text;
                
                // Test phân tích cảm xúc
                var phanTich = await _emotionService.PhanTichCamXucAsync(input);

                txtResult.Text = "✅ KẾT QUẢ PHÂN TÍCH:\r\n\r\n";
                txtResult.AppendText($"📝 Văn bản: {input}\r\n\r\n");
                txtResult.AppendText($"😊 Cảm xúc chính: {phanTich.CamXucChinh}\r\n");
                txtResult.AppendText($"📊 Độ tin cậy: {phanTich.DoTinCay * 100:F0}%\r\n");
                txtResult.AppendText($"💭 Lý do: {phanTich.LyDo}\r\n");
                txtResult.AppendText($"🔑 Từ khóa: {string.Join(", ", phanTich.TuKhoaCamXuc)}\r\n");
                txtResult.AppendText($"📚 Thể loại gợi ý: {phanTich.GoiYTheLoai}\r\n\r\n");

                // Test câu động viên
                string dongVien = _emotionService.TaoCauDongVien(phanTich.CamXucChinh);
                txtResult.AppendText($"💪 Động viên: {dongVien}\r\n\r\n");

                // Test lấy thể loại phù hợp
                var theLoai = _emotionService.LayTheLoaiPhuHop(phanTich.CamXucChinh);
                txtResult.AppendText($"📖 Các thể loại phù hợp:\r\n");
                foreach (var loai in theLoai)
                {
                    txtResult.AppendText($"  - {loai}\r\n");
                }

                txtResult.AppendText("\r\n✅ Test thành công!");
            }
            catch (Exception ex)
            {
                txtResult.Text = $"❌ LỖI:\r\n\r\n{ex.Message}\r\n\r\n{ex.StackTrace}";
            }
        }
    }           
}
