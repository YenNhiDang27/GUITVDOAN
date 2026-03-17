using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThuVien.Models;

namespace ThuVien.Services
{
    /// <summary>
    /// Service phân tích cảm xúc từ văn bản người dùng
    /// Sử dụng kết hợp Rule-Based và AI để nhận diện cảm xúc chính xác
    /// </summary>
    public class EmotionAnalysisService
    {
        private readonly AIService _aiService;

        // Từ điển cảm xúc tiếng Việt
        private readonly Dictionary<string, List<string>> _tuDienCamXuc = new Dictionary<string, List<string>>
        {
            ["buồn"] = new List<string> { "buồn", "buồn bã", "cô đơn", "cô độc", "trầm cảm", "chán nản", 
                "tuyệt vọng", "đau khổ", "khổ tâm", "đau đớn", "thất vọng", "thất bại", "mất mát", 
                "chia tay", "nhớ", "tiếc nuối", "ray rứt", "thương tâm", "bi ai", "sầu muộn", 
                "u ám", "tối tăm", "tuyệt vọng", "mệt mỏi", "kiệt sức" },
            
            ["vui"] = new List<string> { "vui", "vui vẻ", "hạnh phúc", "hân hoan", "phấn khởi", 
                "phấn khích", "hứng khởi", "hào hứng", "yêu đời", "tích cực", "lạc quan", 
                "tuyệt vời", "tốt đẹp", "may mắn", "thành công", "chiến thắng", "đạt được", 
                "vượt qua", "hoàn thành", "tự hào", "sung sướng", "rạng rỡ", "rực rỡ", 
                "vẻ vang", "huy hoàng", "tươi sáng" },
            
            ["lo lắng"] = new List<string> { "lo lắng", "lo âu", "âu lo", "căng thẳng", "stress", 
                "áp lực", "sợ hãi", "hoảng sợ", "bối rối", "hoang mang", "lúng túng", 
                "không biết", "mơ hồ", "mông lung", "bất an", "hồi hộp", "bồn chồn", 
                "xao xuyến", "dằn vặt", "day dứt", "trăn trở", "băn khoăn", "phân vân", 
                "lưỡng lự", "do dự", "nghi ngờ", "hoài nghi", "sợ" },
            
            ["tức giận"] = new List<string> { "tức giận", "giận dữ", "tức", "giận", "bực", 
                "bực bội", "bực mình", "khó chịu", "phật ý", "cáu gắt", "cáu kỉnh", 
                "nóng giận", "phẫn nộ", "căm phẫn", "căm ghét", "căm tức", "căm hờn", 
                "oán giận", "oán hận", "hận thù", "thù địch", "không thích", "chán", 
                "kinh tởm", "gớm ghiếc", "ghê tởm", "phẫn uất" },
            
            ["hạnh phúc"] = new List<string> { "hạnh phúc", "행복", "행복하다", "sung sướng", "vui sướng", 
                "mãn nguyện", "trọn vẹn", "viên mãn", "tròn đầy", "đầm ấm", "êm ấm", 
                "ấm áp", "bình yên", "bình an", "thanh thản", "yên tĩnh", "thư thái", 
                "thoải mái", "dễ chịu", "thỏa mãn", "mãn nguyện", "như ý", "điều tâm", 
                "thích thú", "thích ý", "thích lắm" },
            
            ["mệt mỏi"] = new List<string> { "mệt", "mệt mỏi", "kiệt sức", "mỏi mệt", "uể oải", 
                "đuối sức", "hết sức", "gục ngã", "suy sụp", "sụp đổ", "không còn", 
                "cạn kiệt", "vắt kiệt", "tan nát", "chán nản", "chán", "ngán", 
                "chán ngấy", "chán đời", "chán chường", "nhàm chán", "buồn tẻ", 
                "tẻ nhạt", "vô vị", "nhạt nhẽo" },
            
            ["hồi hộp"] = new List<string> { "hồi hộp", "háo hức", "mong chờ", "mong đợi", 
                "hy vọng", "kỳ vọng", "chờ đợi", "chờ mong", "trông chờ", "trông mong", 
                "tò mò", "hiếu kỳ", "tìm hiểu", "khám phá", "phát hiện", "tìm ra", 
                "khao khát", "khát khao", "mong mỏi", "ao ước", "ước mơ", "khát vọng" },
            
            ["bình thường"] = new List<string> { "bình thường", "ổn", "ok", "tạm", "được", 
                "cũng được", "không sao", "bình yên", "ổn định", "êm đềm", "nhẹ nhàng", 
                "thoải mái", "dễ chịu", "ổn thỏa", "vậy thôi", "như thường" }
        };

        // Mapping từ cảm xúc sang thể loại sách phù hợp
        private readonly Dictionary<string, string[]> _camXucSangTheLoai = new Dictionary<string, string[]>
        {
            ["buồn"] = new[] { "Tâm lý học", "Triết học", "Tự lực", "Động viên", "Văn học", "Thơ", "Tâm linh" },
            ["vui"] = new[] { "Hài hước", "Phiêu lưu", "Du lịch", "Âm nhạc", "Nghệ thuật", "Thể thao" },
            ["lo lắng"] = new[] { "Tâm lý học", "Thiền", "Yoga", "Tự lực", "Kỹ năng sống", "Tâm linh" },
            ["tức giận"] = new[] { "Tâm lý học", "Thiền", "Triết học", "Tự lực", "Kỹ năng giao tiếp" },
            ["hạnh phúc"] = new[] { "Tản văn", "Thơ", "Du lịch", "Ẩm thực", "Gia đình", "Tình yêu" },
            ["mệt mỏi"] = new[] { "Tâm lý học", "Tự lực", "Sức khỏe", "Thể thao", "Thiền", "Nghỉ ngơi" },
            ["hồi hộp"] = new[] { "Trinh thám", "Phiêu lưu", "Khoa học viễn tưởng", "Kỳ ảo", "Giật gân" },
            ["bình thường"] = new[] { "Văn học", "Tiểu thuyết", "Tản văn", "Lịch sử", "Khoa học" }
        };

        public EmotionAnalysisService()
        {
            _aiService = new AIService();
        }

        /// <summary>
        /// Phân tích cảm xúc từ văn bản người dùng (kết hợp Rule-based và AI)
        /// </summary>
        public async Task<PhanTichCamXuc> PhanTichCamXucAsync(string vanBan)
        {
            if (string.IsNullOrWhiteSpace(vanBan))
            {
                return new PhanTichCamXuc
                {
                    CamXucChinh = "bình thường",
                    DoTinCay = 0.5,
                    LyDo = "Không có nội dung để phân tích",
                    TuKhoaCamXuc = new string[] { },
                    GoiYTheLoai = "Văn học"
                };
            }

            // Bước 1: Phân tích bằng Rule-based (nhanh, offline)
            var ketQuaRuleBased = PhanTichBangTuDien(vanBan);

            // Bước 2: Nếu độ tin cậy thấp, dùng AI để phân tích sâu hơn
            if (ketQuaRuleBased.DoTinCay < 0.6)
            {
                try
                {
                    var ketQuaAI = await PhanTichBangAI(vanBan);
                    // Ưu tiên kết quả AI nếu có
                    if (ketQuaAI != null && ketQuaAI.DoTinCay > ketQuaRuleBased.DoTinCay)
                    {
                        return ketQuaAI;
                    }
                }
                catch (Exception ex)
                {
                    // Nếu AI lỗi, dùng rule-based
                    Console.WriteLine($"AI phân tích lỗi: {ex.Message}. Dùng rule-based.");
                }
            }

            return ketQuaRuleBased;
        }

        /// <summary>
        /// Phân tích cảm xúc bằng từ điển và luật (offline, không cần API)
        /// </summary>
        private PhanTichCamXuc PhanTichBangTuDien(string vanBan)
        {
            vanBan = vanBan.ToLower().Trim();
            
            // Đếm số lần xuất hiện của mỗi cảm xúc
            var diemCamXuc = new Dictionary<string, int>();
            var tuKhoaTimThay = new List<string>();

            foreach (var camXuc in _tuDienCamXuc)
            {
                int diem = 0;
                foreach (var tuKhoa in camXuc.Value)
                {
                    if (vanBan.Contains(tuKhoa.ToLower()))
                    {
                        diem++;
                        tuKhoaTimThay.Add(tuKhoa);
                    }
                }
                if (diem > 0)
                {
                    diemCamXuc[camXuc.Key] = diem;
                }
            }

            // Nếu không tìm thấy cảm xúc nào
            if (!diemCamXuc.Any())
            {
                return new PhanTichCamXuc
                {
                    CamXucChinh = "bình thường",
                    DoTinCay = 0.4,
                    LyDo = "Không tìm thấy từ khóa cảm xúc rõ ràng",
                    TuKhoaCamXuc = new string[] { },
                    GoiYTheLoai = "Văn học"
                };
            }

            // Tìm cảm xúc có điểm cao nhất
            var camXucChinh = diemCamXuc.OrderByDescending(x => x.Value).First();
            double doTinCay = Math.Min(0.9, 0.5 + (camXucChinh.Value * 0.1)); // Max 0.9 cho rule-based

            return new PhanTichCamXuc
            {
                CamXucChinh = camXucChinh.Key,
                DoTinCay = doTinCay,
                LyDo = $"Phát hiện {camXucChinh.Value} từ khóa liên quan đến cảm xúc '{camXucChinh.Key}'",
                TuKhoaCamXuc = tuKhoaTimThay.Distinct().ToArray(),
                GoiYTheLoai = string.Join(", ", _camXucSangTheLoai.GetValueOrDefault(camXucChinh.Key, new[] { "Văn học" }))
            };
        }

        /// <summary>
        /// Phân tích cảm xúc bằng AI (chính xác hơn nhưng cần API)
        /// </summary>
        private async Task<PhanTichCamXuc> PhanTichBangAI(string vanBan)
        {
            try
            {
                string systemPrompt = @"Bạn là chuyên gia tâm lý học, nhiệm vụ của bạn là phân tích cảm xúc từ văn bản tiếng Việt.

Hãy phân tích cảm xúc chính từ văn bản và trả lời theo định dạng JSON:
{
    ""cam_xuc_chinh"": ""buồn|vui|lo lắng|tức giận|hạnh phúc|mệt mỏi|hồi hộp|bình thường"",
    ""do_tin_cay"": 0.0-1.0,
    ""ly_do"": ""Giải thích ngắn gọn"",
    ""tu_khoa"": [""từ1"", ""từ2""],
    ""the_loai_sach"": ""Thể loại sách phù hợp""
}

Ví dụ:
Input: ""Hôm nay tôi buồn quá, cảm thấy mệt mỏi và cô đơn""
Output: {""cam_xuc_chinh"": ""buồn"", ""do_tin_cay"": 0.9, ""ly_do"": ""Văn bản thể hiện rõ sự buồn bã, mệt mỏi và cô đơn"", ""tu_khoa"": [""buồn"", ""mệt mỏi"", ""cô đơn""], ""the_loai_sach"": ""Tâm lý học, Tự lực, Động viên""}";

                string userMessage = $"Phân tích cảm xúc: {vanBan}";
                string response = await _aiService.ChatAsync(userMessage, systemPrompt);

                // Parse JSON response
                var jsonMatch = Regex.Match(response, @"\{[^}]+\}");
                if (jsonMatch.Success)
                {
                    var json = System.Text.Json.JsonDocument.Parse(jsonMatch.Value);
                    var root = json.RootElement;

                    return new PhanTichCamXuc
                    {
                        CamXucChinh = root.GetProperty("cam_xuc_chinh").GetString() ?? "bình thường",
                        DoTinCay = root.GetProperty("do_tin_cay").GetDouble(),
                        LyDo = root.GetProperty("ly_do").GetString() ?? "",
                        TuKhoaCamXuc = root.GetProperty("tu_khoa").EnumerateArray()
                            .Select(x => x.GetString()).ToArray(),
                        GoiYTheLoai = root.GetProperty("the_loai_sach").GetString() ?? "Văn học"
                    };
                }

                return null; // Parse lỗi
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi phân tích AI: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách thể loại sách phù hợp với cảm xúc
        /// </summary>
        public string[] LayTheLoaiPhuHop(string camXuc)
        {
            return _camXucSangTheLoai.GetValueOrDefault(camXuc.ToLower(), new[] { "Văn học" });
        }

        /// <summary>
        /// Tạo câu động viên/khuyến khích dựa trên cảm xúc
        /// </summary>
        public string TaoCauDongVien(string camXuc)
        {
            var cauDongVien = new Dictionary<string, string[]>
            {
                ["buồn"] = new[] {
                    "Đừng lo, mọi chuyện rồi sẽ tốt thôi! 💙",
                    "Hãy để sách làm bạn đồng hành cùng bạn vượt qua giai đoạn này nhé!",
                    "Mỗi ngày khó khăn đều là bài học quý giá. Sách sẽ giúp bạn mạnh mẽ hơn!"
                },
                ["vui"] = new[] {
                    "Thật tuyệt! Hãy giữ vững tinh thần tích cực này! 🌟",
                    "Một cuốn sách hay sẽ làm ngày của bạn trở nên tuyệt vời hơn!",
                    "Niềm vui sẽ gấp đôi khi chia sẻ cùng những câu chuyện hay!"
                },
                ["lo lắng"] = new[] {
                    "Hãy thư giãn, mọi thứ sẽ ổn thôi! 🌿",
                    "Sách có thể giúp bạn tìm được sự bình yên trong tâm hồn.",
                    "Đọc sách là cách tuyệt vời để xua tan lo âu!"
                },
                ["tức giận"] = new[] {
                    "Hít thở sâu, mọi thứ sẽ qua! 🌬️",
                    "Đọc sách giúp bạn bình tĩnh và nhìn nhận vấn đề một cách sáng suốt hơn.",
                    "Đừng để cơn giận làm chủ! Sách sẽ giúp bạn tìm được sự thanh thản."
                },
                ["mệt mỏi"] = new[] {
                    "Nghỉ ngơi thôi, bạn đã làm tốt lắm rồi! 💪",
                    "Một cuốn sách nhẹ nhàng sẽ giúp bạn thư giãn và lấy lại năng lượng.",
                    "Đọc sách là cách nghỉ ngơi tích cực cho tâm hồn đấy!"
                },
                ["hồi hộp"] = new[] {
                    "Sự hồi hộp này sẽ được đền đáp xứng đáng! ✨",
                    "Hãy để những câu chuyện kỳ thú làm tăng thêm sự phấn khích!",
                    "Cuộc phiêu lưu tuyệt vời đang chờ bạn trong từng trang sách!"
                }
            };

            var random = new Random();
            var danhSach = cauDongVien.GetValueOrDefault(camXuc.ToLower(), 
                new[] { "Chúc bạn có những giây phút đọc sách thật tuyệt vời! 📚" });
            
            return danhSach[random.Next(danhSach.Length)];
        }
    }
}
