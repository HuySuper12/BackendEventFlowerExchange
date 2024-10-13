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


        private GetProduct ConvertProductToGetProduct(Product value)
        {
            var newValue = new GetProduct()
            {
                ProductName = value.ProductName,
                Category = value.Category,
                ComboType = value.ComboType,
                CreatedAt = value.CreatedAt,
                Description = value.Description,
                FreshnessDuration = value.FreshnessDuration,
                Price = value.Price,
                Quantity = value.Quantity,
                SellerId = value.SellerId,
                ProductId = value.ProductId,
                Status = value.Status,
            };

            //Lấy list đối tượng có chứ hình ảnh của Id product và sau đó lấy list  
            var productImageList = _context.ImageProducts.Where(x => x.ProductId == newValue.ProductId).ToList().Select(x => x.LinkImage).ToList();

            newValue.ProductImage = productImageList;

            return newValue;
        }

        public async Task<List<GetProduct?>> GetEnableProductListAsync()
        {
            string status = "Enable";
            _context = new Swp391eventFlowerExchangePlatformContext();
            var productList = await _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }

            return getProductList;
        }

        public async Task<List<GetProduct?>> GetDisableProductListAsync()
        {
            string status = "Disable";
            _context = new Swp391eventFlowerExchangePlatformContext();
            var productList = await _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> GetInProgressProductListAsync()
        {
            //_context = new Swp391eventFlowerExchangePlatformContext();
            //return await _context.Products.Where(p => p.Status == null).ToListAsync();

            _context = new Swp391eventFlowerExchangePlatformContext();
            var productList = await _context.Products.Where(p => p.Status == null).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> GetRejectedProductListAsync()
        {
            string status = "Rejected";
            _context = new Swp391eventFlowerExchangePlatformContext();
            var productList = await _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
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
                SellerId = product.SellerId,
                Description = product.Description,
                Category = product.Category
            };
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            for (int i = 0; i < product.ListImage.Count; i++)
            {
                ImageProduct newValue = new ImageProduct()
                {
                    ProductId = newProduct.ProductId,
                    LinkImage = product.ListImage[i]
                };
                _context.ImageProducts.Add(newValue);

            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductAsync(GetProduct product)
        {
            Product newProduct = new Product()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                FreshnessDuration = product.FreshnessDuration,
                Price = product.Price,
                ComboType = product.ComboType,
                CreatedAt = product.CreatedAt,
                Quantity = product.Quantity,
                SellerId = product.SellerId,
                Description = product.Description,
                Category = product.Category
            };
            newProduct.Status = "Disable";
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Products.Update(newProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GetProduct?> SearchProductByIdAsync(GetProduct product)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var checkProduct = await _context.Products.FindAsync(product.ProductId);
            var newValue = ConvertProductToGetProduct(checkProduct);


            return newValue;
        }

        public async Task<List<GetProduct?>> SearchProductByNameAsync(string name)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string status = "Enable";
            var productList = await _context.Products.Where(p => p.ProductName.ToLower().Contains(name.ToLower()) && p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> SearchProductByPriceRangeAsync(decimal from, decimal to)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var productList = await _context.Products.Where(p => p.Price >= from && p.Price <= to).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> SearchProductByComboType_BatchesAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string comboType = "Batches";
            string status = "Enable";
            var productList = await _context.Products.Where(p => p.ComboType.ToLower().Contains(comboType.ToLower()) && p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> SearchProductByComboType_EventsAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string comboType = "Events";
            string status = "Enable";

            var productList = await _context.Products.Where(p => p.ComboType.ToLower().Contains(comboType.ToLower()) && p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> SearchProductByCategory_WeddingAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string category = "Wedding";
            string status = "Enable";

            var productList = await _context.Products.Where(p => p.Category.ToLower().Contains(category.ToLower()) && p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> SearchProductByCategory_ConferenceAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string category = "Conference";
            string status = "Enable";

            var productList = await _context.Products.Where(p => p.Category.ToLower().Contains(category.ToLower()) && p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> SearchProductByCategory_BirthdayAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string category = "Birthday";
            string status = "Enable";

            var productList = await _context.Products.Where(p =>
                p.Category.ToLower().Contains(category.ToLower())
                && p.Status != null
                && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> GetLatestProductsAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string status = "Enable";
            var productList = await _context.Products
                .Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower()))
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync();

            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

        public async Task<List<GetProduct?>> GetOldestProductsAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            string status = "Enable";
            var productList = await _context.Products
                .Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower()))
                .OrderBy(p => p.CreatedAt)
                .Take(8)
                .ToListAsync();

            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                getProductList.Add(newValue);
            }
            return getProductList;
        }

    }
}
