using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IRefundService refundService;
        private readonly IAccountService accountService;
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly IDeliveryLogService deliveryLogService;
        private readonly INotificationService noti;

        public RefundController(IRefundService refundService, IAccountService accountService, IProductService productService, IOrderService orderService, IDeliveryLogService deliveryLogService, INotificationService noti)
        {
            this.refundService = refundService;
            this.accountService = accountService;
            this.productService = productService;
            this.orderService = orderService;
            this.deliveryLogService = deliveryLogService;
            this.noti = noti;
        }

        [HttpPost("BuyerReturnAction")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> BuyerReturnAction(int orderId)
        {
            var getOrder = new Order { OrderId = orderId };
            var order = await orderService.SearchOrderByOrderIdFromAPIAsync(getOrder);

            var getBuyer = new Account { Id = order.BuyerId };
            var buyer = await accountService.GetUserByIdFromAPIAsync(getBuyer);

            if (buyer != null && order != null)
            {
                var result = await refundService.BuyerReturnActionFromApiAsync(buyer, order);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        [HttpPost("CreateDeliveryLogForRefund")]
        //[Authorize(Roles = ApplicationRoles.Staff)]
        public async Task<ActionResult<bool>> CreateDeliveryLogForRefund(CreateDeliveryLog createDeliveryLog)
        {
            var deliveryLogResult = await deliveryLogService.ViewDeliveryLogByOrderIdFromAsync(new Order() { OrderId = (int)createDeliveryLog.OrderId });

            if (deliveryLogResult != null)
            {
                var account = await accountService.GetUserByEmailFromAPIAsync(new Account() { Email = createDeliveryLog.DeliveryPersonEmail });
                DeliveryLog deliveryLog = new DeliveryLog()
                {
                    OrderId = createDeliveryLog.OrderId,
                    DeliveryPersonId = account.Id,
                    CreatedAt = DateTime.Now
                };
                await deliveryLogService.CreateDeliveryLogFromAsync(deliveryLog);

                //Gan shipper vao don hang, thay doi trang thai don hang thanh giao hang
                var order = await orderService.SearchOrderByOrderIdFromAPIAsync(new Order() { OrderId = (int)createDeliveryLog.OrderId });
                order.Status = "Refund";
                order.DeliveryPersonId = account.Id;
                order.UpdateAt = DateTime.Now;
                await orderService.UpdateOrderStatusFromAPIAsync(order);

                var accountSeller = await accountService.GetUserByIdFromAPIAsync(new Account() { Id = order.SellerId });

                //Gui thong bao don hang dang giao
                CreateNotification notification = new CreateNotification()
                {
                    UserEmail = accountSeller.Email,
                    Content = "Shipper is coming to pick up your refund order"
                };
                await noti.CreateNotificationFromApiAsync(notification);

                return true;
            }
            return false;
        }

        [HttpPut("UpdateDeliveryLogRefundDeliveringStatus")]
        //[Authorize(Roles = ApplicationRoles.Shipper)]
        public async Task<ActionResult<bool>> UpdateDeliveryLogRefundDeliveringStatus(int orderId)
        {
            var deliveryLog = await deliveryLogService.ViewDeliveryLogDeliveringByOrderIdFromAsync(new Order() { OrderId = orderId });
            var order = await orderService.SearchOrderByOrderIdFromAPIAsync(new Order() { OrderId = orderId });
            var accountSeller = await accountService.GetUserByIdFromAPIAsync(new Account() { Id = order.SellerId });

            if (deliveryLog.Status == null)
            {
                deliveryLog.Status = "Delivering";
                deliveryLog.TakeOverAt = DateTime.Now;
                await deliveryLogService.UpdateDeliveryLogStatusFromAsync(deliveryLog);

                order.Status = "Delivering";
                await orderService.UpdateOrderStatusFromAPIAsync(order);

                CreateNotification notification = new CreateNotification()
                {
                    UserEmail = accountSeller.Email,
                    Content = "Your refund order is on its way"
                };
                await noti.CreateNotificationFromApiAsync(notification);

                return true;
            }
            return false;
        }

        [HttpPut("UpdateDeliveryLogRefundSuccessStatus")]
        //[Authorize(Roles = ApplicationRoles.Shipper)]
        public async Task<ActionResult<bool>> UpdateDeliveryLogRefundSuccessStatus(int orderId, string url)
        {
            var deliveryLog = await deliveryLogService.ViewDeliveryLogDeliveringByOrderIdFromAsync(new Order() { OrderId = orderId });

            if (deliveryLog == null)
            {
                return false;
            }

            deliveryLog.Status = "Delivery Success";
            deliveryLog.PhotoUrl = url;
            deliveryLog.DeliveryAt = DateTime.Now;
            await deliveryLogService.UpdateDeliveryLogStatusFromAsync(deliveryLog);

            //Cap nhat thoi gian chuyen tien cho seller
            var order = await orderService.SearchOrderByOrderIdFromAPIAsync(new Order() { OrderId = orderId });

            if (order.UpdateAt != null)
            {
                order.Status = "Success";
                order.UpdateAt = DateTime.Now.AddDays(2);
                await orderService.UpdateOrderStatusFromAPIAsync(order);

                var account = await accountService.GetUserByIdFromAPIAsync(new Account() { Id = order.SellerId });

                CreateNotification notification = new CreateNotification()
                {
                    UserEmail = account.Email,
                    Content = "Your refund order has been delivered successfully."
                };

                await noti.CreateNotificationFromApiAsync(notification);

                return true;
            }
            return false;
        }

        [HttpPost("StaffApproveRefundRequest")]
        //[Authorize(Roles = ApplicationRoles.Staff)]
        public async Task<ActionResult<bool>> StaffApproveRefundRequest(string staffEmail, int orderId)
        {
            var getOrder = new Order { OrderId = orderId };
            var order = await orderService.SearchOrderByOrderIdFromAPIAsync(getOrder);

            var getStaff = new Account { Email = staffEmail };
            var staff = await accountService.GetUserByEmailFromAPIAsync(getStaff);

            if (staff != null && order != null)
            {
                var result = await refundService.UpdateRefundRequestStatusFromApiAsync(staff, order);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        [HttpPost("UpdateSellerRefundConfirmationStatus")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<ActionResult<bool>> UpdateSellerRefundConfirmationStatus(int orderId)
        {
            var getOrder = new Order { OrderId = orderId };
            var order = await orderService.SearchOrderByOrderIdFromAPIAsync(getOrder);

            if (order != null)
            {
                var result = await refundService.UpdateSellerRefundConfirmationStatusFromApiAsync(order);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

    }
}
