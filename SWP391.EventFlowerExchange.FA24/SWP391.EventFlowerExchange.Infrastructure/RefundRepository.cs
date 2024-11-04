using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public class RefundRepository : IRefundRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;
        private INotificationRepository _notificationRepository;
        private ITransactionRepository _transactionRepository;
        private IAccountRepository _accountRepository;
        private IRequestRepository _requestRepository;
        private IDeliveryLogRepository _deliveryLogRepository;
        private IOrderRepository _orderRepository;

        public RefundRepository(INotificationRepository notificationRepository, ITransactionRepository transactionRepository, IAccountRepository accountRepository, IRequestRepository requestRepository, IDeliveryLogRepository deliveryLogRepository, IOrderRepository orderRepository)
        {
            _notificationRepository = notificationRepository;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _requestRepository = requestRepository;
            _deliveryLogRepository = deliveryLogRepository;
            _orderRepository = orderRepository;
        }

        public decimal CheckFeeShipForOrderBatch(string address)
        {
            if (address.ToLower().Contains("binh chanh") || address.ToLower().Contains("can gio") || address.ToLower().Contains("cu chi") || address.ToLower().Contains("hoc mon") || address.ToLower().Contains("nha be"))
            {
                return 100000;
            }
            else
            {
                return 80000;
            }
        }

        public async Task<IdentityResult> BuyerReturnActionAsync(Account buyer, Order order)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            //gửi request lên admin
            var newRequest = new CreateRequest()
            {
                Amount = order.TotalPrice - this.CheckFeeShipForOrderBatch(order.DeliveredAt),
                UserId = buyer.Id,
                RequestType = "Refund",
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            await _requestRepository.CreateRequestAsync(newRequest);

            //trừ 50% tiền ship cho buyer 

            var minusMoney = this.CheckFeeShipForOrderBatch(order.DeliveredAt);
            var buyerAccount = await _accountRepository.GetUserByEmailAsync(buyer);

            buyerAccount.Balance -= minusMoney * 0.5m;

            await _accountRepository.UpdateAccountAsync(buyerAccount);

            //Cong tien vao he thong
            var system = await _accountRepository.GetUserByEmailAsync(new Account() { Email = "ManagerEVESystem@gmail.com" });
            system.Balance += minusMoney * 0.5m;
            await _accountRepository.UpdateAccountAsync(system);

            //tạo transaction
            var newTransaction = new Transaction()
            {
                Amount = minusMoney * 0.5m,
                CreatedAt = DateTime.Now,
                OrderId = order.OrderId,
                UserId = buyer.Id,
                TransactionType = 2,
                Status = true
            };

            await _transactionRepository.CreateTransactionAsync(newTransaction);

            //gửi thông báo lên hệ thống

            var newNotification = new CreateNotification()
            {
                UserEmail = buyer.Email,
                Content = "You have requested a refund for the order " + order.OrderId + ". Please wait for the admin to approve!"
            };

            await _notificationRepository.CreateNotificationAsync(newNotification);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateRefundRequestStatusAsync(Account staff, Order order)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            //gửi thông báo cho nguồi mua
            var buyer = await _accountRepository.GetUserByIdAsync(new Account() { Id = order.BuyerId });
            var newNotification = new CreateNotification()
            {
                UserEmail = buyer.Email,
                Content = "Your refund request for order " + order.OrderId + " has been approved!"
            };

            await _notificationRepository.CreateNotificationAsync(newNotification);

            //tạo request cho người bán, ở trạng thái pending
            var getStaff = await _accountRepository.GetUserByEmailAsync(staff);

            var newRequest = new CreateRequest()
            {

                Amount = order.TotalPrice - this.CheckFeeShipForOrderBatch(order.DeliveredAt),
                UserId = getStaff.Id,
                RequestType = "Refund",
                Status = "Pending",
                CreatedAt = DateTime.Now

            };

            await _requestRepository.CreateRequestAsync(newRequest);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateSellerRefundConfirmationStatusAsync(Order order)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            //Tiền từ hệ thống sẽ chuyển về cho người mua 
            var buyer = await _accountRepository.GetUserByIdAsync(new Account() { Id = order.BuyerId });
            buyer.Balance += order.TotalPrice - this.CheckFeeShipForOrderBatch(order.DeliveredAt);

            await _accountRepository.UpdateAccountAsync(buyer);

            //Cong tien vao he thong
            var system = await _accountRepository.GetUserByEmailAsync(new Account() { Email = "ManagerEVESystem@gmail.com" });
            system.Balance -= order.TotalPrice - this.CheckFeeShipForOrderBatch(order.DeliveredAt);

            await _accountRepository.UpdateAccountAsync(system);

            //tạo transaction
            var newTransaction = new Transaction()
            {
                Amount = order.TotalPrice - this.CheckFeeShipForOrderBatch(order.DeliveredAt),
                TransactionContent = "Refund for order " + order.OrderId,
                CreatedAt = DateTime.Now,
                OrderId = order.OrderId,
                UserId = buyer.Id,
                TransactionType = 1,
                Status = true
            };

            await _transactionRepository.CreateTransactionAsync(newTransaction);

            //tạo thông báo cho người mua
            var newNotification = new CreateNotification()
            {
                UserEmail = buyer.Email,
                Content = "Your money refund for order " + order.OrderId + " has been sended!"
            };

            await _notificationRepository.CreateNotificationAsync(newNotification);

            return IdentityResult.Success;
        }
    }
}
