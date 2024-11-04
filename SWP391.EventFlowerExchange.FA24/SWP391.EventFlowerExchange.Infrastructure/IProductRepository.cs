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

        public Task<bool> CreateNewProductAsync(CreateProduct product, Account account);

        public Task<bool> RemoveProductAsync(GetProduct product);

        public Task<bool> UpdateProductAsync(GetProduct product);


        public Task<GetProduct?> SearchProductByIdAsync(GetProduct product);

        public Task<List<GetProduct?>> SearchProductByNameAsync(string name);

        public Task<List<GetProduct?>> GetLatestProductsAsync();

        public Task<List<GetProduct?>> GetOldestProductsAsync();



        //BỔ SUNG
        public Task<List<GetProduct?>> GetEnableProductListBySellerEmailAsync(Account value);

        public Task<List<GetProduct?>> GetDisableProductListBySellerEmailAsync(Account value);

        public Task<List<GetProduct?>> GetExpiredProductListBySellerEmailAsync(Account value);

        public Task<ImageProduct> SearchProductImageByIdAsync(GetProduct product);

        public Task<ProductStatistics> GetAllOrdersAndRatingBySellerEmail(Account account);

    }
}
