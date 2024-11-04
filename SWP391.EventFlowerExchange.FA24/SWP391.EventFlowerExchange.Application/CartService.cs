using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
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
    public class CartService : ICartService
    {
        private ICartRepository _repo;
        private IAccountRepository _accountRepository;
        private IProductRepository _productRepository;

        public CartService(ICartRepository repo, IAccountRepository accountRepository, IProductRepository productRepository)
        {
            _repo = repo;
            _accountRepository = accountRepository;
            _productRepository = productRepository;
        }

        public async Task<IdentityResult> CreateCartFromApiAsync(Account account)
        {
            var allCarts = await _repo.ViewAllCartAsync();
            var cart = allCarts.FirstOrDefault(c => c.BuyerId == account.Id);//néu không nhập buyerid thì xet như thế nào?

            if (cart == null)
            {
                await _repo.CreateCartAsync(new Account() { Id = account.Id });
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> CreateCartItemFromApiAsync(CreateCartItem cartItem)
        {
            // Lấy tất cả giỏ hàng từ cơ sở dữ liệu.
            var allCarts = await _repo.ViewAllCartAsync();

            // Tìm thông tin người dùng dựa trên email đã cung cấp từ `cartItem.BuyerEmail`.
            var user = await _accountRepository.GetUserByEmailAsync(new Account() { Email = cartItem.BuyerEmail });

            // Tìm giỏ hàng tương ứng với người mua (dựa trên BuyerId từ tài khoản).
            var cart = allCarts.FirstOrDefault(c => c.BuyerId == user.Id);

            // Nếu giỏ hàng không tồn tại, tạo mới giỏ hàng cho người dùng.
            if (cart == null)
            {
                await _repo.CreateCartAsync(new Account() { Id = user.Id });
                allCarts = await _repo.ViewAllCartAsync();
                cart = allCarts.FirstOrDefault(c => c.BuyerId == user.Id);
            }

            // Lấy danh sách các sản phẩm đang trong trạng thái "Enable" và kiểm tra nếu sản phẩm tồn tại.
            var resultList = await _productRepository.GetEnableProductListAsync();
            var result = resultList.FirstOrDefault(c => c.ProductId == cartItem.ProductId);

            // Nếu sản phẩm tồn tại, thực hiện các hành động thêm sản phẩm vào giỏ hàng.
            if (result != null)
            {
                var itemList = await _repo.ViewAllCartItemByUserIdAsync(new Account() { Id = user.Id });
                var existingItem = itemList.FirstOrDefault(c => c.ProductId == result.ProductId);
                var productImage = await _productRepository.SearchProductImageByIdAsync(result);

                if (existingItem == null)
                {
                    var newItem = new CartItem
                    {
                        CartId = cart.CartId,
                        BuyerId = user.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = 1,
                        Price = result.Price,
                        ProductImage = productImage.LinkImage,
                        CreatedAt = DateTime.Now
                    };

                    await _repo.CreateCartItemAsync(newItem);

                    return IdentityResult.Success;
                }
            }

            // Trả về kết quả thất bại nếu sản phẩm không hợp lệ hoặc đã có lỗi xảy ra.
            return IdentityResult.Failed();
        }


        public async Task<IdentityResult> RemoveItemFromCartFromApiAsync(CartItem cartItem)
        {
            // Lấy toàn bộ danh sách giỏ hàng bằng phương thức bất đồng bộ ViewAllCartAsync
            var allCarts = await _repo.ViewAllCartAsync();

            // Tìm giỏ hàng tương ứng với BuyerId của CartItem truyền vào
            var cart = allCarts.FirstOrDefault(c => c.BuyerId == cartItem.BuyerId);

            // Nếu tìm thấy giỏ hàng của người mua
            if (cart != null)
            {
                // Lấy danh sách các sản phẩm trong giỏ hàng của người mua bằng phương thức ViewAllCartItemByUserIdAsync
                // Phương thức này truyền vào một đối tượng Account với Id là BuyerId
                var itemList = await _repo.ViewAllCartItemByUserIdAsync(new Account() { Id = cartItem.BuyerId });

                // Tìm sản phẩm cần xóa khỏi giỏ hàng dựa trên ProductId
                var itemToCheck = itemList.FirstOrDefault(c => c.ProductId == cartItem.ProductId);

                var itemToRemove = new CartItem()
                {
                    CartId = itemToCheck.CartId,
                    BuyerId = itemToCheck.BuyerId,
                    ProductId = itemToCheck.ProductId,
                    Quantity = itemToCheck.Quantity,
                    Price = itemToCheck.Price,
                    ProductImage = itemToCheck.ProductImage,
                    CreatedAt = itemToCheck.CreatedAt

                };

                // Nếu tìm thấy sản phẩm cần xóa trong giỏ hàng
                // Xóa sản phẩm khỏi giỏ hàng bằng phương thức bất đồng bộ RemoveItemFromCartAsync
                if (itemToRemove != null)
                {
                    await _repo.RemoveItemFromCartAsync(itemToRemove);


                    return IdentityResult.Success;
                }
            }

            // Nếu không tìm thấy giỏ hàng hoặc sản phẩm để xóa, trả về kết quả thất bại
            return IdentityResult.Failed();
        }


        public async Task<List<GetCartItem>> ViewAllCartItemByUserIdFromApiAsync(Account account)
        {
            return await _repo.ViewAllCartItemByUserIdAsync(account);
        }

        public async Task<int> GetCountCartItemByUserIdFromApiAsync(Account account)
        {
            return await _repo.GetCountCartItemByUserIdAsync(account);
        }
    }
}
