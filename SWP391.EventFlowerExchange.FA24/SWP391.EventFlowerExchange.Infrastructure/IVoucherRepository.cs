using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IVoucherRepository
    {
        public Task<bool> CreateVoucherAsync(CreateVoucher createVoucher);
        public Task<bool> UpdateVoucherAsync(Voucher voucher);
        public Task<bool> DeleteVoucherAsync(Voucher voucher);
        public Task<Voucher> SearchVoucherByIdAsync(Voucher voucher);

        public Task<Voucher> SearchVoucherByCodeAsync(string code);

        public Task<List<Voucher>> ViewAllVoucherAsync();
    }
}
