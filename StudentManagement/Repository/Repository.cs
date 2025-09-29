using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Repository.IRepository;
using System.Linq.Expressions;

namespace StudentManagement.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public void Add(T t)
        {
            dbSet.Add(t);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }

        public void Remove(T t)
        {
            dbSet.Remove(t);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
