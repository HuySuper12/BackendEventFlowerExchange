using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues
{
    public partial class Voucher
    {
        public int VoucherId { get; set; }

        public string Code { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? MinOrderValue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public decimal DiscountValue { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
