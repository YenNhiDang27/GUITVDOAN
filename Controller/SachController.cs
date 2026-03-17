using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.Controller
{
    public class SachController
    {
        private  SachRepository _repository = new SachRepository();

        public SachController()
        {
         
        }

        public List<Sach> GetAll()
        {
            return _repository.GetAll();
        }

        public Sach? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Sach entity)
        {
            _repository.Add(entity);
        }

        public void Update(Sach entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
