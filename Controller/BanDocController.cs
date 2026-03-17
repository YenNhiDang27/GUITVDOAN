using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThuVien.Models;
using ThuVien.Repository;

namespace ThuVien.Controller
{
    public class BanDocController
    {
        private  BanDocRepository _repository;

        public BanDocController(BanDocRepository _repository)
        {
            this._repository = _repository;
        }

        public List<BanDoc> GetAll()
        {
            return _repository.GetAll();
        }

        public BanDoc GetById(int maBanDoc)
        {
            return _repository.GetBanDocById(maBanDoc);
        }

        public void Add(BanDoc banDoc)
        {
            _repository.Add(banDoc);
        }

        public void Update(BanDoc banDoc)
        {
            _repository.Update(banDoc);
        }

        public void Delete(int maBanDoc)
        {
            _repository.Delete(maBanDoc);
        }
    }
}
