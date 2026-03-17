using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Repository;
using ThuVien.Models;

namespace ThuVien.Controller
{
    public class PhieuMuonController
    {
        private  PhieuMuonRepository _repository;

        public PhieuMuonController(PhieuMuonRepository phieuMuonRepository)
        {
          _repository = phieuMuonRepository;
        }

        public List<PhieuMuon> GetAll()
        {
            return _repository.GetAll();
        }

        public PhieuMuon GetById(int id)
        {
            return _repository.GetById(id);
        }

      

        public void Update(PhieuMuon entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
        public bool KiemTraPhieuMuonChuaTra(int maNguoiDoc) {

            return _repository.KiemTraPhieuMuonChuaTra(maNguoiDoc);
            
        }
    }
}
