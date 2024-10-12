using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWP391.EventFlowerExchange.Domain;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public class NotificationRepository : INotificationRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;

        public NotificationRepository(Swp391eventFlowerExchangePlatformContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateNotificationAsync(CreateNotification notification)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var noti = new Notification
            {
                UserId=notification.UserId,
                Content= notification.Content,
                CreatedAt = DateTime.UtcNow,
                Status = "true"
            };

            await _context.Notifications.AddAsync(noti);
            int result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<List<Notification>> ViewAllNotificationAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            return await _context.Notifications.ToListAsync();
        }

        public async Task<List<Notification>> ViewAllNotificationByUserIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var noti = await _context.Notifications.Where(x => x.UserId == account.Id).ToListAsync();
            if (noti != null)
            {
                return noti;
            }

            return null;
        }

        public async Task<Notification> ViewNotificationByIdAsync(Notification notification)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var noti = await _context.Notifications.FirstOrDefaultAsync(p => p.NotificationId == notification.NotificationId);
            if (noti != null)
            {
                return noti;
            }

            return null;
        }
    }
}
