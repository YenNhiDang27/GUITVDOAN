using System;
using System.Collections.Generic;
using System.Linq;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.Services
{
    /// <summary>
    /// Service Machine Learning để gợi ý sách thông minh
    /// Sử dụng thuật toán Collaborative Filtering và Content-Based Filtering
    /// </summary>
    public class RecommendationService
    {
        private readonly PhieuMuonRepository _phieuMuonRepo;
        private readonly ChiTietPhieuMuonRepository _chiTietRepo;
        private readonly SachRepository _sachRepo;
        private readonly BanDocRepository _banDocRepo;
        private readonly GoiYSachRepository _goiYRepo;

        public RecommendationService()
        {
            _phieuMuonRepo = new PhieuMuonRepository();
            _chiTietRepo = new ChiTietPhieuMuonRepository();
            _sachRepo = new SachRepository();
            _banDocRepo = new BanDocRepository();
            _goiYRepo = new GoiYSachRepository();
        }

        /// <summary>
        /// Tạo gợi ý sách cho bạn đọc dựa trên lịch sử mượn
        /// </summary>
        public List<GoiYSach> TaoGoiYChoDocGia(int maBanDoc, int soLuongGoiY = 10)
        {
            var goiYList = new List<GoiYSach>();

            try
            {
                // 1. Lấy lịch sử mượn của độc giả
                var lichSuMuon = LayLichSuMuonSach(maBanDoc);
                
                if (!lichSuMuon.Any())
                {
                    // Nếu chưa có lịch sử, gợi ý sách phổ biến
                    return GoiYSachPhoiBien(maBanDoc, soLuongGoiY);
                }

                // 2. Phân tích sở thích độc giả
                var soThich = PhanTichSoThichDocGia(lichSuMuon);

                // 3. Content-Based Filtering: Tìm sách tương tự
                var goiYTheoNoiDung = GoiYTheoNoiDung(maBanDoc, soThich, lichSuMuon);
                goiYList.AddRange(goiYTheoNoiDung);

                // 4. Collaborative Filtering: Tìm sách từ độc giả tương tự
                var goiYCongTac = GoiYCongTac(maBanDoc, lichSuMuon);
                goiYList.AddRange(goiYCongTac);

                // 5. Kết hợp và xếp hạng các gợi ý
                var ketQuaCuoi = KetHopVaXepHang(goiYList, soLuongGoiY);

                // 6. Lưu vào database
                LuuGoiYVaoDatabase(ketQuaCuoi);

                return ketQuaCuoi;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo gợi ý: {ex.Message}");
                return new List<GoiYSach>();
            }
        }

        /// <summary>
        /// Lấy lịch sử mượn sách của độc giả
        /// </summary>
        private List<LichSuMuonDTO> LayLichSuMuonSach(int maBanDoc)
        {
            var result = new List<LichSuMuonDTO>();
            var phieuMuonList = _phieuMuonRepo.GetAll()
                .Where(pm => pm.MaBanDoc == maBanDoc)
                .ToList();

            foreach (var pm in phieuMuonList)
            {
                var chiTietList = _chiTietRepo.GetChiTietPhieuMuon(pm.MaPhieuMuon);
                foreach (var ct in chiTietList)
                {
                    var sach = _sachRepo.GetById(ct.MaSach);
                    if (sach != null)
                    {
                        // Xử lý trường hợp SoLuong NULL hoặc 0
                        int soLuong = ct.SoLuong > 0 ? ct.SoLuong : 1;

                        result.Add(new LichSuMuonDTO
                        {
                            MaSach = ct.MaSach,
                            TenSach = sach.TenSach,
                            TacGia = sach.TacGia,
                            MaLoai = sach.MaLoai,
                            TenLoai = sach.TenLoai,
                            NgayMuon = pm.NgayMuon,
                            SoLuongMuon = soLuong  // Dùng giá trị đã xử lý
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Phân tích sở thích độc giả từ lịch sử
        /// </summary>
        private SoThichDocGiaDTO PhanTichSoThichDocGia(List<LichSuMuonDTO> lichSu)
        {
            var soThich = new SoThichDocGiaDTO
            {
                TheLoaiYeuThich = new Dictionary<int, int>(),
                TacGiaYeuThich = new Dictionary<string, int>()
            };

            foreach (var item in lichSu)
            {
                // Đếm thể loại
                if (!soThich.TheLoaiYeuThich.ContainsKey(item.MaLoai))
                    soThich.TheLoaiYeuThich[item.MaLoai] = 0;
                soThich.TheLoaiYeuThich[item.MaLoai] += item.SoLuongMuon;

                // Đếm tác giả
                if (!string.IsNullOrEmpty(item.TacGia))
                {
                    if (!soThich.TacGiaYeuThich.ContainsKey(item.TacGia))
                        soThich.TacGiaYeuThich[item.TacGia] = 0;
                    soThich.TacGiaYeuThich[item.TacGia] += item.SoLuongMuon;
                }
            }

            return soThich;
        }

        /// <summary>
        /// Content-Based Filtering: Gợi ý dựa trên nội dung sách
        /// </summary>
        private List<GoiYSach> GoiYTheoNoiDung(int maBanDoc, SoThichDocGiaDTO soThich, List<LichSuMuonDTO> lichSu)
        {
            var goiYList = new List<GoiYSach>();
            var sachDaMuon = lichSu.Select(ls => ls.MaSach).ToHashSet();
            var tatCaSach = _sachRepo.GetAll();

            foreach (var sach in tatCaSach)
            {
                // Bỏ qua sách đã mượn
                if (sachDaMuon.Contains(sach.MaSach))
                    continue;

                // Bỏ qua sách hết
                if (sach.TinhTrangSach == "Đã hết" || sach.SoLuong <= 0)
                    continue;

                decimal diem = 0;
                string lyDo = "";

                // Điểm cho thể loại
                if (soThich.TheLoaiYeuThich.ContainsKey(sach.MaLoai))
                {
                    int soLanMuonTheLoai = soThich.TheLoaiYeuThich[sach.MaLoai];
                    int tongSoLanMuon = soThich.TheLoaiYeuThich.Values.Sum();
                    decimal tiLe = (decimal)soLanMuonTheLoai / tongSoLanMuon;
                    diem += tiLe * 0.6m; // Trọng số 60% cho thể loại
                    
                    if (tiLe > 0.3m)
                        lyDo += $"Bạn thích thể loại '{sach.TenLoai}'. ";
                }

                // Điểm cho tác giả
                if (!string.IsNullOrEmpty(sach.TacGia) && soThich.TacGiaYeuThich.ContainsKey(sach.TacGia))
                {
                    int soLanMuonTacGia = soThich.TacGiaYeuThich[sach.TacGia];
                    int tongSoLanMuon = soThich.TacGiaYeuThich.Values.Sum();
                    decimal tiLe = (decimal)soLanMuonTacGia / tongSoLanMuon;
                    diem += tiLe * 0.4m; // Trọng số 40% cho tác giả
                    
                    if (tiLe > 0.2m)
                        lyDo += $"Bạn đã mượn sách của tác giả '{sach.TacGia}'. ";
                }

                // Chỉ thêm nếu điểm > 0
                if (diem > 0)
                {
                    goiYList.Add(new GoiYSach
                    {
                        MaBanDoc = maBanDoc,
                        MaSach = sach.MaSach,
                        DiemGoiY = Math.Min(diem, 1.0m), // Giới hạn 0-1
                        LyDoGoiY = lyDo.Trim(),
                        NgayGoiY = DateTime.Now,
                        DaXem = false,
                        DaMuon = false
                    });
                }
            }

            return goiYList;
        }

        /// <summary>
        /// Collaborative Filtering: Gợi ý dựa trên độc giả tương tự
        /// </summary>
        private List<GoiYSach> GoiYCongTac(int maBanDoc, List<LichSuMuonDTO> lichSuNguoiDung)
        {
            var goiYList = new List<GoiYSach>();
            var sachDaMuon = lichSuNguoiDung.Select(ls => ls.MaSach).ToHashSet();

            // Lấy tất cả độc giả khác
            var tatCaBanDoc = _banDocRepo.GetAll();
            var docGiaTuongTu = new Dictionary<int, double>();

            foreach (var banDoc in tatCaBanDoc)
            {
                if (banDoc.MaBanDoc == maBanDoc)
                    continue;

                // Lấy lịch sử của độc giả này
                var lichSuBanDocKhac = LayLichSuMuonSach(banDoc.MaBanDoc);
                var sachBanDocKhacMuon = lichSuBanDocKhac.Select(ls => ls.MaSach).ToHashSet();

                // Tính độ tương đồng Jaccard
                var giaoDiem = sachDaMuon.Intersect(sachBanDocKhacMuon).Count();
                var hopNhat = sachDaMuon.Union(sachBanDocKhacMuon).Count();

                if (hopNhat > 0)
                {
                    double doTuongDong = (double)giaoDiem / hopNhat;
                    if (doTuongDong > 0.2) // Ngưỡng tương đồng tối thiểu 20%
                    {
                        docGiaTuongTu[banDoc.MaBanDoc] = doTuongDong;
                    }
                }
            }

            // Lấy sách từ độc giả tương tự
            var demSach = new Dictionary<int, DoTuongTuSachDTO>();

            foreach (var kvp in docGiaTuongTu.OrderByDescending(x => x.Value).Take(5)) // Top 5 độc giả tương tự
            {
                var lichSuDocGiaTuongTu = LayLichSuMuonSach(kvp.Key);
                
                foreach (var item in lichSuDocGiaTuongTu)
                {
                    // Bỏ qua sách đã mượn
                    if (sachDaMuon.Contains(item.MaSach))
                        continue;

                    var sach = _sachRepo.GetById(item.MaSach);
                    if (sach == null || sach.TinhTrangSach == "Đã hết" || sach.SoLuong <= 0)
                        continue;

                    if (!demSach.ContainsKey(item.MaSach))
                    {
                        demSach[item.MaSach] = new DoTuongTuSachDTO
                        {
                            MaSach = item.MaSach,
                            TongDiem = 0,
                            SoDocGiaGioiThieu = 0
                        };
                    }

                    demSach[item.MaSach].TongDiem += kvp.Value; // Cộng dồn độ tương đồng
                    demSach[item.MaSach].SoDocGiaGioiThieu++;
                }
            }

            // Tạo gợi ý từ kết quả
            foreach (var item in demSach.Values.OrderByDescending(x => x.TongDiem))
            {
                decimal diemGoiY = (decimal)(item.TongDiem / item.SoDocGiaGioiThieu); // Trung bình
                string lyDo = $"Được {item.SoDocGiaGioiThieu} độc giả có sở thích tương tự gợi ý.";

                goiYList.Add(new GoiYSach
                {
                    MaBanDoc = maBanDoc,
                    MaSach = item.MaSach,
                    DiemGoiY = Math.Min(diemGoiY, 1.0m),
                    LyDoGoiY = lyDo,
                    NgayGoiY = DateTime.Now,
                    DaXem = false,
                    DaMuon = false
                });
            }

            return goiYList;
        }

        /// <summary>
        /// Gợi ý sách phổ biến khi chưa có lịch sử
        /// </summary>
        private List<GoiYSach> GoiYSachPhoiBien(int maBanDoc, int soLuong)
        {
            var goiYList = new List<GoiYSach>();
            var thongKeMuon = new Dictionary<int, int>();

            // Đếm số lần mượn của mỗi sách
            var tatCaPhieuMuon = _phieuMuonRepo.GetAll();
            foreach (var pm in tatCaPhieuMuon)
            {
                var chiTiet = _chiTietRepo.GetChiTietPhieuMuon(pm.MaPhieuMuon);
                foreach (var ct in chiTiet)
                {
                    if (!thongKeMuon.ContainsKey(ct.MaSach))
                        thongKeMuon[ct.MaSach] = 0;

                    // Xử lý trường hợp SoLuong NULL hoặc 0
                    int soLuongMuon = ct.SoLuong > 0 ? ct.SoLuong : 1;
                    thongKeMuon[ct.MaSach] += soLuongMuon;
                }
            }

            // Lấy top sách được mượn nhiều nhất
            var topSach = thongKeMuon.OrderByDescending(x => x.Value).Take(soLuong);

            foreach (var item in topSach)
            {
                var sach = _sachRepo.GetById(item.Key);
                if (sach != null && sach.TinhTrangSach != "Đã hết" && sach.SoLuong > 0)
                {
                    goiYList.Add(new GoiYSach
                    {
                        MaBanDoc = maBanDoc,
                        MaSach = item.Key,
                        DiemGoiY = 0.7m, // Điểm cơ bản cho sách phổ biến
                        LyDoGoiY = $"Sách phổ biến được mượn {item.Value} lần.",
                        NgayGoiY = DateTime.Now,
                        DaXem = false,
                        DaMuon = false
                    });
                }
            }

            return goiYList;
        }

        /// <summary>
        /// Kết hợp và xếp hạng các gợi ý
        /// </summary>
        private List<GoiYSach> KetHopVaXepHang(List<GoiYSach> goiYList, int soLuong)
        {
            // Nhóm theo MaSach và tính điểm trung bình
            var nhomTheoSach = goiYList
                .GroupBy(g => g.MaSach)
                .Select(group => new GoiYSach
                {
                    MaBanDoc = group.First().MaBanDoc,
                    MaSach = group.Key,
                    DiemGoiY = group.Average(g => g.DiemGoiY),
                    LyDoGoiY = string.Join(" ", group.Select(g => g.LyDoGoiY).Distinct()),
                    NgayGoiY = DateTime.Now,
                    DaXem = false,
                    DaMuon = false
                })
                .OrderByDescending(g => g.DiemGoiY)
                .Take(soLuong)
                .ToList();

            return nhomTheoSach;
        }

        /// <summary>
        /// Lưu gợi ý vào database
        /// </summary>
        private void LuuGoiYVaoDatabase(List<GoiYSach> goiYList)
        {
            foreach (var goiY in goiYList)
            {
                try
                {
                    _goiYRepo.Insert(goiY);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lưu gợi ý sách {goiY.MaSach}: {ex.Message}");
                }
            }
        }

        #region DTO Classes

        private class LichSuMuonDTO
        {
            public int MaSach { get; set; }
            public string? TenSach { get; set; }
            public string? TacGia { get; set; }
            public int MaLoai { get; set; }
            public string? TenLoai { get; set; }
            public DateTime NgayMuon { get; set; }
            public int SoLuongMuon { get; set; }
        }

        private class SoThichDocGiaDTO
        {
            public Dictionary<int, int> TheLoaiYeuThich { get; set; } = new();
            public Dictionary<string, int> TacGiaYeuThich { get; set; } = new();
        }

        private class DoTuongTuSachDTO
        {
            public int MaSach { get; set; }
            public double TongDiem { get; set; }
            public int SoDocGiaGioiThieu { get; set; }
        }

        #endregion
    }
}
