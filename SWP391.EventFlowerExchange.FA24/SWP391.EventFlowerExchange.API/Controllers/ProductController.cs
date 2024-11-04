using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _service;
        private IAccountService _account;
        private IConfiguration _config;
        private IRequestService _requestService;

        public ProductController(IProductService service, IConfiguration config, IAccountService account, IRequestService requestService)
        {
            _service = service;
            _account = account;
            _config = config;
            _requestService = requestService;
        }


        [HttpGet("GetProductList/Enable")]
        //[Authorize]
        public async Task<IActionResult> GetAllEnableProductList()
        {
            return Ok(await _service.GetEnableProductListFromAPIAsync());

        }


        [HttpGet("GetProductList/Disable")]
        //[Authorize]
        public async Task<IActionResult> GetAllDisableProductList()
        {
            return Ok(await _service.GetDisableProductListFromAPIAsync());
        }


        [HttpGet("GetProductList/InProgress")]
        //[Authorize(Roles = ApplicationRoles.Admin)]
        public async Task<IActionResult> GetAllInProgressProductList()
        {
            return Ok(await _service.GetInProgressProductListFromAPIAsync());
        }


        [HttpGet("GetProductList/Rejected")]
        //[Authorize(Roles = ApplicationRoles.Seller + "," + ApplicationRoles.Admin)]
        public async Task<IActionResult> GetAllRejectedProductList()
        {
            return Ok(await _service.GetRejectedProductListFromAPIAsync());
        }

        [HttpGet("GetProductList/Latest")]
        public async Task<IActionResult> GetLatestProductList()
        {
            var products = await _service.GetLatestProductsFromAPIAsync();
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("GetProductList/Oldest")]
        public async Task<IActionResult> GetOldestProductList()
        {
            var products = await _service.GetOldestProductsFromAPIAsync();
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }


        //BỔ SUNG API
        [HttpGet("GetProductList/EnableAndDisable")]
        //[Authorize]
        public async Task<IActionResult> GetEnableAndDisableProductList()
        {
            var disable = await _service.GetDisableProductListFromAPIAsync();
            var enable = await _service.GetEnableProductListFromAPIAsync();
            var combinedList = enable.Concat(disable).ToList();
            return Ok(combinedList);

        }

        [HttpGet("GetProductList/Enable/Seller")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<IActionResult> GetEnableProductListBySellerEmail(string email)
        {
            var acc = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            //var list = await _service.GetEnableProductListFromAPIAsync();
            //var filter = list.Where(p => p.SellerId.Contains(acc.Id)).ToList();
            var filter = await _service.GetEnableProductListBySellerEmailFromAPIAsync(acc);
            return Ok(filter);
        }


        [HttpGet("GetProductList/Disable/Seller")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<IActionResult> GetDisableProductListBySellerEmail(string email)
        {
            var acc = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            //var list = await _service.GetDisableProductListFromAPIAsync();
            //var filter = list.Where(p => p.SellerId.Contains(acc.Id)).ToList();

            var filter = await _service.GetDisableProductListBySellerEmailFromAPIAsync(acc);
            return Ok(filter);
        }


        [HttpGet("GetProductList/InProgress/Seller")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<IActionResult> GetInProgressProductListBySellerEmail(string email)
        {
            var acc = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            var list = await _service.GetInProgressProductListFromAPIAsync();
            var filter = list.Where(p => p.SellerId.Contains(acc.Id)).ToList();
            return Ok(filter);
        }


        [HttpGet("GetProductList/Rejected/Seller")]
        //[Authorize(Roles = ApplicationRoles.Seller + "," + ApplicationRoles.Admin)]
        public async Task<IActionResult> GetRejectedProductListBySellerEmail(string email)
        {
            var acc = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            var list = await _service.GetRejectedProductListFromAPIAsync();
            var filter = list.Where(p => p.SellerId.Contains(acc.Id)).ToList();
            return Ok(filter);
        }

        [HttpGet("GetProductList/Expired/Seller")]
        //[Authorize(Roles = ApplicationRoles.Seller + "," + ApplicationRoles.Admin)]
        public async Task<IActionResult> GetExpiredProductListBySellerEmail(string email)
        {
            var acc = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            return Ok(await _service.GetExpiredProductListBySellerEmailFromAPIAsync(acc));
        }

        [HttpGet("GetProductList/Deal/Seller")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<IActionResult> GetDealProductListBySellerEmail(string email)
        {
            var acc = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            var list = await _service.GetEnableProductListFromAPIAsync();
            var filter = list.Where(p => p.SellerId.Contains(acc.Id) && p.Price == 0).ToList();
            return Ok(filter);
        }

        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProductByID(int id)
        {
            GetProduct product = new GetProduct() { ProductId = id };
            var checkProduct = await _service.SearchProductByIdFromAPIAsync(product);
            if (checkProduct == null)
            {
                return NotFound();
            }
            return Ok(checkProduct);
        }

        [HttpGet("SearchProduct/{name}")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            var checkProduct = await _service.SearchProductByNameFromAPIAsync(name);
            if (checkProduct == null)
            {
                return NotFound();
            }
            return Ok(checkProduct);
        }


        [HttpGet("GetOrdersAndRatingBySellerEmail")]
        //[Authorize(Roles = ApplicationRoles.Seller)]

        public async Task<IActionResult> GetOrdersAndRatingBySellerEmail(string email)
        {
            var acc = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = email });
            return Ok(await _service.GetAllOrdersAndRatingBySellerFromAPIEmailAsync(acc));
        }

        [HttpPost("CreateProduct")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<ActionResult<bool>> CreateNewProduct(CreateProduct product)
        {
            var account = await _account.GetUserByEmailFromAPIAsync(new Account() { Email = product.SellerEmail });
            return await _service.CreateNewProductFromAPIAsync(product, account);

        }

        [HttpPut("UpdateProduct/{id}, {status}")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<ActionResult<bool>> UpdateProduct(int id, string status)
        {
            var product = await _service.SearchProductByIdFromAPIAsync(new GetProduct() { ProductId = id });
            product.Status = status;
            return await _service.UpdateProductFromAPIAsync(product);

        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = ApplicationRoles.Seller)]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            GetProduct product = new GetProduct() { ProductId = id };
            var checkProduct = await _service.SearchProductByIdFromAPIAsync(product);
            if (checkProduct == null)
            {
                return BadRequest();
            }
            bool status = await _service.RemoveProductFromAPIAsync(checkProduct);
            return Ok(status);
        }

    }
}
