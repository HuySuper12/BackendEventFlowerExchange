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
        public Task<List<GetProduct?>> GetEnableProductListFromAPIAsync();

        public Task<List<GetProduct?>> GetDisableProductListFromAPIAsync();

        public Task<List<GetProduct?>> GetInProgressProductListFromAPIAsync();

        public Task<List<GetProduct?>> GetRejectedProductListFromAPIAsync();

        public Task<bool> CreateNewProductFromAPIAsync(CreateProduct product);

        public Task<bool> RemoveProductFromAPIAsync(GetProduct product);

        public Task<List<GetProduct?>> SearchProductByPriceRangeFromAPIAsync(decimal from, decimal to);

        public Task<GetProduct?> SearchProductByIdFromAPIAsync(GetProduct product);

        public Task<List<GetProduct?>> SearchProductByNameFromAPIAsync(string name);

        public Task<List<GetProduct?>> SearchProductByComboType_EventsFromAPIAsync();

        public Task<List<GetProduct?>> SearchProductByComboType_BatchesFromAPIAsync();

        public Task<List<GetProduct?>> SearchProductByCategory_WeddingFromAPIAsync();

        public Task<List<GetProduct?>> SearchProductByCategory_ConferenceFromAPIAsync();

        public Task<List<GetProduct?>> SearchProductByCategory_BirthdayFromAPIAsync();

        public Task<List<GetProduct?>> GetLatestProductsFromAPIAsync();

        public Task<List<GetProduct?>> GetOldestProductsFromAPIAsync();

    }
}
