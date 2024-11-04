using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public class VoucherRepository : IVoucherRepository
    {

        private Swp391eventFlowerExchangePlatformContext _context;
        public async Task<bool> CreateVoucherAsync(CreateVoucher createVoucher)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var discount = createVoucher.DiscountValue / 100;
            Voucher voucher = new Voucher()
            {
                Code = createVoucher.Code.ToUpper(),
                Description = createVoucher.Description,
                MinOrderValue = createVoucher.MinOrderValue,
                StartDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(createVoucher.ExpiryDate),
                DiscountValue = discount,
                CreatedAt = DateTime.Now,
            };
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteVoucherAsync(Voucher voucher)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Voucher> SearchVoucherByCodeAsync(string code)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Vouchers.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Voucher> SearchVoucherByIdAsync(Voucher voucher)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Vouchers.FirstOrDefaultAsync(x => x.VoucherId == voucher.VoucherId);
        }

        public async Task<bool> UpdateVoucherAsync(Voucher voucher)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Voucher>> ViewAllVoucherAsync()
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            return await _context.Vouchers.ToListAsync();
        }
    }
}
