using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class DeliveryTime
    {
        public DateTime? TakeOverTime { get; set; }
        public DateTime? DeliveringTime { get; set; }
        public DateTime? SuccessOrFailTime { get; set; }
    }
}
