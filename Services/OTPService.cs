using System;
using System.Collections.Generic;
using System.Linq;

namespace ThuVien.Services
{
    public class OTPService
    {
        private static Dictionary<string, OTPData> _otpStorage = new Dictionary<string, OTPData>();
        private readonly Random _random = new Random();

        public class OTPData
        {
            public string OTP { get; set; } = "";
            public DateTime ExpiryTime { get; set; }
            public int AttemptCount { get; set; }
        }

        // Tạo mã OTP 6 chữ số
        public string GenerateOTP()
        {
            return _random.Next(100000, 999999).ToString();
        }

        // Lưu OTP với thời gian hết hạn (mặc định 5 phút)
        public void StoreOTP(string email, string otp, int expiryMinutes = 5)
        {
            _otpStorage[email.ToLower()] = new OTPData
            {
                OTP = otp,
                ExpiryTime = DateTime.Now.AddMinutes(expiryMinutes),
                AttemptCount = 0
            };
        }

        // Xác thực OTP
        public (bool isValid, string message) ValidateOTP(string email, string otp)
        {
            string emailLower = email.ToLower();

            if (!_otpStorage.ContainsKey(emailLower))
            {
                return (false, "Không tìm thấy mã OTP. Vui lòng gửi lại mã OTP.");
            }

            var otpData = _otpStorage[emailLower];

            // Kiểm tra số lần thử
            if (otpData.AttemptCount >= 3)
            {
                _otpStorage.Remove(emailLower);
                return (false, "Bạn đã nhập sai quá 3 lần. Vui lòng gửi lại mã OTP.");
            }

            // Kiểm tra thời gian hết hạn
            if (DateTime.Now > otpData.ExpiryTime)
            {
                _otpStorage.Remove(emailLower);
                return (false, "Mã OTP đã hết hạn. Vui lòng gửi lại mã OTP.");
            }

            // Kiểm tra mã OTP
            otpData.AttemptCount++;
            if (otpData.OTP != otp)
            {
                return (false, $"Mã OTP không chính xác. Còn {3 - otpData.AttemptCount} lần thử.");
            }

            // OTP hợp lệ, xóa khỏi bộ nhớ
            _otpStorage.Remove(emailLower);
            return (true, "Xác thực thành công!");
        }

        // Xóa OTP
        public void RemoveOTP(string email)
        {
            _otpStorage.Remove(email.ToLower());
        }

        // Kiểm tra email đã có OTP chưa hết hạn
        public bool HasValidOTP(string email)
        {
            string emailLower = email.ToLower();
            if (_otpStorage.ContainsKey(emailLower))
            {
                return DateTime.Now <= _otpStorage[emailLower].ExpiryTime;
            }
            return false;
        }
    }
}