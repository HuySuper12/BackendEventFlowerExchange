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
    public class OrderService : IOrderService
    {
        private IOrderRepository _repo;
        private ICartRepository _cartRepository;
        private IAccountRepository _accountRepository;
        private IProductRepository _productRepository;
        private ITransactionRepository _transactionRepository;
        private INotificationRepository _notificationRepository;

        public OrderService(IOrderRepository repo, ICartRepository cartRepository, IAccountRepository accountRepository, IProductRepository productRepository, ITransactionRepository transactionRepository, INotificationRepository notificationRepository)
        {
            _accountRepository = accountRepository;
            _repo = repo;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> CreateOrderFromAPIAsync(DeliveryInformation deliveryInformation, Voucher voucher)
        {
            var account = await _accountRepository.GetUserByEmailAsync(new Account() { Email = deliveryInformation.Email });
            for (var i = 0; i < deliveryInformation.Product.Count(); i++)
            {
                var product = await _productRepository.SearchProductByIdAsync(new GetProduct() { ProductId = deliveryInformation.Product[i] });
                if (product.Status == "Enable")
                    return await _repo.CreateOrderAsync(deliveryInformation, account, deliveryInformation.Product, voucher);
            }
            return false;
        }

        public async Task<bool> CreateOrderBySellerFromAPIAsync(CreateOrderBySeller createOrderBySeller)
        {
            //Cho tao nhieu don hang thang nao accept truoc thi cac don giong v bang null se cap nhat bang fail
            var accountBuyer = await _accountRepository.GetUserByEmailAsync(new Account() { Email = createOrderBySeller.BuyerEmail });
            var product = await _productRepository.SearchProductByIdAsync(new GetProduct() { ProductId = createOrderBySeller.ProductId });
            if (product.Status == "Enable")
                return await _repo.CreateOrderBySellerAsync(createOrderBySeller, accountBuyer, product);
            return false;
        }

        public async Task<List<GetProduct>> ViewOrderDetailFromAPIAsync(Order order)
        {
            return await _repo.ViewOrderDetailAsync(order);
        }

        public async Task<List<Order>> ViewAllOrderFromAPIAsync()
        {
            return await _repo.ViewAllOrderAsync();
        }

        public async Task<List<Order>> ViewOrderByBuyerIdFromAPIAsync(Account account)
        {
            return await _repo.ViewOrderByBuyerIdAsync(account);
        }

        public async Task<OrderItem> SearchOrderItemByProductIdFromAPIAsync(GetProduct product)
        {
            return await _repo.SearchOrderItemByProductIdAsync(product);
        }

        public async Task<List<Order>> ViewOrderBySellerIdFromAPIAsync(Account account)
        {
            return await _repo.ViewOrderBySellerIdAsync(account);
        }
        public async Task<List<Order>> ViewOrderByShipperIdFromAPIAsync(Account account)
        {
            return await _repo.ViewOrderByShipperIdAsync(account);
        }

        public async Task<Order> SearchOrderByOrderIdFromAPIAsync(Order order)
        {
            return await _repo.SearchOrderByOrderIdAsync(order);
        }

        public Task<List<Order>> ViewOrderByStatusFromAPIAsync(Order order)
        {
            return _repo.ViewOrderByStatusAsync(order);
        }

        public async Task<bool> UpdateOrderStatusFromAPIAsync(Order order)
        {
            return await _repo.UpdateOrderStatusAsync(order);
        }

        public Dictionary<string, int> GetMonthlyOrderStatisticsFromAPI()
        {
            return _repo.GetMonthlyOrderStatistics();
        }

        public decimal CheckFeeShipForOrderEvent(string address)
        {
            return _repo.CheckFeeShipForOrderEvent(address);
        }

        public decimal CheckFeeShipForOrderBatch(string address)
        {
            return _repo.CheckFeeShipForOrderBatch(address);
        }

        public async Task<bool> CheckFeeShipEventOrBatchFromAPIAsync(List<int> productIdList)
        {
            return await _repo.CheckFeeShipEventOrBatchAsync(productIdList);
        }

        public async Task<string> DivideProductHasSameSellerFromAPIAsync(List<int> productIdList)
        {
            return await _repo.DivideProductHasSameSellerAsync(productIdList);
        }

        public async Task<CheckOutAfter> CheckOutOrderFromAPIAsync(string address, List<int> productList, Voucher voucher)
        {
            return await _repo.CheckOutOrderAsync(address, productList, voucher);
        }

        public async Task<bool> UpdateOrderStatusByUserFromAPIAsync(Order order, string status)
        {
            var orderItem = await _repo.ViewOrderDetailAsync(order);
            if (order.Status == null && status == "Accepted")
            {
                order.Status = "Pending";
                await _repo.UpdateOrderStatusAsync(order);

                //Cap nhat trang thai fail cho cac order cung mua product do
                var orderList = await _repo.SearchOrderItemByProductAsync(new GetProduct() { ProductId = orderItem[0].ProductId });
                for (int i = 0; i < orderList.Count; i++)
                {
                    if (orderList[i].Status != "Pending")
                    {
                        orderList[i].Status = "Fail";
                        await _repo.UpdateOrderStatusAsync(orderList[i]);

                        var account = await _accountRepository.GetUserByIdAsync(new Account() { Id = orderList[i].BuyerId });

                        CreateNotification notification = new CreateNotification()
                        {
                            UserEmail = account.Email,
                            Content = "Your product had been bought by another buyer."
                        };
                        await _notificationRepository.CreateNotificationAsync(notification);
                    }
                }

                //Xoa tung san pham trong gio hang (Do co 1 sp nen orderItem[0])
                await _cartRepository.RemoveCartItemToCreateOrderAsync(new CartItem() { ProductId = orderItem[0].ProductId });

                //Tao giao dich
                var transactionCode = "EVE";
                while (true)
                {
                    transactionCode = "EVE" + new Random().Next(100000, 999999).ToString();
                    var result = await _transactionRepository.ViewTransactionByCodeAsync(new Transaction() { TransactionCode = transactionCode });
                    if (result == null)
                    {
                        break;
                    }
                }
                Transaction transaction = new Transaction()
                {
                    TransactionCode = transactionCode,
                    Amount = order.TotalPrice,
                    CreatedAt = DateTime.Now,
                    OrderId = order.OrderId,
                    UserId = order.BuyerId,
                    TransactionType = 1,
                    TransactionContent = $"Successful online purchase payment with order code is {order.OrderId}.",
                    Status = true
                };
                await _transactionRepository.CreateTransactionAsync(transaction);

                var accountBuyer = await _accountRepository.GetUserByIdAsync(new Account() { Id = order.BuyerId });
                var accountSeller = await _accountRepository.GetUserByIdAsync(new Account() { Id = order.SellerId });

                //Tru tien trong so du cua nguoi mua
                accountBuyer.Balance -= order.TotalPrice;
                await _accountRepository.UpdateAccountAsync(accountBuyer);

                //Cong tien vao he thong
                var system = await _accountRepository.GetUserByEmailAsync(new Account() { Email = "ManagerEVESystem@gmail.com" });
                system.Balance += order.TotalPrice;
                await _accountRepository.UpdateAccountAsync(system);

                //Gui thong bao cho nguoi ban va nguoi mua
                CreateNotification notificationBuyer = new CreateNotification()
                {
                    UserEmail = accountBuyer.Email,
                    Content = "Order payment successful.",
                };
                await _notificationRepository.CreateNotificationAsync(notificationBuyer);

                CreateNotification notificationSeller = new CreateNotification()
                {
                    UserEmail = accountSeller.Email,
                    Content = "Your product has been ordered. Please prepare the order.",
                };
                await _notificationRepository.CreateNotificationAsync(notificationSeller);

                return true;
            }
            else if (order.Status == null && status == "Rejected")
            {
                order.Status = "Rejected";
                await _repo.UpdateOrderStatusAsync(order);

                var accountBuyer = await _accountRepository.GetUserByIdAsync(new Account() { Id = order.BuyerId });
                var accountSeller = await _accountRepository.GetUserByIdAsync(new Account() { Id = order.SellerId });

                CreateNotification notificationSeller = new CreateNotification()
                {
                    UserEmail = accountSeller.Email,
                    Content = $"Your order has been rejected by buyer {accountBuyer.Name}",
                };
                await _notificationRepository.CreateNotificationAsync(notificationSeller);
                return true;
            }
            return false;
        }
    }
}
