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

        public async Task<bool> CreateNewProductFromAPIAsync(CreateProduct product)
        {
            return await _repo.CreateNewProductAsync(product);
        }

        public async Task<List<Product?>> GetEnableProductListFromAPIAsync()
        {
            return await _repo.GetEnableProductListAsync();
        }

        public async Task<List<Product?>> GetDisableProductListFromAPIAsync()
        {
            return await _repo.GetDisableProductListAsync();
        }

        public async Task<List<Product?>> GetInProgressProductListFromAPIAsync()
        {
            return await _repo.GetInProgressProductListAsync();
        }

        public async Task<List<Product?>> GetRejectedProductListFromAPIAsync()
        {
            return await _repo.GetRejectedProductListAsync();
        }

        public async Task<bool> RemoveProductFromAPIAsync(Product product)
        {
            return await _repo.RemoveProductAsync(product);
        }

        public async Task<Product> SearchProductByIdFromAPIAsync(Product product)
        {
            return await _repo.SearchProductByIdAsync(product);
        }

        public async Task<List<Product?>> SearchProductByNameFromAPIAsync(string name)
        {
            return await _repo.SearchProductByNameAsync(name);
        }

        public async Task<List<Product>> SearchProductByPriceRangeFromAPIAsync(decimal from, decimal to)
        {

            return await _repo.SearchProductByPriceRangeAsync(from, to);
        }
    }
}
