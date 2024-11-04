using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPAYController : ControllerBase
    {

        private IVnPayService _vnPayservice;
        private IAccountService _accountService;
        private IRequestService _requestService;
        private INotificationService _notification;


        public VNPAYController(IVnPayService vnPayservice, IAccountService account, IRequestService requestService, INotificationService notificationService)
        {
            _vnPayservice = vnPayservice;
            _accountService = account;
            _requestService = requestService;
            _notification = notificationService;
        }

        [EnableCors("MyCors")]
        [HttpPost("create-payment-link")]
        public IActionResult CreatePaymentLink(VnPayRequest model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid payment request.");
            }
            string paymentURL = _vnPayservice.CreatePaymentUrl(HttpContext, model);
            return Ok(paymentURL);
        }

        [HttpGet("GetPaymentListBy/{type}")]
        public async Task<IActionResult> GetPaymentListByType(int type)
        {
            return Ok(await _vnPayservice.GetAllPaymentListFromAPIAsync(type));
        }

        [HttpGet("GetPaymentListBy")]
        public async Task<IActionResult> GetPayementByTypeAndEmail(int type, string email)
        {
            var account = await _accountService.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            return Ok(await _vnPayservice.GetPayementByTypeAndEmailFromAPIAsync(type, account));
        }

        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = await _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.ResponseCode != "00" || response.Status != true)
            {
                response.Status = false;
                await _vnPayservice.CreatePaymentFromAPIAsync(response);
                return Redirect("https://event-flower-exchange.vercel.app/");
            }
            var account = await _accountService.GetUserByIdFromAPIAsync(new Account() { Id = response.UserId });

            if (response.PaymentType == 1) //NAP TIEN
            {
                account.Balance = (response.Amount) + account.Balance;
                response.PaymentContent = $"Deposit money into {account.Name}'s wallet";
                response.PaymentType = 1; //1: Nạp tiền, 2: Rút tiền

                // Here you can save the order to the database if needed
                await _vnPayservice.CreatePaymentFromAPIAsync(response);
                await _accountService.UpdateAccountFromAPIAsync(account);
            }
            else //RUT TIEN
            {
                response.PaymentContent = $"Withdraw money from {account.Name}'s wallet";
                var check = await _vnPayservice.CreatePaymentFromAPIAsync(response);
                if (check)
                {
                    var payment = await _vnPayservice.GetPayementByCodeFromAPIAsync(response);
                    var request = await _requestService.GetRequestByIdFromAPIAsync((int)response.RequestId);
                    request.Status = "Accepted";
                    request.PaymentId = payment.PaymentId;
                    request.UpdatedAt = response.CreatedAt;
                    await _requestService.UpdateRequestFromAPIAsync(request);

                    account.Balance = account.Balance - (response.Amount);
                    await _accountService.UpdateAccountFromAPIAsync(account);

                    var withdrawalNotification = new CreateNotification()
                    {
                        UserEmail = account.Email,
                        Content = $"Your withdrawal request has been accepted."
                    };
                    await _notification.CreateNotificationFromApiAsync(withdrawalNotification);
                }
            }
            return Redirect("https://anime47.tv/xem-phim-kekkon-yubiwa-monogatari-ep-02/204546.html");
        }

    }
}
