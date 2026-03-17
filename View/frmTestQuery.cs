using System;
using System.Windows.Forms;
using ThuVien.Repository;

namespace ThuVien.View
{
    public partial class frmTestQuery : Form
    {
        private Button btnTest;
        private TextBox txtResult;

        public frmTestQuery()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Test Query GetPhieuMuonQuaHan";
            this.Size = new System.Drawing.Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnTest = new Button
            {
                Text = "🔍 Test Query",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(150, 40),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White
            };
            btnTest.Click += BtnTest_Click;
            this.Controls.Add(btnTest);

            txtResult = new TextBox
            {
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(640, 380),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new System.Drawing.Font("Consolas", 10)
            };
            this.Controls.Add(txtResult);
        }

        private void BtnTest_Click(object? sender, EventArgs e)
        {
            try
            {
                txtResult.Clear();
                txtResult.AppendText("🔍 Testing GetPhieuMuonQuaHan()...\n\n");

                PhieuMuonRepository repo = new PhieuMuonRepository();
                var danhSach = repo.GetPhieuMuonQuaHan();

                txtResult.AppendText($"✅ Số lượng phiếu quá hạn: {danhSach.Count}\n\n");

                if (danhSach.Count == 0)
                {
                    txtResult.AppendText("⚠️ KHÔNG CÓ PHIẾU MƯỢN QUÁ HẠN!\n\n");
                    txtResult.AppendText("Hãy chạy script tạo phiếu mượn test trong file TestEmailDebug.sql\n");
                    return;
                }

                // Lấy phần tử đầu tiên
                var firstItem = danhSach[0];
                var type = firstItem.GetType();

                txtResult.AppendText("📋 Các thuộc tính của dynamic object:\n");
                txtResult.AppendText("".PadRight(50, '=') + "\n");

                foreach (var prop in type.GetProperties())
                {
                    var value = prop.GetValue(firstItem);
                    txtResult.AppendText($"✓ {prop.Name}: {value}\n");
                }

                txtResult.AppendText("\n" + "".PadRight(50, '=') + "\n\n");

                // Kiểm tra cụ thể property Email
                var emailProp = type.GetProperty("Email");
                if (emailProp != null)
                {
                    txtResult.AppendText("✅ CÓ PROPERTY 'Email'!\n");
                    txtResult.AppendText($"   Giá trị: {emailProp.GetValue(firstItem)}\n\n");
                }
                else
                {
                    txtResult.AppendText("❌ KHÔNG CÓ PROPERTY 'Email'!\n\n");
                    txtResult.AppendText("⚠️ BẠN CHƯA REBUILD SOLUTION!\n");
                    txtResult.AppendText("Hãy làm:\n");
                    txtResult.AppendText("1. Build → Clean Solution\n");
                    txtResult.AppendText("2. Build → Rebuild Solution\n");
                    txtResult.AppendText("3. Chạy lại\n");
                }

                // In ra tất cả dữ liệu
                txtResult.AppendText("\n📊 DANH SÁCH PHIẾU MƯỢN QUÁ HẠN:\n");
                txtResult.AppendText("".PadRight(50, '=') + "\n");

                int stt = 1;
                foreach (var item in danhSach)
                {
                    txtResult.AppendText($"\n#{stt++}\n");
                    var itemType = item.GetType();
                    
                    var maPM = itemType.GetProperty("MaPhieuMuon")?.GetValue(item);
                    var maBD = itemType.GetProperty("MaBanDoc")?.GetValue(item);
                    var email = itemType.GetProperty("Email")?.GetValue(item);
                    var ngayHenTra = itemType.GetProperty("NgayHenTra")?.GetValue(item);

                    txtResult.AppendText($"  Mã phiếu: {maPM}\n");
                    txtResult.AppendText($"  Mã bạn đọc: {maBD}\n");
                    txtResult.AppendText($"  Email: {email}\n");
                    txtResult.AppendText($"  Ngày hẹn trả: {ngayHenTra:dd/MM/yyyy}\n");
                }

                MessageBox.Show("Test hoàn tất! Xem kết quả trong textbox.", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                txtResult.AppendText($"\n❌ LỖI:\n{ex.Message}\n\n{ex.StackTrace}");
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
