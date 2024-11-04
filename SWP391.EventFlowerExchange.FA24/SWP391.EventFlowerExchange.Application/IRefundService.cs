using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IRefundService
    {
        public Task<IdentityResult> BuyerReturnActionFromApiAsync(Account buyerEmail, Order orderId);
        public Task<IdentityResult> UpdateRefundRequestStatusFromApiAsync(Account staffEmail, Order orderId);
        public Task<IdentityResult> UpdateSellerRefundConfirmationStatusFromApiAsync(Order orderId);
    }
}
