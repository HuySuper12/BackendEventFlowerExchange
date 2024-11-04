using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class GetCartItem
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ComboType { get; set; }

        public string BuyerId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? ProductImage { get; set; }
    }
}
