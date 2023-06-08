using Core.Models;
using Task = System.Threading.Tasks.Task;

namespace UI.Interfaces
{
    public interface IConsoleManager<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByPredicateAsync(Func<TEntity, bool> predicate);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(int id, TEntity entity);

        Task DeleteAsync(int id);
    }
}
