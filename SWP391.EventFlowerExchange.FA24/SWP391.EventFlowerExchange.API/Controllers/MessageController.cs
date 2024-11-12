using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IMessageService _service;
        private IAccountService _accountService;

        public MessageController(IMessageService service, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        [HttpPost("CreateMessage")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> CreateMessage(CreateMessage message)
        {
            Account sender = new Account();
            sender.Email = message.SenderEmail;
            var check1 = await _accountService.GetUserByEmailFromAPIAsync(sender);

            Account receiver = new Account();
            receiver.Email = message.ReveiverEmail;
            var check2 = await _accountService.GetUserByEmailFromAPIAsync(receiver);

            if (check1 != null && check2 != null)
            {

                var result = await _service.CreateMessageFromApiAsync(message);
                if (result == true)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        [HttpGet("GetMessages")]
        //[Authorize(Roles = ApplicationRoles.Buyer + ", " + ApplicationRoles.Seller)]
        public async Task<ActionResult<List<Message>>> GetMessagesByReceiverId(string senderEmail, string receiverEmail)
        {
            var receiver = await _accountService.GetUserByEmailFromAPIAsync(new Account() { Email = receiverEmail });
            var sender = await _accountService.GetUserByEmailFromAPIAsync(new Account() { Email = senderEmail });
            if (receiver != null && sender != null)
            {
                var result = await _service.GetMessagesByReceiverIdFromApiAsync(sender, receiver);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return BadRequest("Not found!");
        }
    }
}
