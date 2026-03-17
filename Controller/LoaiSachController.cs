using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.Controller
{
    public class LoaiSachController
    {
        private  LoaiSachRepository _repository = new LoaiSachRepository();

        public LoaiSachController()
        {
          
        }

        public List<LoaiSach> GetAll()
        {
            return _repository.GetAll();
        }

        public LoaiSach? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(LoaiSach entity)
        {
            _repository.Add(entity);
        }

        public void Update(LoaiSach entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
