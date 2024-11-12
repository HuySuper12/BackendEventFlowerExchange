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
    public class MessageService : IMessageService
    {
        private IMessageRepository _repo;

        public MessageService(IMessageRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateMessageFromApiAsync(CreateMessage message)
        {
            return await _repo.CreateMessageAsync(message);
        }

        public async Task<List<Message>> GetMessagesByReceiverIdFromApiAsync(Account sender, Account receiver)
        {
            return await _repo.GetMessagesByReceiverIdAsync(sender, receiver);
        }
    }
}
