using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IMessageRepository
    {
        public Task<bool> CreateMessageAsync(CreateMessage message);

        public Task<List<Message>> GetMessagesByReceiverIdAsync(Account sender, Account receiver);

        public Task<List<string>> GetMessagesBySenderIdAsync(Account sender);

        public Task<List<string>> GetMessagesByReceiveIdAsync(Account receiver);

    }
}
