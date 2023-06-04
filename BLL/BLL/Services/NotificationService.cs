using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NotificationService : GenericService<Notification>, INotificationService
    {
        public NotificationService(IStorage<Notification> storage) : base(storage)
        {
        }

        public async Task<List<string>> GetNotificationsByUserId(int id)
        {
            var notifications = await GetAll();
            List<string> result = new List<string>();

            foreach (var notification in notifications.Where(n => n.UserId == id)) 
            {
                result.Add(notification.Content);
            }

            return result;
        }
    }
}
