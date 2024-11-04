using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IRefundRepository
    {
        public Task<IdentityResult> BuyerReturnActionAsync(Account buyer, Order order);
        public Task<IdentityResult> UpdateRefundRequestStatusAsync(Account staff, Order order);
        public Task<IdentityResult> UpdateSellerRefundConfirmationStatusAsync(Order order);
    }
}
