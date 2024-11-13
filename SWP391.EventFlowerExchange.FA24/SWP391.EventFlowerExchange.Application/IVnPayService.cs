using Microsoft.AspNetCore.Http;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IVnPayService
    {
        public string CreatePaymentUrl(HttpContext context, VnPayRequest model);
        public Task<CreatePayment> PaymentExecute(IQueryCollection collections);

        public Task<bool> CreatePaymentFromAPIAsync(CreatePayment newValue);

        public Task<Payment> GetPayementByCodeFromAPIAsync(CreatePayment payment);

        public Task<List<Payment>> GetAllPaymentListFromAPIAsync(int type);

        public Task<List<Payment>> GetPayementByTypeAndEmailFromAPIAsync(int type, Account account);

        public Task<bool> PaymentSalaryFromAPIAsync();
    }
}
