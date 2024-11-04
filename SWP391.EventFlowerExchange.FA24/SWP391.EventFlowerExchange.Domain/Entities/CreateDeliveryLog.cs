using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateDeliveryLog
    {
        [Required]
        public int? OrderId { get; set; }

        [Required]
        public string? DeliveryPersonEmail { get; set; }

    }
}
