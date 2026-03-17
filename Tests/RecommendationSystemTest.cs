using System;
using System.Linq;
using ThuVien.Services;
using ThuVien.Controller;
using ThuVien.Repository;

namespace ThuVien.Tests
{
    /// <summary>
    /// Class để test hệ thống gợi ý sách
    /// </summary>
    public class RecommendationSystemTest
    {
        private RecommendationService _recommendationService;
        private GoiYSachController _controller;
        private BanDocRepository _banDocRepo;

        public RecommendationSystemTest()
        {
            _recommendationService = new RecommendationService();
            _controller = new GoiYSachController();
            _banDocRepo = new BanDocRepository();
        }

        /// <summary>
        /// Test tạo gợi ý cho một bạn đọc
        /// </summary>
        public void TestTaoGoiYChoDocGia()
        {
            Console.WriteLine("=== TEST TẠO GỢI Ý CHO ĐỘC GIẢ ===\n");

            try
            {
                // Lấy bạn đọc đầu tiên để test
                var banDocList = _banDocRepo.GetAll();
                if (!banDocList.Any())
                {
                    Console.WriteLine("❌ Không có bạn đọc nào trong hệ thống!");
                    return;
                }

                var banDoc = banDocList.First();
                Console.WriteLine($"📖 Bạn đọc: {banDoc.HoTen} (Mã: {banDoc.MaBanDoc})");
                Console.WriteLine($"📧 Email: {banDoc.Email}\n");

                // Tạo gợi ý
                Console.WriteLine("🔄 Đang tạo gợi ý...");
                var goiY = _recommendationService.TaoGoiYChoDocGia(banDoc.MaBanDoc, 10);

                Console.WriteLine($"✅ Đã tạo {goiY.Count} gợi ý\n");

                // Hiển thị chi tiết
                Console.WriteLine("📊 DANH SÁCH GỢI Ý:\n");
                int stt = 1;
                foreach (var item in goiY.OrderByDescending(g => g.DiemGoiY))
                {
                    Console.WriteLine($"{stt}. Mã sách: {item.MaSach}");
                    Console.WriteLine($"   Điểm: {item.DiemGoiY:F4} ({(int)(item.DiemGoiY * 100)}%)");
                    Console.WriteLine($"   Lý do: {item.LyDoGoiY}");
                    Console.WriteLine($"   Ngày: {item.NgayGoiY:dd/MM/yyyy HH:mm}");
                    Console.WriteLine();
                    stt++;
                }

                Console.WriteLine("✅ TEST THÀNH CÔNG!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ LỖI: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Test lấy danh sách gợi ý từ database
        /// </summary>
        public void TestLayDanhSachGoiY()
        {
            Console.WriteLine("\n=== TEST LẤY DANH SÁCH GỢI Ý ===\n");

            try
            {
                // Lấy bạn đọc đầu tiên
                var banDocList = _banDocRepo.GetAll();
                if (!banDocList.Any())
                {
                    Console.WriteLine("❌ Không có bạn đọc nào!");
                    return;
                }

                var banDoc = banDocList.First();
                Console.WriteLine($"📖 Bạn đọc: {banDoc.HoTen}\n");

                // Lấy gợi ý
                var goiYList = _controller.LayDanhSachGoiY(banDoc.MaBanDoc);

                if (!goiYList.Any())
                {
                    Console.WriteLine("⚠️ Chưa có gợi ý nào. Tạo gợi ý mới...");
                    _controller.TaoGoiYMoi(banDoc.MaBanDoc, 10);
                    goiYList = _controller.LayDanhSachGoiY(banDoc.MaBanDoc);
                }

                Console.WriteLine($"📊 Tìm thấy {goiYList.Count} gợi ý:\n");

                foreach (var item in goiYList.Take(5))
                {
                    Console.WriteLine($"📚 {item.TenSach}");
                    Console.WriteLine($"   ✍️ Tác giả: {item.TacGia}");
                    Console.WriteLine($"   📂 Thể loại: {item.TenLoai}");
                    Console.WriteLine($"   ⭐ Độ phù hợp: {item.PhanTramPhuHop}%");
                    Console.WriteLine($"   💡 Lý do: {item.LyDoGoiY}");
                    Console.WriteLine($"   📦 Tình trạng: {item.TinhTrangSach}");
                    Console.WriteLine();
                }

                Console.WriteLine("✅ TEST THÀNH CÔNG!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ LỖI: {ex.Message}");
            }
        }

        /// <summary>
        /// Test hiệu suất hệ thống
        /// </summary>
        public void TestHieuSuat()
        {
            Console.WriteLine("\n=== TEST HIỆU SUẤT HỆ THỐNG ===\n");

            try
            {
                var banDocList = _banDocRepo.GetAll().Take(5).ToList();
                Console.WriteLine($"📊 Test với {banDocList.Count} bạn đọc\n");

                var startTime = DateTime.Now;

                foreach (var banDoc in banDocList)
                {
                    Console.Write($"⏳ Xử lý {banDoc.HoTen}... ");
                    var start = DateTime.Now;

                    var goiY = _recommendationService.TaoGoiYChoDocGia(banDoc.MaBanDoc, 10);

                    var duration = (DateTime.Now - start).TotalSeconds;
                    Console.WriteLine($"✅ {goiY.Count} gợi ý ({duration:F2}s)");
                }

                var totalTime = (DateTime.Now - startTime).TotalSeconds;
                Console.WriteLine($"\n⏱️ Tổng thời gian: {totalTime:F2} giây");
                Console.WriteLine($"⚡ Trung bình: {totalTime / banDocList.Count:F2} giây/độc giả");

                Console.WriteLine("\n✅ TEST THÀNH CÔNG!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ LỖI: {ex.Message}");
            }
        }

        /// <summary>
        /// Test tất cả các chức năng
        /// </summary>
        public void RunAllTests()
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║  HỆ THỐNG TEST RECOMMENDATION SYSTEM         ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝\n");

            TestTaoGoiYChoDocGia();
            TestLayDanhSachGoiY();
            TestHieuSuat();

            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║  HOÀN TẤT TẤT CẢ TESTS                       ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
        }

        /// <summary>
        /// Test thống kê gợi ý
        /// </summary>
        public void TestThongKe()
        {
            Console.WriteLine("\n=== TEST THỐNG KÊ GỢI Ý ===\n");

            try
            {
                var goiYRepo = new GoiYSachRepository();
                var banDocList = _banDocRepo.GetAll();

                int tongSoGoiY = 0;
                int soBanDocCoGoiY = 0;

                foreach (var banDoc in banDocList)
                {
                    var goiY = goiYRepo.GetByMaBanDoc(banDoc.MaBanDoc);
                    if (goiY.Any())
                    {
                        soBanDocCoGoiY++;
                        tongSoGoiY += goiY.Count;
                    }
                }

                Console.WriteLine($"📊 THỐNG KÊ:");
                Console.WriteLine($"   Tổng số bạn đọc: {banDocList.Count}");
                Console.WriteLine($"   Số bạn đọc có gợi ý: {soBanDocCoGoiY}");
                Console.WriteLine($"   Tổng số gợi ý: {tongSoGoiY}");
                Console.WriteLine($"   Trung bình: {(tongSoGoiY > 0 ? (double)tongSoGoiY / soBanDocCoGoiY : 0):F2} gợi ý/độc giả");

                Console.WriteLine("\n✅ TEST THÀNH CÔNG!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ LỖI: {ex.Message}");
            }
        }
    }
}
