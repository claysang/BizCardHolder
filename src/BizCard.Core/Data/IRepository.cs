using System.Linq;
using System.Threading.Tasks;

namespace BizCard.Core.Data
{
    public interface IRepository<T>
    {
        Task Save(T entity);

        IQueryable<T> All();

        T Get(int id);

        void Delete(T entity);

        void Update(T entity);
    }
}
