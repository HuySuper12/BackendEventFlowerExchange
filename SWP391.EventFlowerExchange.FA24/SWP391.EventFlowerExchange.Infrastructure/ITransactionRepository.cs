using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface ITransactionRepository
    {
        public Task<List<Transaction>> ViewAllTransactionAsync();
        public Task<List<Transaction>> ViewAllTransactionByUserIdAsync(Account account);
        public Task<Transaction> ViewTransactionByIdAsync(Transaction transaction);
        public Task<Transaction> ViewTransactionByCodeAsync(Transaction transaction);
        public Task<List<Transaction>> ViewAllTransactionByOrderIdAsync(Order order);
        public Task<bool> CreateTransactionAsync(Transaction transaction);

        public Dictionary<string, decimal> GetRevenueOrderStatistics();
    }
}
