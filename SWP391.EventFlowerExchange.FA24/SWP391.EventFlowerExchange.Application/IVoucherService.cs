using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IVoucherService
    {
        public Task<bool> CreateVoucherFromAPIAsync(CreateVoucher createVoucher);
        public Task<bool> UpdateVoucherFromAPIAsync(Voucher voucher);
        public Task<bool> DeleteVoucherFromAPIAsync(Voucher voucher);
        public Task<Voucher> SearchVoucherByIdFromAPIAsync(Voucher voucher);

        public Task<Voucher> SearchVoucherByCodeFromAPIAsync(string code);
        public Task<List<Voucher>> ViewAllVoucherFromAPIAsync();
    }
}
