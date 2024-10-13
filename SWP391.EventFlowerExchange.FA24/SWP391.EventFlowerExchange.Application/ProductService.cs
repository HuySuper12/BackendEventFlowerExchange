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

        public async Task<List<GetProduct?>> SearchProductByPriceRangeFromAPIAsync(decimal from, decimal to)
        {

            return await _repo.SearchProductByPriceRangeAsync(from, to);
        }

        public async Task<List<GetProduct?>> SearchProductByComboType_EventsFromAPIAsync()
        {
            return await _repo.SearchProductByComboType_EventsAsync();
        }

        public async Task<List<GetProduct?>> SearchProductByComboType_BatchesFromAPIAsync()
        {
            return await _repo.SearchProductByComboType_BatchesAsync();
        }

        public async Task<List<GetProduct?>> SearchProductByCategory_WeddingFromAPIAsync()
        {
            return await _repo.SearchProductByCategory_WeddingAsync();

        }

        public async Task<List<GetProduct?>> SearchProductByCategory_ConferenceFromAPIAsync()
        {
            return await _repo.SearchProductByCategory_ConferenceAsync();
        }

        public async Task<List<GetProduct?>> SearchProductByCategory_BirthdayFromAPIAsync()
        {
            return await _repo.SearchProductByCategory_BirthdayAsync();
        }

        public Task<List<GetProduct?>> GetLatestProductsFromAPIAsync()
        {
            return _repo.GetLatestProductsAsync();
        }

        public Task<List<GetProduct?>> GetOldestProductsFromAPIAsync()
        {
            return _repo.GetOldestProductsAsync();
        }
    }
}
