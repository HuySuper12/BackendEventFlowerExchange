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
    public class TransactionRepository : ITransactionRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transaction>> ViewAllTransactionAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Transactions.ToListAsync();
        }

        public async Task<List<Transaction>> ViewAllTransactionByUserIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Transactions.Where(x => x.UserId == account.Id).ToListAsync();
        }

        public async Task<List<Transaction>> ViewAllTransactionByOrderIdAsync(Order order)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Transactions.Where(x => x.OrderId == order.OrderId).ToListAsync();
        }

        public async Task<Transaction> ViewTransactionByIdAsync(Transaction transaction)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var searchTransaction = await _context.Transactions.FirstOrDefaultAsync(x => x.TransactionId == transaction.TransactionId);
            if (searchTransaction != null)
            {
                return searchTransaction;
            }
            return null;
        }

        public async Task<Transaction> ViewTransactionByCodeAsync(Transaction transaction)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Transactions.FirstOrDefaultAsync(x => x.TransactionCode == transaction.TransactionCode);
        }

        public Dictionary<string, decimal> GetRevenueOrderStatistics()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var statistics = _context.Transactions
            .Where(transaction => transaction.CreatedAt.HasValue)
            .GroupBy(order => new { Year = order.CreatedAt.Value.Year, Month = order.CreatedAt.Value.Month })
            .ToDictionary(
                g => $"{g.Key.Year}-{g.Key.Month}",  // Key: Năm và tháng
                g => GetRevenueSystem(g.ToList())  // Value: Doanh thu theo thang
            );
            return statistics;
        }

        //Them ham view transaction by orderId => Lay ra dc list order, (Lay thang giao dich dau tru giao dich sau
        public decimal GetRevenueSystem(List<Transaction> transactions)
        {
            decimal totalPrice = 0;

            for (int i = 0; i < transactions.Count; i++)
            {
                if (transactions[i].Status == true)
                    totalPrice += (decimal)transactions[i].Amount;
                else
                {
                    totalPrice -= (decimal)transactions[i].Amount;
                }
            }

            return totalPrice;
        }

    }
}
