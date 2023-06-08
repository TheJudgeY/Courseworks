using Core.Models;
using Helpers.Validators;

namespace DAL.Abstractions.Interfaces
{
    public interface IStorage<T> where T : BaseEntity
    {
        Task<Result<List<T>>> GetAllAsync(int pageNumber = 1, int pageSize = 10);

        Task<Result<T>> GetByIdAsync(int id);

        Task<Result<T>> GetByPredicateAsync(Func<T, bool> predicate);

        Task<Result<bool>> AddAsync(T obj);

        Task<Result<bool>> UpdateAsync(int id, T updatedObj);

        Task<Result<bool>> RemoveAsync(int id);
    }
}
