using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        Task<int> GetId(T obj, int input);
    }
}
