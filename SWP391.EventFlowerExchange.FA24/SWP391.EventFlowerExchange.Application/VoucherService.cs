using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class VoucherService : IVoucherService
    {

        private IVoucherRepository _repo;
        public VoucherService(IVoucherRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateVoucherFromAPIAsync(CreateVoucher createVoucher)
        {
            return await _repo.CreateVoucherAsync(createVoucher);
        }

        public async Task<bool> DeleteVoucherFromAPIAsync(Voucher voucher)
        {
            return await _repo.DeleteVoucherAsync(voucher);
        }

        public async Task<Voucher> SearchVoucherByCodeFromAPIAsync(string code)
        {
            return await _repo.SearchVoucherByCodeAsync(code);
        }

        public Task<Voucher> SearchVoucherByIdFromAPIAsync(Voucher voucher)
        {
            return _repo.SearchVoucherByIdAsync(voucher);
        }

        public async Task<bool> UpdateVoucherFromAPIAsync(Voucher voucher)
        {
            return await _repo.UpdateVoucherAsync(voucher);
        }

        public async Task<List<Voucher>> ViewAllVoucherFromAPIAsync()
        {
            return await _repo.ViewAllVoucherAsync();
        }
    }
}
