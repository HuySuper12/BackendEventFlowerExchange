using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateVoucher
    {
        public string Code { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? MinOrderValue { get; set; }

        public int ExpiryDate { get; set; }

        public decimal DiscountValue { get; set; }
    }
}
