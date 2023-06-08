using Core.Models;
using Task = System.Threading.Tasks.Task;

namespace BLL.Abstractions.Interfaces
{
    public interface IGenericService<T> where T : BaseEntity
    {
        Task Add(T obj);

        Task Remove(int id);

        Task<T> GetById(int id);

        Task<List<T>> GetAll();

        Task<T> GetByPredicate(Func<T, bool> predicate);

        Task Update(int id, T obj);
    }
}
