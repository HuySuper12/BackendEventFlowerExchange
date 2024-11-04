using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class RefundService : IRefundService
    {
        private IRefundRepository _repo;

        public RefundService(IRefundRepository repo)
        {
            _repo = repo;
        }

        public async Task<IdentityResult> BuyerReturnActionFromApiAsync(Account buyerEmail, Order orderId)
        {
            return await _repo.BuyerReturnActionAsync(buyerEmail, orderId);
        }

        public async Task<IdentityResult> UpdateRefundRequestStatusFromApiAsync(Account staffEmail, Order orderId)
        {
            return await _repo.UpdateRefundRequestStatusAsync(staffEmail, orderId);
        }

        public async Task<IdentityResult> UpdateSellerRefundConfirmationStatusFromApiAsync(Order orderId)
        {
            return await _repo.UpdateSellerRefundConfirmationStatusAsync(orderId);
        }
    }
}
