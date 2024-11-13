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
    public class DeliveryLogRepository : IDeliveryLogRepository
    {

        private Swp391eventFlowerExchangePlatformContext _context;

        public async Task<bool> CreateDeliveryLogAsync(DeliveryLog deliveryLog)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.DeliveryLogs.Add(deliveryLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDeliveryLogStatusAsync(DeliveryLog deliveryLog)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.DeliveryLogs.Update(deliveryLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DeliveryLog>> ViewAllDeliveryLogAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.DeliveryLogs.ToListAsync();
        }

        public async Task<DeliveryLog> ViewDeliveryLogByOrderIdAsync(Order order)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.DeliveryLogs.FirstOrDefaultAsync(x => x.OrderId == order.OrderId);
        }

        public async Task<List<DeliveryLog>> ViewDeliveryLogByShipperIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.DeliveryLogs.Where(x => x.DeliveryPersonId == account.Id).ToListAsync();
        }

        public async Task<DeliveryTime> ViewDeliveryTimeAsync(Order order)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var deliveryLog = await _context.DeliveryLogs.FirstOrDefaultAsync(x => (int)x.OrderId == order.OrderId);

            if (deliveryLog == null)
            {
                return null;
            }

            DeliveryTime deliveryTime = new DeliveryTime()
            {
                TakeOverTime = deliveryLog.CreatedAt,
                DeliveringTime = deliveryLog.TakeOverAt,
                SuccessOrFailTime = deliveryLog.DeliveryAt
            };
            return deliveryTime;
        }

        public async Task<DeliveryLog> ViewDeliveryLogDeliveringByShipperIdAsync(DeliveryLog deliveryLog)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var deliveryLogList = await _context.DeliveryLogs.Where(x => x.DeliveryPersonId == deliveryLog.DeliveryPersonId).ToListAsync();
            for (int i = 0; i < deliveryLogList.Count; i++)
            {
                if (deliveryLogList[i].DeliveryAt == null)
                {
                    return deliveryLogList[i];
                }
            }
            return null;
        }

        public async Task<Boolean> CheckShipperIsFree(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var deliveryLogList = await _context.DeliveryLogs.Where(x => x.DeliveryPersonId == account.Id).ToListAsync();

            if (deliveryLogList.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < deliveryLogList.Count; i++)
            {
                if (deliveryLogList[i].DeliveryAt == null)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<DeliveryLog> ViewDeliveryLogDeliveringByOrderIdAsync(Order order)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var x = await _context.DeliveryLogs.FirstOrDefaultAsync(x => x.OrderId == order.OrderId && x.DeliveryAt == null);
            return x;
        }
    }
}
