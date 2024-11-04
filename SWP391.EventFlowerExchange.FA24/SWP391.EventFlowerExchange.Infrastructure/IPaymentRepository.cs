using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IPaymentRepository
    {
        public Task<bool> CreatePayementAsync(CreatePayment newValue);

        public Task<Payment> GetPayementByCodeAsync(CreatePayment payment);

        public Task<List<Payment>> GetAllPaymentListByType(int type);

        public Task<List<Payment>> GetPayementByTypeAndEmailAsync(int type, Account account);

    }
}
