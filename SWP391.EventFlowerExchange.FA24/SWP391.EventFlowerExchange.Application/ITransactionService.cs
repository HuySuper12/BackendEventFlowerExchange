using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface ITransactionService
    {
        public Task<List<Transaction>> ViewAllTransactionFromAPIAsync();
        public Task<List<Transaction>> ViewAllTransactionByUserIdFromAPIAsync(Account account);
        public Task<Transaction> ViewTransactionByIdFromAPIAsync(Transaction transaction);
        public Task<Transaction> ViewTransactionByCodeFromAPIAsync(Transaction transaction);
        public Task<List<Transaction>> ViewAllTransactionByOrderIdFromAPIAsync(Order order);
        public Task<bool> CreateTransactionFromAPIAsync(Transaction transaction);

        public Dictionary<string, decimal> GetRevenueOrderStatisticsFromAPI();
    }
}
