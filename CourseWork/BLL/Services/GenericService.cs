using Helpers.Validators;
using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace BLL.Services
{
    public abstract class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IStorage<T> _storage;

        protected GenericService(IStorage<T> storage)
        {
            _storage = storage;
        }

        public virtual async Task Add(T obj)
        {
            try
            {
                var result = await _storage.AddAsync(obj);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to add {typeof(T).Name}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add {typeof(T).Name}. Exception: {ex.Message}");
            }
        }

        public virtual async Task Remove(int id)
        {
            try
            {
                var result = await _storage.RemoveAsync(id);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to delete {typeof(T).Name} with Id {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete {typeof(T).Name} with Id {id}. Exception: {ex.Message}");
            }
        }

        public virtual async Task<List<T>> GetAll()
        {
            var result = await _storage.GetAllAsync();

            if (!result.IsSuccessful)
            {
                throw new Exception($"Failed to get all {typeof(T).Name}s.");
            }

            return result.Data;
        }

        public virtual async Task Update(int id, T obj)
        {
            try
            {
                var result = await _storage.UpdateAsync(id, obj);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to update {typeof(T).Name} with Id {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update {typeof(T).Name} with Id {id}. Exception: {ex.Message}");
            }
        }

        public async Task<T> GetByPredicate(Func<T, bool> predicate)
        {
            try
            {
                var result = await _storage.GetByPredicateAsync(predicate);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to get by predicate {typeof(T).Name}s.");
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get by predicate {typeof(T).Name}s. Exception: {ex.Message}");
            }
        }

        public virtual async Task<T> GetById(int Id)
        {
            try
            {
                var result = await _storage.GetByIdAsync(Id);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to get {typeof(T).Name} by Id {Id}.");
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get {typeof(T).Name} by Id {Id}. Exception: {ex.Message}");
            }
        }
    }
}
