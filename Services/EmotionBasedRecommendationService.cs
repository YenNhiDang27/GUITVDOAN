using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.Services
{
    /// <summary>
    /// Service gợi ý sách thông minh dựa trên cảm xúc của người dùng
    /// </summary>
    public class EmotionBasedRecommendationService
    {
        private readonly SachRepository _sachRepo;
        private readonly LoaiSachRepository _loaiRepo;
        private readonly EmotionAnalysisService _emotionService;
        private readonly AIService _aiService;

        public EmotionBasedRecommendationService()
        {
            _sachRepo = new SachRepository();
            _loaiRepo = new LoaiSachRepository();
            _emotionService = new EmotionAnalysisService();
            _aiService = new AIService();
        }

        /// <summary>
        /// Gợi ý sách dựa trên trạng thái cảm xúc của người dùng
        /// </summary>
        public async Task<DanhSachGoiYTheoEmocao> GoiYSachTheoEmotionAsync(string trangThaiNguoiDung, int maBanDoc, int soLuongGoiY = 10)
        {
            try
            {
                // 1. Phân tích cảm xúc từ văn bản
                var phanTichCamXuc = await _emotionService.PhanTichCamXucAsync(trangThaiNguoiDung);

                // 2. Lấy tất cả sách có sẵn
                var tatCaSach = _sachRepo.GetAll();

                // 3. Lọc sách theo thể loại phù hợp với cảm xúc
                var theLoaiPhuHop = _emotionService.LayTheLoaiPhuHop(phanTichCamXuc.CamXucChinh);
                var sachPhuHop = LocSachTheoTheLoai(tatCaSach, theLoaiPhuHop);

                // 4. Nếu có AI, dùng AI để phân tích sâu hơn và xếp hạng
                List<SachGoiYTheoEmotion> danhSachGoiY;
                if (await KiemTraAIKhaDung())
                {
                    danhSachGoiY = await GoiYBangAI(sachPhuHop, phanTichCamXuc, soLuongGoiY);
                }
                else
                {
                    // Không có AI, dùng rule-based
                    danhSachGoiY = GoiYBangRuleBased(sachPhuHop, phanTichCamXuc, soLuongGoiY);
                }

                // 5. Tạo câu động viên
                string cauDongVien = _emotionService.TaoCauDongVien(phanTichCamXuc.CamXucChinh);

                return new DanhSachGoiYTheoEmocao
                {
                    TrangThaiNguoiDung = trangThaiNguoiDung,
                    CamXucPhanTich = phanTichCamXuc,
                    DanhSachSach = danhSachGoiY,
                    CauDongVien = cauDongVien,
                    ThoiGian = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gợi ý sách theo cảm xúc: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lọc sách theo các thể loại phù hợp
        /// </summary>
        private List<Sach> LocSachTheoTheLoai(List<Sach> tatCaSach, string[] theLoaiPhuHop)
        {
            var ketQua = new List<Sach>();
            var danhSachLoai = _loaiRepo.GetAll();

            foreach (var theLoai in theLoaiPhuHop)
            {
                var loai = danhSachLoai.FirstOrDefault(l => 
                    l.TenLoai != null && l.TenLoai.Contains(theLoai, StringComparison.OrdinalIgnoreCase));

                if (loai != null)
                {
                    var sachCuaLoai = tatCaSach.Where(s => s.MaLoai == loai.MaLoai).ToList();
                    ketQua.AddRange(sachCuaLoai);
                }
            }

            // Nếu không tìm thấy sách nào theo thể loại, trả về tất cả
            if (!ketQua.Any())
            {
                return tatCaSach.Where(s => s.SoLuong > 0).ToList();
            }

            // Loại bỏ trùng lặp và chỉ lấy sách còn hàng
            return ketQua.Where(s => s.SoLuong > 0)
                .GroupBy(s => s.MaSach)
                .Select(g => g.First())
                .ToList();
        }

        /// <summary>
        /// Gợi ý sách bằng AI (chính xác, có giải thích chi tiết)
        /// </summary>
        private async Task<List<SachGoiYTheoEmotion>> GoiYBangAI(List<Sach> sachPhuHop, PhanTichCamXuc camXuc, int soLuong)
        {
            try
            {
                // Tạo danh sách sách để AI phân tích (giới hạn 20 cuốn để không quá dài)
                var sachDePhanTich = sachPhuHop.Take(Math.Min(20, sachPhuHop.Count)).ToList();
                
                string danhSachSach = string.Join("\n", sachDePhanTich.Select(s => 
                    $"- [{s.MaSach}] {s.TenSach} - {s.TacGia} ({s.TenLoai}) - {s.GioiThieu}"));

                string systemPrompt = $@"Bạn là chuyên gia tư vấn sách, nhiệm vụ của bạn là gợi ý sách phù hợp với cảm xúc của người đọc.

Cảm xúc hiện tại: {camXuc.CamXucChinh}
Lý do: {camXuc.LyDo}
Từ khóa: {string.Join(", ", camXuc.TuKhoaCamXuc)}

Danh sách sách có sẵn:
{danhSachSach}

Hãy chọn {soLuong} cuốn sách PHÙ HỢP NHẤT và trả lời theo định dạng JSON:
[
    {{
        ""ma_sach"": 123,
        ""diem_phu_hop"": 0.95,
        ""ly_do"": ""Cuốn sách này phù hợp vì..."",
        ""doan_hay"": ""Một đoạn trích hay từ sách (nếu biết)""
    }},
    ...
]

Lưu ý: Chỉ chọn sách từ danh sách trên, không tự tạo sách mới!";

                string response = await _aiService.ChatAsync("Hãy gợi ý sách cho tôi", systemPrompt);

                // Parse JSON response
                var result = new List<SachGoiYTheoEmotion>();
                try
                {
                    var jsonMatch = System.Text.RegularExpressions.Regex.Match(response, @"\[[\s\S]*\]");
                    if (jsonMatch.Success)
                    {
                        var json = System.Text.Json.JsonDocument.Parse(jsonMatch.Value);
                        foreach (var item in json.RootElement.EnumerateArray())
                        {
                            int maSach = item.GetProperty("ma_sach").GetInt32();
                            var sach = sachPhuHop.FirstOrDefault(s => s.MaSach == maSach);
                            
                            if (sach != null)
                            {
                                result.Add(new SachGoiYTheoEmotion
                                {
                                    Sach = sach,
                                    DiemPhuHop = item.GetProperty("diem_phu_hop").GetDouble(),
                                    LyDoGoiY = item.GetProperty("ly_do").GetString() ?? "",
                                    DoanTrichHay = item.TryGetProperty("doan_hay", out var doanHay) 
                                        ? doanHay.GetString() : ""
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi parse JSON từ AI: {ex.Message}");
                }

                // Nếu AI không trả về kết quả, dùng rule-based
                if (!result.Any())
                {
                    return GoiYBangRuleBased(sachPhuHop, camXuc, soLuong);
                }

                return result.OrderByDescending(x => x.DiemPhuHop).Take(soLuong).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gợi ý bằng AI: {ex.Message}");
                return GoiYBangRuleBased(sachPhuHop, camXuc, soLuong);
            }
        }

        /// <summary>
        /// Gợi ý sách bằng rule-based (không cần AI, nhanh)
        /// </summary>
        private List<SachGoiYTheoEmotion> GoiYBangRuleBased(List<Sach> sachPhuHop, PhanTichCamXuc camXuc, int soLuong)
        {
            var result = new List<SachGoiYTheoEmotion>();

            // Tính điểm phù hợp dựa trên:
            // 1. Thể loại sách (60%)
            // 2. Từ khóa trong tên/giới thiệu (30%)
            // 3. Tác giả nổi tiếng (10%)

            foreach (var sach in sachPhuHop)
            {
                double diem = 0.0;
                var lyDo = new List<string>();

                // Điểm thể loại (base 0.6)
                diem += 0.6;
                lyDo.Add($"Thể loại '{sach.TenLoai}' phù hợp với cảm xúc '{camXuc.CamXucChinh}'");

                // Điểm từ khóa trong tên/giới thiệu
                if (camXuc.TuKhoaCamXuc != null)
                {
                    int demTuKhoa = 0;
                    foreach (var tuKhoa in camXuc.TuKhoaCamXuc)
                    {
                        if ((sach.TenSach?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (sach.GioiThieu?.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase) ?? false))
                        {
                            demTuKhoa++;
                        }
                    }
                    if (demTuKhoa > 0)
                    {
                        double diemTuKhoa = Math.Min(0.3, demTuKhoa * 0.1);
                        diem += diemTuKhoa;
                        lyDo.Add($"Nội dung sách liên quan đến cảm xúc của bạn");
                    }
                }

                // Điểm tác giả nổi tiếng (đơn giản: sách có nhiều trong thư viện = nổi tiếng)
                if (sach.SoLuong > 5)
                {
                    diem += 0.05;
                }

                // Random thêm một chút để không quá đều
                diem += new Random().NextDouble() * 0.05;

                result.Add(new SachGoiYTheoEmotion
                {
                    Sach = sach,
                    DiemPhuHop = Math.Min(1.0, diem),
                    LyDoGoiY = string.Join(". ", lyDo) + ".",
                    DoanTrichHay = ""
                });
            }

            return result.OrderByDescending(x => x.DiemPhuHop).Take(soLuong).ToList();
        }

        /// <summary>
        /// Kiểm tra AI có khả dụng không
        /// </summary>
        private async Task<bool> KiemTraAIKhaDung()
        {
            try
            {
                // Thử gọi AI với câu đơn giản
                await _aiService.ChatAsync("Test");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Kết quả gợi ý sách theo cảm xúc
    /// </summary>
    public class DanhSachGoiYTheoEmocao
    {
        public string TrangThaiNguoiDung { get; set; }
        public PhanTichCamXuc CamXucPhanTich { get; set; }
        public List<SachGoiYTheoEmotion> DanhSachSach { get; set; }
        public string CauDongVien { get; set; }
        public DateTime ThoiGian { get; set; }
    }

    /// <summary>
    /// Sách được gợi ý kèm thông tin cảm xúc
    /// </summary>
    public class SachGoiYTheoEmotion
    {
        public Sach Sach { get; set; }
        public double DiemPhuHop { get; set; } // 0-1
        public string LyDoGoiY { get; set; }
        public string DoanTrichHay { get; set; } // Đoạn trích hay từ sách
    }
}
