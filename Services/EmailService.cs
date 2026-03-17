using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ThuVien.Services
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _senderName;

        public EmailService()
        {
            // Cấu hình Gmail SMTP
            _smtpHost = "smtp.gmail.com";
            _smtpPort = 587;
            _senderEmail = "your-email@gmail.com"; // Thay bằng email của bạn
            _senderPassword = "your-app-password"; // Mật khẩu ứng dụng Gmail
            _senderName = "Thư Viện";
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_senderEmail, _senderName);
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;

                    using (SmtpClient smtp = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendMultipleEmailsAsync(List<string> toEmails, string subject, string body)
        {
            int successCount = 0;
            foreach (string email in toEmails)
            {
                bool result = await SendEmailAsync(email, subject, body);
                if (result) successCount++;
            }
            return successCount > 0;
        }
    }
}