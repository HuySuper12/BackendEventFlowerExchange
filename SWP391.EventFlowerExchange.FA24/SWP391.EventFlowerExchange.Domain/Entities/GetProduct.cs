using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class GetProduct
    {
        public int ProductId { get; set; }

        public List<string?> ProductImage { get; set; }

        public string? SellerId { get; set; }

        public string? ProductName { get; set; }

        public int? FreshnessDuration { get; set; }

        public string? ComboType { get; set; }

        public string? Status { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public string? Category { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ExpireddAt { get; set; }

    }
}
