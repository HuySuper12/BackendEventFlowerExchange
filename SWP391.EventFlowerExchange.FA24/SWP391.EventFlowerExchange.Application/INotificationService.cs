using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface INotificationService
    {
        public Task<List<Notification>> ViewAllNotificationFromApiAsync();
        public Task<List<Notification>> ViewAllNotificationByUserIdFromApiAsync(Account account);
        public Task<Notification> ViewNotificationByIdFromApiAsync(Notification notification);
        public Task<IdentityResult> CreateNotificationFromApiAsync(CreateNotification notification);
    }
}
