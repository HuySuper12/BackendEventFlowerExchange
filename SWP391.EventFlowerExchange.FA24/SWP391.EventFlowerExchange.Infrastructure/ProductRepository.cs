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
        private IRequestRepository _request;
        private IFollowRepository _follow;
        private INotificationRepository _notification;


        public ProductRepository(IRequestRepository request, IFollowRepository follow, INotificationRepository notification)
        {
            _request = request;
            _follow = follow;
            _notification = notification;
        }

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
                ExpireddAt = value.ExpiredAt,
            };
            //Lấy list đối tượng có chứ hình ảnh của Id product và sau đó lấy list  
            var productImageList = _context.ImageProducts.Where(x => x.ProductId == newValue.ProductId).ToList().Select(x => x.LinkImage).ToList();

            newValue.ProductImage = productImageList;

            return newValue;
        }

        private Product ConvertGetProductToProduct(GetProduct value)
        {
            var newValue = new Product()
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
                ExpiredAt = value.ExpireddAt,
            };
            return newValue;
        }

        private void CheckExpiredDateProduct()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var list = _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains("Enable")).ToList();
            for (var i = 0; i < list.Count; i++)
            {
                if (DateTime.Now > list[i].ExpiredAt)
                {
                    list[i].Status = "Expired";
                    _context.Products.Update(list[i]);
                    _context.SaveChanges();
                }
            }
        }

        public async Task<List<GetProduct?>> GetEnableProductListAsync()
        {
            CheckExpiredDateProduct();
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
            string status = "Pending";
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


        public async Task<bool> CreateNewProductAsync(CreateProduct product, Account account)
        {
            int? check;

            if (product.FreshnessDuration == 1)
                check = 1;
            else
                check = product.FreshnessDuration - 1;

            Product newProduct = new Product()
            {
                ProductName = product.ProductName,
                FreshnessDuration = product.FreshnessDuration,
                Price = product.Price,
                ComboType = product.ComboType,
                CreatedAt = DateTime.Now,
                Quantity = product.Quantity,
                SellerId = account.Id,
                Description = product.Description,
                Category = product.Category,
                ExpiredAt = DateTime.Now.AddDays((int)check),
                Status = "Pending"
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
            Request request = new Request()
            {
                ProductId = newProduct.ProductId,
                CreatedAt = newProduct.CreatedAt,
                Status = "Pending",
                RequestType = "Post",
                UserId = account.Id,
            };
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductAsync(GetProduct product)
        {
            Product newProduct = ConvertGetProductToProduct(product);
            newProduct.Status = "Disable";
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Products.Update(newProduct);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<GetProduct?> SearchProductByIdAsync(GetProduct product)
        {
            CheckExpiredDateProduct();
            _context = new Swp391eventFlowerExchangePlatformContext();
            var checkProduct = await _context.Products.FindAsync(product.ProductId);
            var newValue = ConvertProductToGetProduct(checkProduct);


            return newValue;
        }


        public async Task<List<GetProduct?>> SearchProductByNameAsync(string name)
        {
            CheckExpiredDateProduct();
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

        public async Task<List<GetProduct?>> GetLatestProductsAsync()
        {
            CheckExpiredDateProduct();
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
            CheckExpiredDateProduct();
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

        //BỔ SUNG THÊM HÀM

        public async Task<List<GetProduct?>> GetEnableProductListBySellerEmailAsync(Account value)
        {
            var list = await GetEnableProductListAsync();
            var filter = list.Where(p => p.SellerId == value.Id).ToList();
            return filter;
        }

        public async Task<List<GetProduct?>> GetDisableProductListBySellerEmailAsync(Account value)
        {
            var list = await GetDisableProductListAsync();
            var filter = list.Where(p => p.SellerId == value.Id).ToList();
            return filter;
        }

        public async Task<List<GetProduct?>> GetExpiredProductListBySellerEmailAsync(Account value)
        {
            string status = "Expired";
            _context = new Swp391eventFlowerExchangePlatformContext();
            var productList = await _context.Products.Where(p => p.Status != null && p.Status.ToLower().Contains(status.ToLower())).ToListAsync();
            var getProductList = new List<GetProduct?>();
            foreach (var product in productList)
            {
                var newValue = ConvertProductToGetProduct(product);
                if (newValue.SellerId == value.Id)
                {
                    getProductList.Add(newValue);
                }
            }
            return getProductList;
        }

        public async Task<bool> UpdateProductAsync(GetProduct product)
        {
            Product newProduct = ConvertGetProductToProduct(product);
            newProduct.Status = product.Status;
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Products.Update(newProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ImageProduct> SearchProductImageByIdAsync(GetProduct product)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var productImage = await _context.ImageProducts.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            return productImage;
        }


        public async Task<ProductStatistics> GetAllOrdersAndRatingBySellerEmail(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            ProductStatistics st = new ProductStatistics();
            // Lấy danh sách tất cả các orders theo sellerId
            var orderList = await _context.Orders
                                          .Where(x => x.SellerId == account.Id && x.Status == "Success")
                                          .ToListAsync();
            var enableList = await GetEnableProductListBySellerEmailAsync(account);
            var soldOutProduct = await GetDisableProductListBySellerEmailAsync(account);

            // Kiểm tra nếu không có đơn hàng nào
            if (!orderList.Any())
            {
                st.Order = 0;
                st.Rating = 0; // Không có đánh giá nếu không có đơn hàng
                st.EnableProducts = enableList.Count > 0 ? enableList.Count : 0;
                st.SoldOut = soldOutProduct.Count > 0 ? soldOutProduct.Count : 0;
            }
            else
            {
                var orderIds = orderList.Select(o => o.OrderId).ToList();
                var reviews = await _context.Reviews
                                            .Join(orderIds,
                                                  review => review.OrderId,
                                                  orderId => orderId,
                                                  (review, orderId) => review)
                                            .ToListAsync();
                // Tính tổng số rating từ tất cả các reviews (kiểm tra nếu null)
                int sumRating = reviews.Where(r => r.Rating.HasValue)
                                       .Sum(r => r.Rating.Value);

                // Tạo đối tượng Statistics

                st.Order = orderList.Count;
                st.Rating = (reviews.Count > 0) ? (double)sumRating / reviews.Count : 0; // Kiểm tra số lượng review trước khi chia
                st.EnableProducts = enableList.Count > 0 ? enableList.Count : 0;
                st.SoldOut = soldOutProduct.Count > 0 ? soldOutProduct.Count : 0;
                st.AllProduct = st.EnableProducts + st.SoldOut;
            }
            return st;
        }
    }
}
