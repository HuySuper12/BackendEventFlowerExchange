using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateCartItem
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string BuyerEmail { get; set; }
    }
}
