using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;
using ThuVien.Repository;
using ThuVien.Services;

namespace ThuVien.Controller
{
    /// <summary>
    /// Controller xử lý các chức năng AI Chatbot
    /// </summary>
    public class AIChatController
    {
        private readonly AIService aiService;
        private readonly LichSuChatRepository lichSuRepo;
        private readonly GoiYSachRepository goiYRepo;
        private readonly ThoiQuanDocSachRepository thoiQuanRepo;
        private readonly SachRepository sachRepo;

        public AIChatController()
        {
            aiService = new AIService();
            lichSuRepo = new LichSuChatRepository();
            goiYRepo = new GoiYSachRepository();
            thoiQuanRepo = new ThoiQuanDocSachRepository();
            sachRepo = new SachRepository();
        }

        /// <summary>
        /// Xử lý tin nhắn từ người dùng
        /// </summary>
        public async Task<string> XuLyTinNhanAsync(string tinNhan, int? maBanDoc = null)
        {
            try
            {
                // Phân tích loại yêu cầu
                string loaiYeuCau = await aiService.PhanTichYeuCauAsync(tinNhan);

                // Lấy thông tin bối cảnh
                string systemPrompt = TaoSystemPrompt(maBanDoc, loaiYeuCau);

                // Gọi AI
                string phanHoi = await aiService.ChatAsync(tinNhan, systemPrompt);

                // Lưu lịch sử
                var lichSu = new LichSuChat
                {
                    MaBanDoc = maBanDoc,
                    NoiDungNguoiDung = tinNhan,
                    NoiDungAI = phanHoi,
                    ThoiGian = DateTime.Now,
                    LoaiYeuCau = loaiYeuCau
                };
                lichSuRepo.Insert(lichSu);

                return phanHoi;
            }
            catch (Exception ex)
            {
                return $"❌ Xin lỗi, đã có lỗi xảy ra: {ex.Message}\n\n" +
                       $"💡 Vui lòng kiểm tra cấu hình AI trong Services\\AIService.cs";
            }
        }

        /// <summary>
        /// Tạo system prompt dựa trên bối cảnh
        /// </summary>
        private string TaoSystemPrompt(int? maBanDoc, string loaiYeuCau)
        {
            // Lấy danh sách sách
            var danhSachSach = sachRepo.GetAll()
                .Select(s => $"- {s.TenSach} ({s.TacGia}) - {s.TenLoai}")
                .ToList();

            // Lấy thói quen đọc nếu có
            string thoiQuanDoc = null;
            if (maBanDoc.HasValue)
            {
                var thoiQuan = thoiQuanRepo.GetByMaBanDoc(maBanDoc.Value);
                if (thoiQuan != null)
                {
                    thoiQuanDoc = $@"
Số sách đã mượn: {thoiQuan.SoLuongSachDaMuon}
Số sách đã đọc: {thoiQuan.SoLuongSachDaDoc}
Thể loại ưa thích: {thoiQuan.TheLoaiUaThich}
Tác giả ưa thích: {thoiQuan.TacGiaUaThich}
Trung bình số trang: {thoiQuan.TrungBinhSoTrang:F0}";
                }
            }

            return aiService.TaoSystemPromptThuVien(danhSachSach, thoiQuanDoc);
        }

        /// <summary>
        /// Lấy gợi ý sách thông minh cho bạn đọc
        /// </summary>
        public async Task<List<GoiYSach>> LayGoiYSachAsync(int maBanDoc, int soLuong = 5)
        {
            try
            {
                // Cập nhật thói quen đọc
                thoiQuanRepo.CapNhatTuLichSuMuon(maBanDoc);
                
                // Lấy thói quen
                var thoiQuan = thoiQuanRepo.GetByMaBanDoc(maBanDoc);
                if (thoiQuan == null)
                {
                    return new List<GoiYSach>();
                }

                // Tạo prompt gợi ý
                var danhSachSach = sachRepo.GetAll();
                var sachChuaMuon = danhSachSach.Take(20).ToList(); // Lấy 20 quyển để phân tích

                string prompt = $@"Dựa vào thói quen đọc sau:
Thể loại ưa thích: {thoiQuan.TheLoaiUaThich}
Tác giả ưa thích: {thoiQuan.TacGiaUaThich}
Trung bình số trang: {thoiQuan.TrungBinhSoTrang:F0}

Hãy gợi ý {soLuong} cuốn sách phù hợp nhất từ danh sách:
{string.Join("\n", sachChuaMuon.Select(s => $"{s.MaSach}. {s.TenSach} - {s.TacGia} ({s.TenLoai})"))}

Trả về theo format: MaSach|Lý do (mỗi sách 1 dòng)";

                var response = await aiService.ChatAsync(prompt, null);

                // Parse kết quả và lưu gợi ý
                var goiYList = new List<GoiYSach>();
                var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int maSach))
                    {
                        var goiY = new GoiYSach
                        {
                            MaBanDoc = maBanDoc,
                            MaSach = maSach,
                            DiemGoiY = 0.8m,
                            LyDoGoiY = parts[1].Trim(),
                            NgayGoiY = DateTime.Now,
                            DaXem = false,
                            DaMuon = false
                        };
                        goiYRepo.Insert(goiY);
                        goiYList.Add(goiY);
                    }
                }

                return goiYList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo gợi ý: {ex.Message}");
            }
        }

        /// <summary>
        /// Dự đoán sách sẽ được ưa chuộng
        /// </summary>
        public async Task<string> DuDoanXuHuongAsync()
        {
            try
            {
                // Lấy thống kê sách được mượn nhiều
                var danhSachSach = sachRepo.GetAll()
                    .OrderByDescending(s => s.SoLuong)
                    .Take(20)
                    .Select(s => $"- {s.TenSach} ({s.TacGia}) - Thể loại: {s.TenLoai}")
                    .ToList();

                string prompt = $@"Dựa vào danh sách sách phổ biến hiện tại:
{string.Join("\n", danhSachSach)}

Hãy phân tích và dự đoán:
1. Xu hướng đọc sách hiện tại
2. Thể loại sách sẽ được ưa chuộng trong 3-6 tháng tới
3. Gợi ý 3-5 thể loại/chủ đề nên bổ sung vào thư viện

Giữ câu trả lời ngắn gọn, dễ hiểu.";

                return await aiService.ChatAsync(prompt, null);
            }
            catch (Exception ex)
            {
                return $"Không thể dự đoán xu hướng: {ex.Message}";
            }
        }

        /// <summary>
        /// Lấy lịch sử chat
        /// </summary>
        public List<LichSuChat> LayLichSuChat(int maBanDoc, int soLuong = 20)
        {
            return lichSuRepo.GetByMaBanDoc(maBanDoc, soLuong);
        }

        /// <summary>
        /// Cập nhật thói quen đọc
        /// </summary>
        public void CapNhatThoiQuanDoc(int maBanDoc)
        {
            thoiQuanRepo.CapNhatTuLichSuMuon(maBanDoc);
        }
    }
}
