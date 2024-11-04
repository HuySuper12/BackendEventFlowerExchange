using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private IRequestService _service;
        private IAccountService _accountService;
        private IProductService _productService;
        private INotificationService _notificationService;
        private IFollowService _followService;

        public RequestController(IRequestService service, IAccountService accountService, IProductService productService, INotificationService notificationService, IFollowService followService)
        {
            _service = service;
            _accountService = accountService;
            _productService = productService;
            _notificationService = notificationService;
            _followService = followService;
        }

        [HttpGet("GetRequestList/{type}")]
        //[Authorize]
        public async Task<IActionResult> GetAllRequestList(string type)
        {
            return Ok(await _service.GetListRequestsFromAPIAsync(type));
        }

        [HttpGet("GetRequestListBy")]
        //[Authorize]
        public async Task<IActionResult> GetListRequestsByEmailAndType(string type, string email)
        {
            var account = await _accountService.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            return Ok(await _service.GetListRequestsByEmailAndTypeAsync(type, account));
        }

        [HttpGet("GetRequestById")]
        //[Authorize]
        public async Task<IActionResult> GetRequestById(int id)
        {
            return Ok(await _service.GetRequestByIdFromAPIAsync(id));
        }

        [HttpPost("CreateRequest_Withdraw")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<ActionResult<bool>> CreateNewRequest(RequestPayment value)
        {
            var account = await _accountService.GetUserByEmailFromAPIAsync(new Account() { Email = value.UserEmail });
            var convert = new CreateRequest()
            {
                Amount = value.Amount,
                CreatedAt = DateTime.Now,
                RequestType = value.RequestType,
                UserId = account.Id,
                Status = "Pending"
            };
            var request = await _service.CreateRequestFromAPIAsync(convert);
            return Ok(request);
        }

        [HttpPost("CreateRequest")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<ActionResult<bool>> CreateRequest(CreateRequest value)
        {
            bool request = false;
            switch (value.RequestType)
            {
                case "Report":
                    {
                        value.RequestId = null;
                        value.Amount = null;
                        value.PaymentId = null;
                        request = await _service.CreateRequestFromAPIAsync(value);
                        break;
                    }
                case "Refund":
                    {
                        value.RequestId = null;
                        value.Amount = null;
                        value.PaymentId = null;
                        request = await _service.CreateRequestFromAPIAsync(value);
                        break;
                    }
            }

            return Ok(request);
        }

        [HttpPut("UpdateRequest")]
        //[Authorize]
        public async Task<ActionResult<bool>> UpdateRequest(CreateRequest value)
        {
            var user = await _accountService.GetUserByIdFromAPIAsync(new Account() { Id = value.UserId });
            Request request = null;
            GetProduct product = null;

            switch (value.RequestType)
            {
                case "Post":
                    {
                        product = await _productService.SearchProductByIdFromAPIAsync(new GetProduct() { ProductId = (int)value.ProductId });
                        value.PaymentId = null;
                        value.Amount = null;

                        if (product != null)
                        {
                            product.Status = "Enable";
                            if (value.Status.ToLower().Contains("Rejected".ToLower()))
                            {
                                product.Status = value.Status;
                            }
                            var check = await _productService.UpdateProductFromAPIAsync(product);
                            if (check)
                            {
                                var sellerNotification = new CreateNotification();

                                if (value.Status.ToLower().Contains("Accepted".ToLower()))
                                {
                                    var followerList = await _followService.GetFollowerListFromApiAsync(user);

                                    //Thông báo cho chủ shop
                                    {
                                        sellerNotification.UserEmail = user.Email;
                                        sellerNotification.Content = $"Your product post has been accepted.";
                                    };


                                    //Thông báo cho followers
                                    foreach (var item in followerList)
                                    {
                                        var follower = await _accountService.GetUserByIdFromAPIAsync(new Account() { Id = item.FollowerId });
                                        var followerNotification = new CreateShopNotification()
                                        {
                                            FollowerẸmail = follower.Email,
                                            SellerEmail = user.Email,
                                            ProductId = product.ProductId,
                                            Content = $"{user.Name} has added a new product in the shop you followed "
                                        };
                                        await _notificationService.CreateShopNotificationFromApiAsync(followerNotification);
                                    }
                                }
                                else if (value.Status.ToLower().Contains("Rejected".ToLower()))
                                {
                                    sellerNotification.UserEmail = user.Email;
                                    sellerNotification.Content = $"Your product post has been rejected.";
                                }
                                await _notificationService.CreateNotificationFromApiAsync(sellerNotification);
                            }
                        }
                        request = await _service.GetRequestByProductIdFromAPIAsync((int)value.ProductId);
                        break;
                    }

                case "Withdraw":
                    {
                        value.ProductId = null;
                        value.PaymentId = null;
                        if (value.Status.ToLower().Contains("Rejected".ToLower()))
                        {
                            var sellerNotification = new CreateNotification()
                            {
                                UserEmail = user.Email,
                                Content = $"Your withdrawal request has been rejected."
                            };
                            await _notificationService.CreateNotificationFromApiAsync(sellerNotification);
                        }
                        request = await _service.GetRequestByIdFromAPIAsync((int)value.RequestId);
                        break;
                    }

                case "Report":
                    {
                        value.PaymentId = null;
                        value.Amount = null;
                        product = await _productService.SearchProductByIdFromAPIAsync(new GetProduct() { ProductId = (int)value.ProductId });

                        var buyerNotification = new CreateNotification();
                        if (value.Status.ToLower().Contains("Rejected".ToLower()))
                        {
                            //Thông báo cho người dùng 
                            {
                                buyerNotification.UserEmail = user.Email;
                                buyerNotification.Content = $"Your report to {product.ProductName} post has been rejected.";
                            };
                        }
                        else if (value.Status.ToLower().Contains("Accepted".ToLower()))
                        {
                            //Thông báo cho người dùng 
                            {
                                buyerNotification.UserEmail = user.Email;
                                buyerNotification.Content = $"Your report to {product.ProductName} post has been accepted.";
                            };
                            product.Status = "Rejected";
                            await _productService.UpdateProductFromAPIAsync(product);

                            var seller = await _accountService.GetUserByIdFromAPIAsync(new Account() { Id = product.SellerId });
                            var sellerNotification = new CreateNotification();
                            sellerNotification.UserEmail = seller.Email;
                            sellerNotification.Content = $"Your {product.ProductName} post has been banned.";

                            await _notificationService.CreateNotificationFromApiAsync(sellerNotification);
                        }
                        await _notificationService.CreateNotificationFromApiAsync(buyerNotification);

                        request = await _service.GetRequestByIdFromAPIAsync((int)value.RequestId);
                        break;
                    }
            }


            request.Status = value.Status;
            request.Amount = value.Amount;
            request.RequestType = value.RequestType;
            request.PaymentId = value.PaymentId;
            request.UpdatedAt = DateTime.Now;
            request.Reason = value.Reason;

            await _service.UpdateRequestFromAPIAsync(request);

            return true;

        }
    }
}
