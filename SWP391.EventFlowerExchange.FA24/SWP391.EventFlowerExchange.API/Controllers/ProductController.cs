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
        public ProductController(IProductService service)
        {
            _service = service;
        }


        [HttpGet("GetProductList/Enable")]
        [Authorize]
        public async Task<IActionResult> GetAllEnableProductList()
        {
            return Ok(await _service.GetEnableProductListFromAPIAsync());
        }


        [HttpGet("GetProductList/Disable")]
        [Authorize]
        public async Task<IActionResult> GetAllDisableProductList()
        {
            return Ok(await _service.GetDisableProductListFromAPIAsync());
        }


        [HttpGet("GetProductList/InProgress")]
        [Authorize(Roles = ApplicationRoles.Staff)]
        public async Task<IActionResult> GetAllInProgressProductList()
        {
            return Ok(await _service.GetInProgressProductListFromAPIAsync());
        }


        [HttpGet("GetProductList/Rejected")]
        [Authorize(Roles = ApplicationRoles.Seller + "," + ApplicationRoles.Staff)]
        public async Task<IActionResult> GetAllRejectedProductList()
        {
            return Ok(await _service.GetRejectedProductListFromAPIAsync());
        }


        
        [HttpGet("SearchProduct/{id:int}")]
        public async Task<IActionResult> SearchProductByID(int id)
        {
            Product product = new Product() { ProductId = id };
            var checkProduct = await _service.SearchProductByIdFromAPIAsync(product);
            if (checkProduct == null)
            {
                return BadRequest("Not found product");
            }
            return Ok(checkProduct);
        }
        [HttpGet("SearchProduct/{name}")]
        public async Task<IActionResult> SearchProductByName(string name)
        {
            var checkProduct = await _service.SearchProductByNameFromAPIAsync(name);
            if (checkProduct == null)
            {
                return BadRequest("Not found product");
            }
            return Ok(checkProduct);
        }
        [HttpGet("SearchProductByPriceRange/{from},{to}")]

        public async Task<IActionResult> SearchProductByPriceRange(decimal from, decimal to)
        {
            if (from < 0 || to < 0)
            {
                return BadRequest("minPrice or maxSalary must not be negative number");
            }

            if (from > to)
            {
                return BadRequest("minPrice must be less than or equal to maxPrice");
            }
            var checkProduct = await _service.SearchProductByPriceRangeFromAPIAsync(from, to);
            if (checkProduct == null)
            {
                return BadRequest("Not found product");
            }
            return Ok(checkProduct);
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<ActionResult<bool>> CreateNewProduct(CreateProduct product)
        {
            var check = await _service.CreateNewProductFromAPIAsync(product);
            if (check)
            {
                return true;
            }
            return false;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ApplicationRoles.Seller)]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = new Product() { ProductId = id };
            var checkProduct = await _service.SearchProductByIdFromAPIAsync(product);
            if (checkProduct == null)
            {
                return BadRequest("Not found product");
            }
            bool status = await _service.RemoveProductFromAPIAsync(checkProduct);
            return Ok(status);
        }

    }
}
