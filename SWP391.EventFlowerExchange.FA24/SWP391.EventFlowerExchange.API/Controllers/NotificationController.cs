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

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("ViewAllNotification")]
        //[Authorize(Roles = ApplicationRoles.Manager)]
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

        [HttpGet("ViewNotificationByUserId/{id}")]
        //[Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<IActionResult> ViewNotificationByUserId(string id)
        {
            Account acc = new Account();
            acc.Id = id;

            var result = await _service.ViewAllNotificationByUserIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpGet("ViewNotificationById/{id}")]
        //[Authorize(Roles = ApplicationRoles.Manager)]
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
        //[Authorize(Roles = ApplicationRoles.Manager)]
        public async Task<ActionResult<bool>> CreateNotification(CreateNotification createNotificationDto)
        {
            try
            {
                var result = await _service.CreateNotificationFromApiAsync(createNotificationDto);
                if (result != null)
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
    }
}
