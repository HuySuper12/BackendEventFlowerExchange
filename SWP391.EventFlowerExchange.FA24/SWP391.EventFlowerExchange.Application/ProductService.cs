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
    public class ProductService : IProductService
    {
        private IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateNewProductFromAPIAsync(CreateProduct product, Account account)
        {

            return await _repo.CreateNewProductAsync(product, account);
        }

        public async Task<List<GetProduct?>> GetEnableProductListFromAPIAsync()
        {
            return await _repo.GetEnableProductListAsync();
        }

        public async Task<List<GetProduct?>> GetDisableProductListFromAPIAsync()
        {
            return await _repo.GetDisableProductListAsync();
        }

        public async Task<List<GetProduct?>> GetInProgressProductListFromAPIAsync()
        {
            return await _repo.GetInProgressProductListAsync();
        }

        public async Task<List<GetProduct?>> GetRejectedProductListFromAPIAsync()
        {
            return await _repo.GetRejectedProductListAsync();
        }

        public async Task<bool> RemoveProductFromAPIAsync(GetProduct product)
        {
            var checkProduct = await _repo.SearchProductByIdAsync(product);
            if (checkProduct != null)
            {
                return await _repo.RemoveProductAsync(product);
            }
            return false;
        }

        public async Task<GetProduct?> SearchProductByIdFromAPIAsync(GetProduct product)
        {
            return await _repo.SearchProductByIdAsync(product);
        }

        public async Task<List<GetProduct?>> SearchProductByNameFromAPIAsync(string name)
        {
            return await _repo.SearchProductByNameAsync(name);
        }

        public Task<List<GetProduct?>> GetLatestProductsFromAPIAsync()
        {
            return _repo.GetLatestProductsAsync();
        }

        public Task<List<GetProduct?>> GetOldestProductsFromAPIAsync()
        {
            return _repo.GetOldestProductsAsync();
        }

        //BỔ SUNG 
        public Task<List<GetProduct?>> GetEnableProductListBySellerEmailFromAPIAsync(Account value)
        {
            return _repo.GetEnableProductListBySellerEmailAsync(value);
        }

        public Task<List<GetProduct?>> GetDisableProductListBySellerEmailFromAPIAsync(Account value)
        {
            return _repo.GetDisableProductListBySellerEmailAsync(value);
        }

        public Task<List<GetProduct?>> GetInProgressProductListBySellerEmailFromAPIAsync(Account value)
        {
            return GetInProgressProductListBySellerEmailFromAPIAsync(value);
        }

        public Task<bool> UpdateProductFromAPIAsync(GetProduct product)
        {
            return _repo.UpdateProductAsync(product);
        }

        public Task<ProductStatistics> GetAllOrdersAndRatingBySellerFromAPIEmailAsync(Account account)
        {
            return _repo.GetAllOrdersAndRatingBySellerEmail(account);
        }

        public Task<List<GetProduct?>> GetExpiredProductListBySellerEmailFromAPIAsync(Account value)
        {
            return _repo.GetExpiredProductListBySellerEmailAsync(value);
        }

        public Task<List<GetProduct?>> GetBannedProductListBySellerEmailFromAPIAsync(Account value)
        {
            return _repo.GetBannedProductListBySellerEmailAsync(value);
        }
    }
}
