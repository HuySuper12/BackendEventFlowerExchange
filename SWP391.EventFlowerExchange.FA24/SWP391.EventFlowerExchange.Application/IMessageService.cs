using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IMessageService
    {
        public Task<bool> CreateMessageFromApiAsync(CreateMessage message);

        public Task<List<Message>> GetMessagesByReceiverIdFromApiAsync(Account sender, Account receiver);
    }
}
