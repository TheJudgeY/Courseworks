using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.Interfaces
{
    public interface INotificationService : IGenericService<Notification>
    {
        Task<List<string>> GetNotificationsByUserId(int id);
    }
}
