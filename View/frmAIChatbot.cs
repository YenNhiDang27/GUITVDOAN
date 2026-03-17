using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien.Controller;
using ThuVien.Utilies;

namespace ThuVien.View
{
    public partial class frmAIChatbot : Form
    {
        private const string EMAIL_GUI = "library.system@gmail.com";  // Email thật của bạn
        private const string MAT_KHAU_UNG_DUNG = "abcd efgh ijkl mnop"; // App Password 16 ký tự

        private readonly AIChatController aiController;
        private int? maBanDoc;

        public frmAIChatbot()
        {
            InitializeComponent();
            aiController = new AIChatController();
        }

        // Lấy mã bạn đọc từ AppState nếu đã đăng nhập


        private void frmAIChatbot_Load(object sender, EventArgs e)
        {
            // Hiển thị lời chào
            ThemTinNhan("AI", "Xin chào! 👋 Tôi là trợ lý AI của thư viện.\n\n" +
                "Tôi có thể giúp bạn:\n" +
                "✨ Gợi ý sách phù hợp với sở thích\n" +
                "🔍 Tìm kiếm sách theo chủ đề\n" +
                "📊 Phân tích xu hướng đọc sách\n\n" +
                "Hãy đặt câu hỏi hoặc nhấn các nút bên dưới!", Color.FromArgb(46, 204, 113));

            // Tải lịch sử chat nếu có
            if (maBanDoc.HasValue)
            {
                TaiLichSuChat();
            }

            // Focus vào ô nhập
            txtInput.Focus();
        }

        private void TaiLichSuChat()
        {
            try
            {
                var lichSu = aiController.LayLichSuChat(maBanDoc.Value, 5);
                if (lichSu.Count > 0)
                {
                    ThemTinNhan("AI", "📜 Lịch sử trò chuyện gần đây:", Color.Gray);

                    foreach (var chat in lichSu)
                    {
                        ThemTinNhan("Bạn", chat.NoiDungNguoiDung, Color.FromArgb(0, 122, 204));
                        ThemTinNhan("AI", chat.NoiDungAI, Color.FromArgb(46, 204, 113));
                    }

                    ThemTinNhan("AI", "━━━━━━━━━━━━━━━━━━━━", Color.LightGray);
                }
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi khi tải lịch sử
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await GuiTinNhan();
        }

        private async void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                await GuiTinNhan();
            }
        }

        private async Task GuiTinNhan()
        {
            string tinNhan = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(tinNhan))
            {
                MessageBox.Show("Vui lòng nhập câu hỏi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị tin nhắn người dùng
            ThemTinNhan("Bạn", tinNhan, Color.FromArgb(0, 122, 204));
            txtInput.Clear();

            // Hiển thị loading
            HienThiLoading(true);

            try
            {
                // Gọi AI
                string phanHoi = await aiController.XuLyTinNhanAsync(tinNhan, maBanDoc);

                // Hiển thị phản hồi
                ThemTinNhan("AI", phanHoi, Color.FromArgb(46, 204, 113));
            }
            catch (Exception ex)
            {
                ThemTinNhan("Lỗi", ex.Message, Color.Red);
            }
            finally
            {
                HienThiLoading(false);
                txtInput.Focus();
            }
        }

        private async void btnGoiY_Click(object sender, EventArgs e)
        {
            if (!maBanDoc.HasValue)
            {
                MessageBox.Show("Bạn cần đăng nhập để sử dụng tính năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            HienThiLoading(true);

            try
            {
                ThemTinNhan("Bạn", "💡 Gợi ý sách cho tôi", Color.FromArgb(0, 122, 204));

                // Cập nhật thói quen đọc
                aiController.CapNhatThoiQuanDoc(maBanDoc.Value);

                // Lấy gợi ý
                var goiYList = await aiController.LayGoiYSachAsync(maBanDoc.Value, 5);

                if (goiYList.Count > 0)
                {
                    string message = "📚 Dựa trên thói quen đọc của bạn, tôi gợi ý:\n\n";
                    foreach (var goiY in goiYList)
                    {
                        message += $"✨ Lý do: {goiY.LyDoGoiY}\n\n";
                    }
                    ThemTinNhan("AI", message, Color.FromArgb(46, 204, 113));
                }
                else
                {
                    ThemTinNhan("AI", "Bạn chưa có đủ dữ liệu để tôi gợi ý. Hãy mượn thêm sách nhé! 📖",
                        Color.FromArgb(230, 126, 34));
                }
            }
            catch (Exception ex)
            {
                ThemTinNhan("Lỗi", $"Không thể tạo gợi ý: {ex.Message}", Color.Red);
            }
            finally
            {
                HienThiLoading(false);
            }
        }

        private async void btnXuHuong_Click(object sender, EventArgs e)
        {
            HienThiLoading(true);

            try
            {
                ThemTinNhan("Bạn", "📊 Phân tích xu hướng sách", Color.FromArgb(0, 122, 204));

                string xuHuong = await aiController.DuDoanXuHuongAsync();
                ThemTinNhan("AI", xuHuong, Color.FromArgb(46, 204, 113));
            }
            catch (Exception ex)
            {
                ThemTinNhan("Lỗi", $"Không thể phân tích xu hướng: {ex.Message}", Color.Red);
            }
            finally
            {
                HienThiLoading(false);
            }
        }

        private void ThemTinNhan(string nguoiGui, string noiDung, Color mauChu)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;

            // Thêm tên người gửi
            rtbChat.SelectionColor = mauChu;
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
            rtbChat.AppendText($"\n{nguoiGui}:\n");

            // Thêm nội dung
            rtbChat.SelectionColor = Color.Black;
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);
            rtbChat.AppendText($"{noiDung}\n");

            // Cuộn xuống cuối
            rtbChat.ScrollToCaret();
        }

        private void HienThiLoading(bool isLoading)
        {
            panelLoading.Visible = isLoading;
            if (isLoading)
            {
                panelLoading.BringToFront();
                // Căn giữa panel loading
                panelLoading.Left = (this.ClientSize.Width - panelLoading.Width) / 2;
                panelLoading.Top = (this.ClientSize.Height - panelLoading.Height) / 2;
            }

            btnSend.Enabled = !isLoading;
            btnGoiY.Enabled = !isLoading;
            btnXuHuong.Enabled = !isLoading;
            txtInput.Enabled = !isLoading;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
