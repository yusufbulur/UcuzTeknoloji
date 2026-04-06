using UcuzTeknoloji.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UcuzTeknoloji.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            Save();
        }

        public void Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(entity);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}