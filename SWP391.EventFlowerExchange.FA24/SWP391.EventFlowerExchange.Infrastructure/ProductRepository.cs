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
    public class ProductRepository : IProductRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;

        public async Task<List<Product>> GetEnableProductListAsync()
        {
            string status = "Enable";
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
        }

        public async Task<List<Product>> GetDisableProductListAsync()
        {
            string status = "Disable";
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
        }

        public async Task<List<Product>> GetInProgressProductListAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Products.Where(p => p.Status == null).ToListAsync();
        }

        public async Task<List<Product>> GetRejectedProductListAsync()
        {
            string status = "Rejected";
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
        }

        public async Task<bool> CreateNewProductAsync(CreateProduct product)
        {
            Product newProduct = new Product()
            {
                ProductName = product.ProductName,
                FreshnessDuration = product.FreshnessDuration,
                Price = product.Price,
                ComboType = product.ComboType,
                CreatedAt = product.CreatedAt,
                Quantity = product.Quantity,
                SellerId = product.SellerId
            };
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductAsync(Product product)
        {
            product.Status = "Disable";
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> SearchProductByIdAsync(Product product)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Products.FindAsync(product.ProductId);
        }

        public async Task<List<Product>> SearchProductByNameAsync(string name)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var products = await _context.Products.Where(p => p.ProductName.ToLower().Contains(name.ToLower())).ToListAsync();
            return products;
        }

        public async Task<List<Product>> SearchProductByPriceRangeAsync(decimal from, decimal to)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var products = await _context.Products.Where(p => p.Price >= from && p.Price <= to).ToListAsync();
            return products;
        }
    }
}
