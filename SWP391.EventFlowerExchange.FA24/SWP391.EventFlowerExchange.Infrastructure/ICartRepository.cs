using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface ICartRepository
    {
        public Task<List<GetCartItem>> ViewAllCartItemByUserIdAsync(Account account);
        public Task<IdentityResult> CreateCartAsync(Account account);
        public Task<IdentityResult> CreateCartItemAsync(CartItem cartItem);
        public Task<List<Cart>> ViewAllCartAsync();
        public Task<IdentityResult> RemoveItemFromCartAsync(CartItem cartItem);
        public Task<CartItem> ViewCartItemDetailAsync(CartItem cartItemDetail);
        public Task<bool> RemoveCartItemToCreateOrderAsync(CartItem cartItemDetail);
        public Task<int> GetCountCartItemByUserIdAsync(Account account);
    }
}
