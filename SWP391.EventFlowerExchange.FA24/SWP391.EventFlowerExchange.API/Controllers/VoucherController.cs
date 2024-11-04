using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private IVoucherService _service;
        public VoucherController(IVoucherService service)
        {
            _service = service;
        }

        [HttpGet("GetAllVoucher")]
        //[Authorize]
        public async Task<IActionResult> GetAllVoucher()
        {
            return Ok(await _service.ViewAllVoucherFromAPIAsync());
        }

        [HttpGet("GetAllVoucherValid")]
        //[Authorize]
        public async Task<IActionResult> GetAllVoucherValid(decimal price)
        {
            var list = await _service.ViewAllVoucherFromAPIAsync();
            List<Voucher> listVoucher = new List<Voucher>();
            for (int i = 0; i < list.Count; i++)
            {
                if (DateTime.Now < list[i].ExpiryDate && price > list[i].MinOrderValue)
                {
                    listVoucher.Add(list[i]);
                }
            }
            return Ok(listVoucher);
        }

        [HttpGet("SearchVoucherById/{id}")]
        //[Authorize]
        public async Task<IActionResult> SearchVoucherByIdAsync(string id)
        {
            try
            {
                var result = await _service.SearchVoucherByIdFromAPIAsync(new Voucher() { VoucherId = int.Parse(id) });
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("VoucherId is a number");
            }

        }

        [HttpGet("SearchVoucherByCode/{code}")]
        //[Authorize]
        public async Task<IActionResult> SearchVoucherByCodeAsync(string code)
        {
            var result = await _service.SearchVoucherByCodeFromAPIAsync(code);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("CreateVoucher")]
        //[Authorize]
        public async Task<ActionResult<bool>> CreateVoucherAsync(CreateVoucher voucher)
        {
            var voucherResult = await _service.SearchVoucherByCodeFromAPIAsync(voucher.Code);
            if (voucherResult == null)
            {
                return await _service.CreateVoucherFromAPIAsync(voucher);
            }
            return false;
        }

        [HttpPut("UpdateVoucher")]
        //[Authorize]
        public async Task<ActionResult<bool>> UpdateVoucherAsync(Voucher voucher)
        {
            var searchVoucher = await _service.SearchVoucherByIdFromAPIAsync(new Voucher() { VoucherId = voucher.VoucherId });
            if (searchVoucher != null)
            {
                await _service.UpdateVoucherFromAPIAsync(voucher);
                return true;
            }
            return false;
        }

        [HttpDelete("RemoveVoucher/{id}")]
        //[Authorize]
        public async Task<ActionResult<bool>> RemoveVoucherAsync(string id)
        {
            var voucher = await _service.SearchVoucherByIdFromAPIAsync(new Voucher() { VoucherId = int.Parse(id) });
            if (voucher != null)
            {
                await _service.DeleteVoucherFromAPIAsync(voucher);
                return true;
            }
            return false;
        }
    }
}
