using BLL.Abstractions.Interfaces;
using Core.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers
{
    public abstract class ConsoleManager<TService, TEntity> 
    where TEntity : BaseEntity 
    where TService : IGenericService<TEntity>
    {
        protected readonly TService Service;

        protected ConsoleManager(TService service) 
        { 
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public virtual async Task <IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await Service.GetAll();
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error appeared in GetAllAsync: {ex.Message}");
            }
        }

        public virtual async Task UpdateAsync(int id, TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                await Service.Update(id, entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            }
        }
    }
}
