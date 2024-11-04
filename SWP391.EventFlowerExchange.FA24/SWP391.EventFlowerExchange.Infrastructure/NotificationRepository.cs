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

        public async Task<int> CountNotificationsByEmailAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            int count = await _context.Notifications.Where(x => x.UserId == account.Id).CountAsync();
            return count;
        }

        public async Task<int> CountShopNotificationByEmailAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            int count = await _context.ShopNotifications.Where(x => x.FollowerId == account.Id).CountAsync();
            return count;
        }

        public async Task<IdentityResult> CreateNotificationAsync(CreateNotification notification)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var acc = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == notification.UserEmail);

            var noti = new Notification
            {
                UserId = acc.Id,
                Content = notification.Content,
                CreatedAt = DateTime.Now,
                Status = "Enable"
            };

            await _context.Notifications.AddAsync(noti);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> CreateShopNotificationAsync(CreateShopNotification notification)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var acc = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == notification.FollowerẸmail);
            var acc2 = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == notification.SellerEmail);

            var noti = new ShopNotification
            {
                SellerId = acc2.Id,
                ProductId = notification.ProductId,
                FollowerId = acc.Id,
                Content = notification.Content,
                CreatedAt = DateTime.Now,
                Status = "Enable"
            };

            await _context.ShopNotifications.AddAsync(noti);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
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

            var noti = await _context.Notifications.Where(x => x.UserId == account.Id && x.Status.Contains("enable")).ToListAsync();
            if (noti != null)
            {
                return noti;
            }

            return null;
        }

        public async Task<List<ShopNotification>> ViewAllShopNotificationAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            return await _context.ShopNotifications.ToListAsync();
        }

        public async Task<List<ShopNotification>> ViewAllShopNotificationByUserIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var noti = await _context.ShopNotifications.Where(x => x.FollowerId == account.Id && x.Status.Contains("enable")).ToListAsync();
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

        public async Task<ShopNotification> ViewShopNotificationByIdAsync(ShopNotification notification)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var noti = await _context.ShopNotifications.FirstOrDefaultAsync(p => p.ShopNotificationId == notification.ShopNotificationId);
            if (noti != null)
            {
                return noti;
            }

            return null;
        }
    }
}
