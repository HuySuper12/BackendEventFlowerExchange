using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public class CartRepository : ICartRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;
        private IProductRepository _productRepository;

        public CartRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IdentityResult> CreateCartAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            // Tạo giỏ hàng mới
            var newCart = new Cart
            {
                BuyerId = account.Id,
            };

            await _context.Carts.AddAsync(newCart);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> CreateCartItemAsync(CartItem cartItem)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            // Nếu sản phẩm chưa có, thêm sản phẩm mới
            var newItem = new CartItem
            {
                CartId = cartItem.CartId,
                BuyerId = cartItem.BuyerId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                ProductImage = cartItem.ProductImage,
                CreatedAt = DateTime.Now
            };

            await _context.CartItems.AddAsync(newItem);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> RemoveItemFromCartAsync(CartItem cartItem)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<List<Cart>> ViewAllCartAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            return await _context.Carts.ToListAsync();
        }

        public async Task<List<GetCartItem>> ViewAllCartItemByUserIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var cart = await _context.CartItems.Where(x => x.BuyerId == account.Id).ToListAsync();
            if (cart != null)
            {
                List<GetCartItem> cartItems = new List<GetCartItem>();

                foreach (var item in cart)
                {
                    var product = _context.Products.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (product.Status == "Disable" || product.Status == "Expired")
                    {
                        await this.RemoveItemFromCartAsync(item);
                    }
                    else
                    {
                        var cartItem = new GetCartItem
                        {
                            CartId = item.CartId,
                            ProductId = item.ProductId,
                            BuyerId = item.BuyerId,
                            Quantity = item.Quantity,
                            Price = item.Price,
                            CreatedAt = item.CreatedAt,
                            ProductImage = item.ProductImage,
                            ComboType = product.ComboType,
                            ProductName = product.ProductName,

                        };

                        cartItems.Add(cartItem);
                    }
                }

                return cartItems;
            }

            return null;
        }

        public async Task<CartItem> ViewCartItemDetailAsync(CartItem cartItemDetail)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.ProductId == cartItemDetail.ProductId && x.BuyerId == cartItemDetail.BuyerId);
            return cartItem;
        }

        public async Task<bool> RemoveCartItemToCreateOrderAsync(CartItem cartItemDetail)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var productDetail = await _productRepository.SearchProductByIdAsync(new GetProduct() { ProductId = cartItemDetail.ProductId });
            var cartItem = await _context.CartItems.Where(x => x.ProductId == cartItemDetail.ProductId).ToListAsync();

            //Cap nhat status product = disable
            await _productRepository.RemoveProductAsync(productDetail);

            //Xoa san pham trong tat ca cart nguoi dung
            for (int i = 0; i < cartItem.Count; i++)
            {
                await RemoveItemFromCartAsync(cartItem[i]);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountCartItemByUserIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var cart = await _context.CartItems.Where(x => x.BuyerId == account.Id).ToListAsync();
            return cart.Count;
        }
    }
}
