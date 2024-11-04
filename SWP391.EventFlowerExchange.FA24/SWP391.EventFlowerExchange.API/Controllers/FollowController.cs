using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private IFollowService _service;
        private IAccountService _accountService;

        public FollowController(IFollowService service, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        [HttpGet("ViewFollowerByUserEmail")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<IActionResult> ViewFollowerByUserEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc); // ĐÃ SỬA
            if (check != null)
            {
                var result = await _service.GetFollowerListFromApiAsync(check);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }

        [HttpPost("CreateFollow")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> CreateFollow(CreateFollower follower)
        {
            Account acc = new Account();
            acc.Email = follower.FollowerEmail;// ĐÃ SỬA
            var check1 = await _accountService.GetUserByEmailFromAPIAsync(acc);// ĐÃ SỬA

            Account acc2 = new Account();
            acc2.Email = follower.SellerEmail;// ĐÃ SỬA
            var check2 = await _accountService.GetUserByEmailFromAPIAsync(acc2);// ĐÃ SỬA

            if (check1 != null && check2 != null)
            {
                //đổi lại để phù hợp với hàm add, email bây giờ sẽ chứa giá trị Id
                follower.FollowerEmail = check1.Id;
                follower.SellerEmail = check2.Id;

                var result = await _service.AddNewFollowerFromApiAsync(follower);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        [HttpDelete("RemoveFollower")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> RemoveAccount(string followerEmail, string sellerEmail)// ĐÃ SỬA
        {
            Account acc = new Account();
            acc.Email = followerEmail; // ĐÃ SỬA
            var check1 = await _accountService.GetUserByEmailFromAPIAsync(acc);// ĐÃ SỬA

            Account acc2 = new Account();
            acc2.Email = sellerEmail;// ĐÃ SỬA
            var check2 = await _accountService.GetUserByEmailFromAPIAsync(acc2);// ĐÃ SỬA

            if (check1 != null && check2 != null)
            {
                ShopNotification cartItem = new ShopNotification() { FollowerId = check1.Id, SellerId = check2.Id };// ĐÃ SỬA
                var result = await _service.RemoveFollowerFromApiAsync(cartItem);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        [HttpGet("GetCountFollowByUserEmail")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<IActionResult> GetCountFollowByUserEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc);
            if (check != null)
            {
                var result = await _service.GetCountFollowByUserEmailFromApiAsync(check);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }

        [HttpGet("CheckFollowByUserEmail")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<ActionResult<bool>> CheckFollowByUserEmail(string followerEmail, string sellerEmail)
        {
            Account acc = new Account();
            acc.Email = followerEmail;
            var check1 = await _accountService.GetUserByEmailFromAPIAsync(acc);

            Account acc2 = new Account();
            acc2.Email = sellerEmail;
            var check2 = await _accountService.GetUserByEmailFromAPIAsync(acc2);

            if (check1 != null && check2 != null)
            {
                var result = await _service.CheckFollowByUserEmailFromApiAsync(check1, check2);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
