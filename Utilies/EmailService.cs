using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ThuVien.Utilies
{
    public class EmailService
    {
        // Cấu hình email mặc định - BẠN CẦN THAY ĐỔI EMAIL VÀ MẬT KHẨU CỦA MÌNH
        private const string EMAIL_GUI = "ngoctienvx27@gmail.com";  // ← Email Gmail của bạn
        private const string MAT_KHAU_UNG_DUNG = "jdloejbtaocuhzmw"; // ← App Password (16 ký tự, có thể có khoảng trắng)
        private const string SMTP_SERVER = "smtp.gmail.com";
        private const int SMTP_PORT = 587;

        /// <summary>
        /// Loại bỏ khoảng trắng trong App Password (Gmail cho password dạng xxxx xxxx xxxx xxxx)
        /// </summary>
        private static string LayMatKhauSachSe()
        {
            return MAT_KHAU_UNG_DUNG.Replace(" ", "").Replace("-", "").Replace("\r", "").Replace("\n", "").Trim();
        }

        /// <summary>
        /// Kiểm tra định dạng email có hợp lệ không
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gửi email bất đồng bộ (không làm đơ giao diện)
        /// </summary>
        public async Task<bool> GuiEmailAsync(string noiDung, string emailNhan, string tieuDe = "Thư viện thông báo")
        {
            // Kiểm tra email hợp lệ
            if (!IsValidEmail(emailNhan))
            {
                throw new ArgumentException("Email người nhận không hợp lệ!");
            }

            try
            {
                string matKhau = LayMatKhauSachSe();
                using (SmtpClient smtpClient = new SmtpClient(SMTP_SERVER))
                {
                    smtpClient.Port = SMTP_PORT;
                    smtpClient.Credentials = new NetworkCredential(EMAIL_GUI, matKhau);
                    smtpClient.EnableSsl = true;
                    smtpClient.Timeout = 30000; // 30 giây

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(EMAIL_GUI, "Thư Viện");
                        mail.To.Add(emailNhan);
                        mail.Subject = tieuDe;
                        mail.Body = noiDung;
                        mail.IsBodyHtml = false;
                        mail.BodyEncoding = Encoding.UTF8;
                        mail.SubjectEncoding = Encoding.UTF8;

                        await smtpClient.SendMailAsync(mail);
                    }
                }
                return true;
            }
            catch (SmtpException smtpEx)
            {
                string loiChiTiet = smtpEx.StatusCode switch
                {
                    System.Net.Mail.SmtpStatusCode.MailboxUnavailable => "Email người nhận không tồn tại hoặc không khả dụng.",
                    System.Net.Mail.SmtpStatusCode.GeneralFailure => "Lỗi chung. Kiểm tra kết nối mạng và cấu hình SMTP.",
                    _ => $"Lỗi SMTP ({smtpEx.StatusCode}): {smtpEx.Message}"
                };
                throw new Exception($"❌ {loiChiTiet}\n\nChi tiết kỹ thuật: {smtpEx.Message}");
            }
            catch (InvalidOperationException configEx)
            {
                // Lỗi cấu hình - ném lại để hiển thị nguyên văn
                throw;
            }
            catch (Exception ex)
            {
                string goiY = "";
                if (ex.Message.Contains("authentication") || ex.Message.Contains("Authentication"))
                {
                    goiY = "\n\n💡 Gợi ý: Kiểm tra lại email và App Password trong file EmailService.cs";
                }
                else if (ex.Message.Contains("timeout") || ex.Message.Contains("Timeout"))
                {
                    goiY = "\n\n💡 Gợi ý: Kiểm tra kết nối internet hoặc tắt tạm firewall/antivirus";
                }
                else if (ex.Message.Contains("secure connection"))
                {
                    goiY = "\n\n💡 Gợi ý: Đảm bảo EnableSsl = true trong code";
                }
                throw new Exception($"❌ Lỗi gửi email: {ex.Message}{goiY}");
            }
        }

        /// <summary>
        /// Gửi email đồng bộ (tương thích với code cũ)
        /// </summary>
        public void GuiEmail(string noidung, string emailGui, string emailNhan)
        {
            try
            {
                if (!IsValidEmail(emailNhan))
                {
                    throw new ArgumentException("Email người nhận không hợp lệ!");
                }

                string matKhau = LayMatKhauSachSe();
                using (SmtpClient smtpClient = new SmtpClient(SMTP_SERVER))
                {
                    smtpClient.Port = SMTP_PORT;
                    smtpClient.Credentials = new NetworkCredential(EMAIL_GUI, matKhau);
                    smtpClient.EnableSsl = true;
                    smtpClient.Timeout = 30000;

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(EMAIL_GUI, "Thư Viện");
                        mail.To.Add(emailNhan);
                        mail.Subject = "Thư viện thông báo";
                        mail.Body = noidung;
                        mail.IsBodyHtml = false;
                        mail.BodyEncoding = Encoding.UTF8;
                        mail.SubjectEncoding = Encoding.UTF8;

                        smtpClient.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi gửi email: {ex.Message}");
            }
        }

        /// <summary>
        /// Hàm tĩnh Send dùng cho chức năng quên mật khẩu
        /// </summary>
        public static void Send(string to, string subject, string body)
        {
            if (!IsValidEmail(to))
            {
                throw new ArgumentException("Email người nhận không hợp lệ!");
            }

            try
            {
                string matKhau = LayMatKhauSachSe();
                using (SmtpClient smtpClient = new SmtpClient(SMTP_SERVER))
                {
                    smtpClient.Port = SMTP_PORT;
                    smtpClient.Credentials = new NetworkCredential(EMAIL_GUI, matKhau);
                    smtpClient.EnableSsl = true;
                    smtpClient.Timeout = 30000;

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(EMAIL_GUI, "Thư Viện");
                        mail.To.Add(to);
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = false;
                        mail.BodyEncoding = Encoding.UTF8;
                        mail.SubjectEncoding = Encoding.UTF8;

                        smtpClient.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi gửi email: {ex.Message}");
            }
        }
    }
}