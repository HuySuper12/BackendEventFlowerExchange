using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IProductRepository
    {
        public Task<List<Product?>> GetEnableProductListAsync();

        public Task<List<Product?>> GetDisableProductListAsync();

        public Task<List<Product?>> GetInProgressProductListAsync();

        public Task<List<Product?>> GetRejectedProductListAsync();

        public Task<bool> CreateNewProductAsync(CreateProduct product);

        public Task<bool> RemoveProductAsync(Product product);

        public Task<List<Product>> SearchProductByPriceRangeAsync(decimal from, decimal to);

        public Task<Product> SearchProductByIdAsync(Product product);

        public Task<List<Product?>> SearchProductByNameAsync(string name);
    }
}
