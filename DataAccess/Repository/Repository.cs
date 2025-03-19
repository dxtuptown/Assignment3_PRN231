using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MyDBContext _context;
        private readonly dynamic _dao;
        public Repository(MyDBContext context, dynamic dao)
        {
            _context = context;
            _dao = dao;
        }

        public IEnumerable<T> GetAll() => _dao.GetAll();
        public T GetById(int id) => _dao.GetById(id);
        public void Add(T entity) => _dao.Add(entity);
        public void Update(T entity) => _dao.Update(entity);
        public void Delete(int id) => _dao.Delete(id);
    }
}
