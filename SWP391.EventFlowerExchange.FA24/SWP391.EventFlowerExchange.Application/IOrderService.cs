using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IOrderService
    {
        public Task<List<Order>> ViewAllOrderFromAPIAsync();
        public Task<List<GetProduct>> ViewOrderDetailFromAPIAsync(Order order);
        public Task<List<Order>> ViewOrderByBuyerIdFromAPIAsync(Account account);
        public Task<List<Order>> ViewOrderBySellerIdFromAPIAsync(Account account);
        public Task<List<Order>> ViewOrderByShipperIdFromAPIAsync(Account account);
        public Task<List<Order>> ViewOrderByStatusFromAPIAsync(Order order);
        public Task<Order> SearchOrderByOrderIdFromAPIAsync(Order order);
        public Task<bool> CreateOrderFromAPIAsync(DeliveryInformation deliveryInformation, Voucher voucher);
        public Task<bool> CreateOrderBySellerFromAPIAsync(CreateOrderBySeller createOrderBySeller);
        public Task<bool> UpdateOrderStatusFromAPIAsync(Order order);
        public Task<bool> UpdateOrderStatusByUserFromAPIAsync(Order order, string status);
        public decimal CheckFeeShipForOrderEvent(string address);
        public decimal CheckFeeShipForOrderBatch(string address);
        public Task<bool> CheckFeeShipEventOrBatchFromAPIAsync(List<int> productIdList);

        public Dictionary<string, int> GetMonthlyOrderStatisticsFromAPI();

        public Task<string> DivideProductHasSameSellerFromAPIAsync(List<int> productIdList);
        public Task<CheckOutAfter> CheckOutOrderFromAPIAsync(string address, List<int> productList, Voucher voucher);

        public Task<OrderItem> SearchOrderItemByProductIdFromAPIAsync(GetProduct product);

    }
}
