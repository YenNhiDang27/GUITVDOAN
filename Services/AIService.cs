using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ThuVien.Services
{
    /// <summary>
    /// Service tích hợp AI để gợi ý sách và trả lời câu hỏi
    /// Hỗ trợ: Azure OpenAI, OpenAI API, hoặc Local AI
    /// </summary>
    public class AIService
    {
        // ==================== CẤU HÌNH AI ====================
        // Chọn loại AI: "OpenAI", "AzureOpenAI", hoặc "LocalAI"
        private const string AI_TYPE = "OpenAI";
        
        // Cấu hình OpenAI (https://platform.openai.com/api-keys)
        private const string OPENAI_API_KEY = "https://platform.openai.com/api-keys";  // ← Thay bằng API key của bạn
        private const string OPENAI_MODEL = "gpt-4o-mini";  // hoặc "gpt-3.5-turbo" (rẻ hơn)
        
        // Cấu hình Azure OpenAI (nếu dùng Azure)
        private const string AZURE_ENDPOINT = "https://your-resource.openai.azure.com/";
        private const string AZURE_API_KEY = "your-azure-key";
        private const string AZURE_DEPLOYMENT = "gpt-4";
        
        private static readonly HttpClient httpClient = new HttpClient();
        
        /// <summary>
        /// Kiểm tra cấu hình AI
        /// </summary>
        private static void KiemTraCauHinh()
        {
            if (AI_TYPE == "OpenAI" && OPENAI_API_KEY == "sk-your-api-key-here")
            {
                throw new InvalidOperationException(
                    "⚠️ CHƯA CẤU HÌNH AI!\n\n" +
                    "Vui lòng làm theo các bước sau:\n" +
                    "1. Đăng ký tài khoản tại: https://platform.openai.com/\n" +
                    "2. Tạo API Key tại: https://platform.openai.com/api-keys\n" +
                    "3. Mở file Services\\AIService.cs\n" +
                    "4. Thay đổi OPENAI_API_KEY thành key của bạn\n\n" +
                    "Lưu ý: OpenAI API có phí (khoảng $0.15-2/1M tokens)\n" +
                    "Khuyến nghị dùng gpt-4o-mini (rẻ nhất, $0.15/1M input)"
                );
            }
        }

        /// <summary>
        /// Gửi tin nhắn tới AI và nhận phản hồi
        /// </summary>
        public async Task<string> ChatAsync(string userMessage, string systemPrompt = null)
        {
            KiemTraCauHinh();

            if (AI_TYPE == "OpenAI")
            {
                return await ChatWithOpenAI(userMessage, systemPrompt);
            }
            else if (AI_TYPE == "AzureOpenAI")
            {
                return await ChatWithAzureOpenAI(userMessage, systemPrompt);
            }
            else
            {
                throw new NotImplementedException($"AI Type '{AI_TYPE}' chưa được hỗ trợ");
            }
        }

        /// <summary>
        /// Chat với OpenAI API
        /// </summary>
        private async Task<string> ChatWithOpenAI(string userMessage, string systemPrompt)
        {
            try
            {
                var messages = new List<object>();
                
                if (!string.IsNullOrEmpty(systemPrompt))
                {
                    messages.Add(new { role = "system", content = systemPrompt });
                }
                
                messages.Add(new { role = "user", content = userMessage });

                var requestBody = new
                {
                    model = OPENAI_MODEL,
                    messages = messages,
                    temperature = 0.7,
                    max_tokens = 1000
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {OPENAI_API_KEY}");

                var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"OpenAI API Error: {response.StatusCode}\n{responseContent}");
                }

                var jsonDoc = JsonDocument.Parse(responseContent);
                var aiMessage = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return aiMessage ?? "Xin lỗi, tôi không thể tạo phản hồi lúc này.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gọi OpenAI API: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Chat với Azure OpenAI
        /// </summary>
        private async Task<string> ChatWithAzureOpenAI(string userMessage, string systemPrompt)
        {
            try
            {
                var messages = new List<object>();
                
                if (!string.IsNullOrEmpty(systemPrompt))
                {
                    messages.Add(new { role = "system", content = systemPrompt });
                }
                
                messages.Add(new { role = "user", content = userMessage });

                var requestBody = new
                {
                    messages = messages,
                    temperature = 0.7,
                    max_tokens = 1000
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("api-key", AZURE_API_KEY);

                var url = $"{AZURE_ENDPOINT}openai/deployments/{AZURE_DEPLOYMENT}/chat/completions?api-version=2024-02-15-preview";
                var response = await httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Azure OpenAI Error: {response.StatusCode}\n{responseContent}");
                }

                var jsonDoc = JsonDocument.Parse(responseContent);
                var aiMessage = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return aiMessage ?? "Xin lỗi, tôi không thể tạo phản hồi lúc này.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gọi Azure OpenAI: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo system prompt cho chatbot thư viện
        /// </summary>
        public string TaoSystemPromptThuVien(List<string> danhSachSach = null, string thoiQuanDoc = null)
        {
            var prompt = @"Bạn là trợ lý AI thông minh của thư viện, chuyên về sách và đọc sách.

NHIỆM VỤ:
- Gợi ý sách phù hợp dựa trên sở thích và thói quen đọc
- Tìm kiếm sách theo yêu cầu của người dùng
- Dự đoán xu hướng sách sẽ được ưa chuộng
- Trả lời thân thiện, nhiệt tình bằng tiếng Việt

QUY TẮC:
- Luôn gợi ý cụ thể tên sách, tác giả
- Giải thích lý do gợi ý
- Nếu không chắc chắn, hãy hỏi thêm thông tin
- Giữ câu trả lời ngắn gọn (dưới 300 từ)";

            if (danhSachSach != null && danhSachSach.Any())
            {
                prompt += "\n\nDÁNH SÁCH SÁCH HIỆN CÓ:\n" + string.Join("\n", danhSachSach.Take(50));
            }

            if (!string.IsNullOrEmpty(thoiQuanDoc))
            {
                prompt += "\n\nTHÓI QUEN ĐỌC CỦA NGƯỜI DÙNG:\n" + thoiQuanDoc;
            }

            return prompt;
        }

        /// <summary>
        /// Phân tích câu hỏi của người dùng
        /// </summary>
        public async Task<string> PhanTichYeuCauAsync(string userMessage)
        {
            var prompt = @"Phân tích câu hỏi sau và trả về 1 trong các loại:
- tim_kiem: Nếu người dùng muốn tìm sách cụ thể
- goi_y: Nếu người dùng muốn được gợi ý sách
- du_doan: Nếu hỏi về xu hướng, sách nổi bật
- khac: Các câu hỏi khác

Chỉ trả về 1 từ khóa, không giải thích.";

            try
            {
                var result = await ChatAsync(userMessage, prompt);
                return result.Trim().ToLower();
            }
            catch
            {
                return "khac";
            }
        }
    }
}
