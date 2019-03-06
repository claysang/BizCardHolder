using System.Linq;

namespace BizCard.Core.Data
{
    public interface IRepository<T>
    {
        void Save(T entity);

        IQueryable<T> All();

        T Get(int id);

        void Delete(T entity);

        void Update(T entity);
    }
}
