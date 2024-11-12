using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public class MessageRepository : IMessageRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;
        private readonly IAccountRepository _accountRepository;

        public MessageRepository(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> CreateMessageAsync(CreateMessage message)
        {
            _context = new();

            var sender = await _accountRepository.GetUserByEmailAsync(new Account() { Email = message.SenderEmail });

            var receiver = await _accountRepository.GetUserByEmailAsync(new Account() { Email = message.ReveiverEmail });

            var noti = new Notification
            {
                UserId = receiver.Id,
                Content = "You had received a message from user " + sender.Name,
                CreatedAt = DateTime.Now,
                Status = "Enable"
            };

            _context.Notifications.Add(noti);

            await _context.SaveChangesAsync();

            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.UserId == receiver.Id && x.CreatedAt == noti.CreatedAt);

            var msg = new Message
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Contents = message.Contents,
                CreatedAt = DateTime.Now,
                NotificationId = notification.NotificationId
            };

            _context.Messages.Add(msg);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Message>> GetMessagesByReceiverIdAsync(Account sender, Account receiver)
        {
            _context = new();
            return await _context.Messages.Where(x => x.ReceiverId == receiver.Id && x.SenderId == sender.Id).ToListAsync();
        }
    }
}
