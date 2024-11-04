using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateProduct
    {
        [Required]
        public string? SellerEmail { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public int? FreshnessDuration { get; set; }

        [Required]
        public string? ComboType { get; set; }

        [Required]
        public int? Quantity { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Category { get; set; }

        public List<string>? ListImage { get; set; }
    }
}
