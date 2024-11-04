using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationService _service;
        private IAccountService _accountService;

        public NotificationController(INotificationService service, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        [HttpGet("ViewAllNotification")]
        //[Authorize(Roles = ApplicationRoles.Manager + " , " + ApplicationRoles.Staff )]
        public async Task<IActionResult> ViewAllNotification()
        {
            try
            {
                return Ok(await _service.ViewAllNotificationFromApiAsync());
            }
            catch
            {
                return Ok("Not found!");
            }
        }

        [HttpGet("ViewNotificationByUserEmail/{email}")]
        //[Authorize(Roles = ApplicationRoles.Seller + " , " + ApplicationRoles.Buyer)]
        public async Task<IActionResult> ViewNotificationByUserEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc); // ĐÃ SỬA
            if (check != null)
            {
                var result = await _service.ViewAllNotificationByUserIdFromApiAsync(check);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }

        [HttpGet("ViewNotificationById/{id}")]
        //[Authorize(Roles = ApplicationRoles.Staff + " , " + ApplicationRoles.Manager)]
        public async Task<IActionResult> ViewNotificationById(int id)
        {
            Notification acc = new Notification();
            acc.NotificationId = id;

            var result = await _service.ViewNotificationByIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpPost("CreateNotification")]
        //[Authorize]
        public async Task<ActionResult<bool>> CreateNotification(CreateNotification createNotificationDto)
        {
            try
            {
                var result = await _service.CreateNotificationFromApiAsync(createNotificationDto);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet("ViewAllShopNotification")]
        //[Authorize(Roles = ApplicationRoles.Manager + " , " + ApplicationRoles.Staff)]
        public async Task<IActionResult> ViewAllShopNotification()
        {
            try
            {
                return Ok(await _service.ViewAllShopNotificationFromApiAsync());
            }
            catch
            {
                return Ok("Not found!");
            }
        }

        [HttpGet("ViewShopNotificationByUserEmail/{email}")]
        //[Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<IActionResult> ViewShopNotificationByUserEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc); // ĐÃ SỬA
            if (check != null)
            {
                var result = await _service.ViewAllShopNotificationByUserIdFromApiAsync(check);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }

        [HttpGet("ViewShopNotificationById/{id}")]
        //[Authorize(Roles = ApplicationRoles.Staff + " , " + ApplicationRoles.Manager)]
        public async Task<IActionResult> ViewShopNotificationById(int id)
        {
            ShopNotification acc = new ShopNotification();
            acc.ShopNotificationId = id;

            var result = await _service.ViewShopNotificationByIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpPost("CreateShopNotification")]
        //[Authorize]
        public async Task<ActionResult<bool>> CreateShopNotification(CreateShopNotification createNotificationDto)
        {
            try
            {
                var result = await _service.CreateShopNotificationFromApiAsync(createNotificationDto);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet("CountNotificationsByEmail")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<IActionResult> CountNotificationsByEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc);
            if (check != null)
            {
                var result = await _service.CountNotificationsByEmailFromApiAsync(check);
                if (result != 0)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }

        [HttpGet("CountShopNotificationsByEmail")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<IActionResult> CountShopNotificationsByEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc);
            if (check != null)
            {
                var result = await _service.CountShopNotificationByEmailFromApiAsync(check);
                if (result != 0)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }
    }
}
