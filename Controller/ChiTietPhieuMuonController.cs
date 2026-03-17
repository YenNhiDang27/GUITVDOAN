using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.Controller
{
    public class ChiTietPhieuMuonController
    {
        private ChiTietPhieuMuonRepository _repository;

        public ChiTietPhieuMuonController(ChiTietPhieuMuonRepository chiTietPhieuMuonRepository)
        {
            _repository = chiTietPhieuMuonRepository;
        }

        public List<ChiTietPhieuMuon> GetAll()
        {
            return _repository.LayDanhSach();
        }

        public ChiTietPhieuMuon? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(ChiTietPhieuMuon entity)
        {
            _repository.ThemChiTiet(entity);
        }

        public void Update(ChiTietPhieuMuon entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.XoaChiTiet(id);
        }
    }

}
