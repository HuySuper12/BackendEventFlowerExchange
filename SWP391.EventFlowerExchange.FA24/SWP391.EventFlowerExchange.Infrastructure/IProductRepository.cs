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
        public Task<List<GetProduct?>> GetEnableProductListAsync();

        public Task<List<GetProduct?>> GetDisableProductListAsync();

        public Task<List<GetProduct?>> GetInProgressProductListAsync();

        public Task<List<GetProduct?>> GetRejectedProductListAsync();

        public Task<bool> CreateNewProductAsync(CreateProduct product);

        public Task<bool> RemoveProductAsync(GetProduct product);

        public Task<List<GetProduct?>> SearchProductByPriceRangeAsync(decimal from, decimal to);

        public Task<GetProduct?> SearchProductByIdAsync(GetProduct product);

        public Task<List<GetProduct?>> SearchProductByNameAsync(string name);

        public Task<List<GetProduct?>> SearchProductByComboType_BatchesAsync();
        public Task<List<GetProduct?>> SearchProductByComboType_EventsAsync();

        public Task<List<GetProduct?>> SearchProductByCategory_WeddingAsync();
        public Task<List<GetProduct?>> SearchProductByCategory_ConferenceAsync();
        public Task<List<GetProduct?>> SearchProductByCategory_BirthdayAsync();

        public Task<List<GetProduct?>> GetLatestProductsAsync();

        public Task<List<GetProduct?>> GetOldestProductsAsync();

    }
}
