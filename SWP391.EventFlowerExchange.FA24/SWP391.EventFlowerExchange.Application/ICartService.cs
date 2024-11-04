using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface ICartService
    {
        public Task<List<GetCartItem>> ViewAllCartItemByUserIdFromApiAsync(Account account);
        public Task<IdentityResult> RemoveItemFromCartFromApiAsync(CartItem cartItem);
        public Task<IdentityResult> CreateCartItemFromApiAsync(CreateCartItem cartItem);
        public Task<IdentityResult> CreateCartFromApiAsync(Account account);
        public Task<int> GetCountCartItemByUserIdFromApiAsync(Account account);
    }
}
