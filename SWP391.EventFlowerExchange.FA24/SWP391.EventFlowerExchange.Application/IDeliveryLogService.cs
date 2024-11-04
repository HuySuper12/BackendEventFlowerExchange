using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IDeliveryLogService
    {
        public Task<List<DeliveryLog>> ViewAllDeliveryLogFromAsync();

        public Task<List<DeliveryLog>> ViewDeliveryLogByShipperIdFromAsync(Account account);

        public Task<DeliveryLog> ViewDeliveryLogByOrderIdFromAsync(Order order);

        public Task<bool> CreateDeliveryLogFromAsync(DeliveryLog deliveryLog);

        public Task<bool> UpdateDeliveryLogStatusFromAsync(DeliveryLog deliveryLog);

        public Task<DeliveryTime> ViewDeliveryTimeFromAPIAsync(Order order);

        public Task<DeliveryLog> ViewDeliveryLogDeliveringByShipperIdFromAPIAsync(DeliveryLog deliveryLog);

        public Task<DeliveryLog> ViewDeliveryLogDeliveringByOrderIdFromAsync(Order order);

    }
}
