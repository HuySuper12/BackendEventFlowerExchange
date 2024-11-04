using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateRating
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(100)]
        public string? BuyerEmail { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }
    }
}
