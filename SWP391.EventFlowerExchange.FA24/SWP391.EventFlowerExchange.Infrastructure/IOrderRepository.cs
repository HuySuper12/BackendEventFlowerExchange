using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IOrderRepository
    {
        public Task<List<Order>> ViewAllOrderAsync();
        public Task<List<GetProduct>> ViewOrderDetailAsync(Order order);
        public Task<Order> SearchOrderByOrderIdAsync(Order order);
        public Task<List<Order>> ViewOrderByBuyerIdAsync(Account account);
        public Task<List<Order>> ViewOrderBySellerIdAsync(Account account);
        public Task<List<Order>> ViewOrderByShipperIdAsync(Account account);
        public Task<List<Order>> ViewOrderByStatusAsync(Order order);
        public Task<bool> CreateOrderAsync(DeliveryInformation deliveryInformation, Account account, List<int> productIdList, Voucher voucher);
        public Task<bool> CreateOrderBySellerAsync(CreateOrderBySeller createOrderBySeller, Account account, GetProduct product);
        public Task<bool> UpdateOrderStatusAsync(Order order);

        public Dictionary<string, int> GetMonthlyOrderStatistics();

        public decimal CheckFeeShipForOrderEvent(string address);
        public decimal CheckFeeShipForOrderBatch(string address);
        public Task<bool> CheckFeeShipEventOrBatchAsync(List<int> productIdList);
        public Task<string> DivideProductHasSameSellerAsync(List<int> productIdList);
        public Task<CheckOutAfter> CheckOutOrderAsync(string address, List<int> productList, Voucher voucher);
        public Task<List<Order>> SearchOrderItemByProductAsync(GetProduct product);

        public Task<OrderItem> SearchOrderItemByProductIdAsync(GetProduct product);
    }
}
