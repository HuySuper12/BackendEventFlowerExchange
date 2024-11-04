using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private IPaymentRepository _repo;
        private IAccountService _accountService;
        private IRequestService _requestService;


        public VnPayService(IConfiguration config, IPaymentRepository repo, IAccountService accountService, IRequestService requestService)
        {
            _config = config;
            _repo = repo;
            _accountService = accountService;
            _requestService = requestService;
        }

        public string CreatePaymentUrl(HttpContext context, VnPayRequest model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());

            vnpay.AddRequestData("vnp_CreateDate", model.CreateDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", $"{model.Email} - {model.Type} - {model.RequestId} - {model.UserId}");
            vnpay.AddRequestData("vnp_OrderType", "Quà tặng");
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<CreatePayment> PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var vnp_Amount = Convert.ToUInt64(vnpay.GetResponseData("vnp_Amount"));

            string[] parts = vnp_OrderInfo.Split(" - ");

            // Lấy email
            string email = parts[0];
            // Lấy type
            string type = parts[1];
            //Lấy requestId
            string numberString = parts[2]; // "1"

            //Lấy UserId
            string userId = parts[3];


            // Convert the number string to an integer
            int requestId = int.Parse(numberString);

            int paymentType = 0;

            if (type == "Deposit")
                paymentType = 1;
            else if (type == "Withdraw")
            {
                paymentType = 2;
                var acc = await _accountService.GetUserByIdFromAPIAsync(new Account() { Id = userId });
                email = acc.Email;
            }
            var user = await _accountService.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            var request = _requestService.GetRequestByIdFromAPIAsync(requestId);
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new CreatePayment
                {
                    Status = false,
                    ResponseCode = vnp_ResponseCode,
                    UserId = user.Id, //email
                    PaymentCode = vnp_orderId, //mã định danh
                    Amount = vnp_Amount / 100,
                    CreatedAt = DateTime.Now,
                    PaymentType = paymentType, //1: Nạp tiền, 2: Rút tiền
                    RequestId = requestId,
                };
            }

            return new CreatePayment
            {
                Status = true,
                ResponseCode = vnp_ResponseCode,
                PaymentCode = vnp_orderId, //mã định danh
                UserId = user.Id, //email
                Amount = vnp_Amount / 100,
                CreatedAt = DateTime.Now,
                PaymentType = paymentType, //1: Nạp tiền, 2: Rút tiền
                RequestId = requestId,
            };
        }

        public async Task<bool> CreatePaymentFromAPIAsync(CreatePayment newValue)
        {
            return await _repo.CreatePayementAsync(newValue);
        }

        public async Task<Payment> GetPayementByCodeFromAPIAsync(CreatePayment payment)
        {
            return await _repo.GetPayementByCodeAsync(payment);
        }

        public Task<List<Payment>> GetPayementByTypeAndEmailFromAPIAsync(int type, Account account)
        {
            return _repo.GetPayementByTypeAndEmailAsync(type, account);
        }

        public Task<List<Payment>> GetAllPaymentListFromAPIAsync(int type)
        {
            return _repo.GetAllPaymentListByType(type);
        }
    }
}
