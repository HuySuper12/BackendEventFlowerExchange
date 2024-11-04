using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IRatingService _service;
        private IAccountService _accountService;
        private IOrderService _orderService;

        public RatingController(IRatingService service, IAccountService accountService, IOrderService orderService)
        {
            _service = service;
            _accountService = accountService;
            _orderService = orderService;
        }

        [HttpGet("ViewRatingByUserEmail")]
        //[Authorize]
        public async Task<IActionResult> ViewRatingByUserEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc);     // ĐÃ SỬA 
            if (check != null)
            {
                var result = await _service.ViewAllRatingByUserIdFromApiAsync(check);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }

        [HttpGet("ViewRatingByProductId")]
        public async Task<IActionResult> ViewRatingByProductId(int productId)
        {
            var order = await _orderService.SearchOrderItemByProductIdFromAPIAsync(new GetProduct() { ProductId = productId });

            if (order != null)
            {
                var check = await _service.ViewRatingByOrderIdFromAPIAsync(new Order() { OrderId = order.OrderId });     // ĐÃ SỬA 
                if (check != null)
                {
                    return Ok(check);

                }
            }
            return Ok("Not found!");
        }

        [HttpPost("PostRating")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> PostRating(CreateRating rate)  //KÊU QUÝ MINH TRUYỀN EMAIL VÀO BUYER ID. ĐỂ KHỎI PHẢI SỬA TÊN CreateRating Ở TRONG REPOSITORY
        {
            Account acc = new Account();
            acc.Email = rate.BuyerEmail;
            var deleteAccount = await _accountService.GetUserByEmailFromAPIAsync(acc);

            if (deleteAccount != null)
            {
                var result = await _service.PostRatingFromApiAsync(rate);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }

            return false;
        }
    }
}
