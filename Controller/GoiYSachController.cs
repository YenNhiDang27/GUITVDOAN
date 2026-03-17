using System;
using System.Collections.Generic;
using System.Linq;
using ThuVien.Models;
using ThuVien.Repository;
using ThuVien.Services;

namespace ThuVien.Controller
{
    /// <summary>
    /// Controller quản lý gợi ý sách thông minh
    /// </summary>
    public class GoiYSachController
    {
        private readonly RecommendationService _recommendationService;
        private readonly GoiYSachRepository _goiYRepo;
        private readonly SachRepository _sachRepo;

        public GoiYSachController()
        {
            _recommendationService = new RecommendationService();
            _goiYRepo = new GoiYSachRepository();
            _sachRepo = new SachRepository();
        }

        /// <summary>
        /// Tạo gợi ý sách mới cho bạn đọc
        /// </summary>
        public List<GoiYSach> TaoGoiYMoi(int maBanDoc, int soLuong = 10)
        {
            return _recommendationService.TaoGoiYChoDocGia(maBanDoc, soLuong);
        }

        /// <summary>
        /// Lấy danh sách gợi ý của bạn đọc
        /// </summary>
        public List<GoiYSachViewModel> LayDanhSachGoiY(int maBanDoc)
        {
            var goiYList = _goiYRepo.GetByMaBanDoc(maBanDoc);
            var result = new List<GoiYSachViewModel>();

            foreach (var goiY in goiYList)
            {
                var sach = _sachRepo.GetById(goiY.MaSach);
                if (sach != null)
                {
                    result.Add(new GoiYSachViewModel
                    {
                        MaGoiY = goiY.MaGoiY,
                        MaSach = sach.MaSach,
                        TenSach = sach.TenSach,
                        TacGia = sach.TacGia,
                        TenLoai = sach.TenLoai,
                        TenNXB = sach.TenNXB,
                        HinhAnh = sach.HinhAnh,
                        DiemGoiY = goiY.DiemGoiY,
                        LyDoGoiY = goiY.LyDoGoiY,
                        NgayGoiY = goiY.NgayGoiY,
                        DaXem = goiY.DaXem,
                        DaMuon = goiY.DaMuon,
                        SoLuong = sach.SoLuong,
                        TinhTrangSach = sach.TinhTrangSach
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Đánh dấu đã xem gợi ý
        /// </summary>
        public void DanhDauDaXem(int maGoiY)
        {
            _goiYRepo.DanhDauDaXem(maGoiY);
        }
    }

    /// <summary>
    /// ViewModel để hiển thị gợi ý sách
    /// </summary>
    public class GoiYSachViewModel
    {
        public int MaGoiY { get; set; }
        public int MaSach { get; set; }
        public string? TenSach { get; set; }
        public string? TacGia { get; set; }
        public string? TenLoai { get; set; }
        public string? TenNXB { get; set; }
        public string? HinhAnh { get; set; }
        public decimal DiemGoiY { get; set; }
        public string? LyDoGoiY { get; set; }
        public DateTime NgayGoiY { get; set; }
        public bool DaXem { get; set; }
        public bool DaMuon { get; set; }
        public int SoLuong { get; set; }
        public string? TinhTrangSach { get; set; }

        /// <summary>
        /// Phần trăm phù hợp
        /// </summary>
        public int PhanTramPhuHop => (int)(DiemGoiY * 100);
    }
}
