using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IProductService
    {
        public Task<List<Product?>> GetEnableProductListFromAPIAsync();

        public Task<List<Product?>> GetDisableProductListFromAPIAsync();

        public Task<List<Product?>> GetInProgressProductListFromAPIAsync();

        public Task<List<Product?>> GetRejectedProductListFromAPIAsync();

        public Task<bool> CreateNewProductFromAPIAsync(CreateProduct product);

        public Task<bool> RemoveProductFromAPIAsync(Product product);

        public Task<List<Product>> SearchProductByPriceRangeFromAPIAsync(decimal from, decimal to);

        public Task<Product> SearchProductByIdFromAPIAsync(Product product);

        public Task<List<Product?>> SearchProductByNameFromAPIAsync(string name);
    }
}
