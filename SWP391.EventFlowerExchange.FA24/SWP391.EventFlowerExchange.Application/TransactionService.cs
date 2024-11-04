using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _repo;

        public TransactionService(ITransactionRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> CreateTransactionFromAPIAsync(Transaction transaction)
        {
            return await _repo.CreateTransactionAsync(transaction);
        }

        public async Task<List<Transaction>> ViewAllTransactionByOrderIdFromAPIAsync(Order order)
        {
            return await _repo.ViewAllTransactionByOrderIdAsync(order);
        }

        public async Task<List<Transaction>> ViewAllTransactionByUserIdFromAPIAsync(Account account)
        {
            return await _repo.ViewAllTransactionByUserIdAsync(account);
        }

        public async Task<List<Transaction>> ViewAllTransactionFromAPIAsync()
        {
            return await _repo.ViewAllTransactionAsync();
        }

        public async Task<Transaction> ViewTransactionByCodeFromAPIAsync(Transaction transaction)
        {
            return await _repo.ViewTransactionByCodeAsync(transaction);
        }

        public async Task<Transaction> ViewTransactionByIdFromAPIAsync(Transaction transaction)
        {
            return await _repo.ViewTransactionByIdAsync(transaction);
        }

        public Dictionary<string, decimal> GetRevenueOrderStatisticsFromAPI()
        {
            return _repo.GetRevenueOrderStatistics();
        }
    }
}
