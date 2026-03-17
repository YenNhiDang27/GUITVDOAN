using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.Controller
{
    public class NhaXuatBanController
    {
        private  NhaXuatBanRepository _repository = new NhaXuatBanRepository();

        public NhaXuatBanController()
        {
            
        }

        public List<NhaXuatBan> GetAll()
        {
            return _repository.GetAll();
        }

        public NhaXuatBan GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(NhaXuatBan entity)
        {
            _repository.Add(entity);
        }

        public void Update(NhaXuatBan entity)
        {
            _repository.Update(entity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
