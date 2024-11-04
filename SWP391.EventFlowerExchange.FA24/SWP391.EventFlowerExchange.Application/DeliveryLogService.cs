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
    public class DeliveryLogService : IDeliveryLogService
    {

        private IDeliveryLogRepository _repo;

        public DeliveryLogService(IDeliveryLogRepository repo)
        {
            _repo = repo;
        }

        public Task<bool> CreateDeliveryLogFromAsync(DeliveryLog deliveryLog)
        {
            return _repo.CreateDeliveryLogAsync(deliveryLog);
        }

        public async Task<bool> UpdateDeliveryLogStatusFromAsync(DeliveryLog deliveryLog)
        {
            await _repo.UpdateDeliveryLogStatusAsync(deliveryLog);
            return true;
        }

        public async Task<List<DeliveryLog>> ViewAllDeliveryLogFromAsync()
        {
            return await _repo.ViewAllDeliveryLogAsync();
        }

        public async Task<DeliveryLog> ViewDeliveryLogByOrderIdFromAsync(Order order)
        {
            return await _repo.ViewDeliveryLogByOrderIdAsync(order);
        }

        public async Task<List<DeliveryLog>> ViewDeliveryLogByShipperIdFromAsync(Account account)
        {
            return await _repo.ViewDeliveryLogByShipperIdAsync(account);
        }

        public async Task<DeliveryTime> ViewDeliveryTimeFromAPIAsync(Order order)
        {
            return await _repo.ViewDeliveryTimeAsync(order);
        }

        public async Task<DeliveryLog> ViewDeliveryLogDeliveringByShipperIdFromAPIAsync(DeliveryLog deliveryLog)
        {
            return await _repo.ViewDeliveryLogDeliveringByShipperIdAsync(deliveryLog);
        }

        public async Task<DeliveryLog> ViewDeliveryLogDeliveringByOrderIdFromAsync(Order order)
        {
            return await _repo.ViewDeliveryLogDeliveringByOrderIdAsync(order);
        }
    }
}
