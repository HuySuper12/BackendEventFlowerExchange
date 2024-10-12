using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class NotificationService : INotificationService
    {
        private INotificationRepository _repo;
        private IAccountRepository _accountRepository;

        public NotificationService(INotificationRepository repo,IAccountRepository accountRepository)
        {
            _repo = repo;
            _accountRepository = accountRepository;
        }

        public async Task<IdentityResult> CreateNotificationFromApiAsync(CreateNotification notification)
        {
            Account check=new Account() { Id=notification.UserId };

            var result= _accountRepository.GetUserByIdAsync(check);
            if (result == null) 
            {
                return IdentityResult.Failed(new IdentityError() { Description = "User not found" });
            }

            return await _repo.CreateNotificationAsync(notification);
        }

        public async Task<List<Notification>> ViewAllNotificationByUserIdFromApiAsync(Account account)
        {
            return await _repo.ViewAllNotificationByUserIdAsync(account);
        }

        public async Task<List<Notification>> ViewAllNotificationFromApiAsync()
        {
            return await _repo.ViewAllNotificationAsync();
        }

        public async Task<Notification> ViewNotificationByIdFromApiAsync(Notification notification)
        {
            return await _repo.ViewNotificationByIdAsync(notification);
        }
    }
}
