using System.Linq.Expressions;

namespace StudentManagement.Repository.IRepository
{
    public interface IRepository<T>  where T : class  
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null);

        T Get(Expression<Func<T,bool>> filter);
        void Add(T t);
        void Remove(T t);
        void Save();

    }
}
